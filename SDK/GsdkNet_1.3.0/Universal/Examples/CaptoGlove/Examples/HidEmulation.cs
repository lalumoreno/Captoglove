using GSdkNet.BLE;
using GSdkNet.BLE.Winapi;
using GSdkNet.Board;
using GSdkNet.Peripheral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GSdkNet.Carrier.Example {
    class HidEmulation : BasicExample {
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
            await StopHidEmulationAsync();
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
                    await StartHidEmulationAsync();
                }
            } 
        }

        private async Task StartHidEmulationAsync() {
            // Board should be configured previously via Suite
            var emulationModes = new EmulationModes() { Mouse = true };
            await Peripheral.EmulationModes.WriteAsync(emulationModes);
            await Peripheral.EmulationState.WriteAsync(true);
        }

        private async Task StopHidEmulationAsync() {
            await Peripheral?.EmulationState.WriteAsync(false);
        }
    }
}
