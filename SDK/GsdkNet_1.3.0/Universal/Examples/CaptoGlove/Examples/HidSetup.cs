using GSdkNet.BLE;
using GSdkNet.BLE.Winapi;
using GSdkNet.Board;
using GSdkNet.BoardSetup;
using GSdkNet.Peripheral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GSdkNet.Carrier.Example {
    class HidSetup : BasicExample {
        private IPeripheralCentral Central;
        private IBoardPeripheral Peripheral;

        public override Task StartAsync() {
            var adapterScanner = new AdapterScanner();
            var adapter = adapterScanner.FindAdapter();
            var configurator = new Configurator(adapter);

            Central = configurator.GetBoardCentral();
            Central.StartScan(new Dictionary<PeripheralScanFlag, object> {
                { PeripheralScanFlag.ScanType, BleScanType.Balanced }
            });
            Central.PeripheralsChanged += Central_PeripheralsChanged;
            return Task.FromResult(Type.Missing);
        }

        public override async Task StopAsync() {
            Central.StopScan();
            await Peripheral?.StopAsync();
        }

        private async void Central_PeripheralsChanged(object sender, PeripheralsEventArgs e) {
            foreach (var peripheral in e.Inserted) {
                // Enumerate peripherals and run first connected
                try {
                    PrintInfo("Trying to connect peripheral");
                    PrintInfo("- ID: " + peripheral.Id);
                    PrintInfo("- Name: " + peripheral.Name);

                    Peripheral = peripheral as IBoardPeripheral;
                    Peripheral.PropertyChanged += Peripheral_PropertyChanged;
                    await Peripheral.StartAsync();
                    return;
                } catch (Exception ex) {
                    Console.WriteLine("Unable to start board " + ex.Message);
                }
            }
        }

        private async void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == PeripheralProperty.Status) {
                Console.WriteLine("Board status: " + Peripheral.Status.ToString());
                if (Peripheral.Status == PeripheralStatus.Connected) {
                    await ConfigureHidParametersAsync();
                }
            }
        }

        private async Task ConfigureHidParametersAsync() {
            var setup = new InputAxisSetup();
            await Peripheral.WriteInputAxisSetupAsync(setup);
        }
    }

    static class BoardPeripheralExtensions {
        public static IInputAxisSetup GetInputAxisSetup(this IBoardPeripheral peripheral) {
            var value = new InputAxisSetup();
            var triggerLinks = new InputAxisLink[BoardDescriptor.InputAxisCount];
            for (int index = 0; index < BoardDescriptor.AccelAxisCount; index += 1) {
                triggerLinks[index] = new InputAxisLink {
                    Info = peripheral.InputAxisInfos[index].Value,
                    PrimaryTrigger = peripheral.InputAxisTriggers[index, 0].Value,
                    SecondaryTrigger = peripheral.InputAxisTriggers[index, 1].Value
                };
            }
            value.TriggerLinks = triggerLinks;
            return value;
        }
        
        public static async Task ReadInputAxisSetupAsync(this IBoardPeripheral peripheral) {
            for (int index = 0; index < BoardDescriptor.InputAxisCount; index += 1) {
                await peripheral.InputAxisInfos[index].ReadAsync();
                await peripheral.InputAxisTriggers[index, 0].ReadAsync();
                await peripheral.InputAxisTriggers[index, 1].ReadAsync();
            }
        }

        public static async Task WriteInputAxisSetupAsync(this IBoardPeripheral peripheral, IInputAxisSetup value) {
            for (int index = 0; index < BoardDescriptor.InputAxisCount; index += 1) {
                var triggerLink = value.TriggerLinks[index];
                await peripheral.InputAxisInfos[index].WriteAsync(triggerLink.Info);
                await peripheral.InputAxisTriggers[index, 0].WriteAsync(triggerLink.PrimaryTrigger);
                await peripheral.InputAxisTriggers[index, 1].WriteAsync(triggerLink.SecondaryTrigger);
            }
        }
    }
}
