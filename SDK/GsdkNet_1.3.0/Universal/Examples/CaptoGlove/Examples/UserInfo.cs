using GSdkNet.BLE;
using GSdkNet.BLE.Winapi;
using GSdkNet.Board;
using GSdkNet.Peripheral;
using GSdkNet.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace GSdkNet.Carrier.Example {
    class UserInfo: BasicExample {
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
                    var userInfo = await ReadUserInfoAsync();
                    await WriteUserInfoAsync(userInfo);
                }
            }
        }
        
        private async Task<IBoardUserInfo> ReadUserInfoAsync() {
            // Abnormal case there to raise exception so we should not catch it
            await Peripheral.UserInfo.ReadAsync();

            try {
                var addonCoder = new BoardUserInfoCoder();
                var addonInfo = addonCoder.Decode(Peripheral.UserInfo.Value);
                PrintInfo("Peripheral read user info, position: " + addonInfo.Position.ToString());
                return addonInfo;
            } catch {
                // It is normal case, and you should be ready to fact 
                // that user has not run suite to store user info.
                PrintWarning("Failed to decode user info, defaults needed");
                return BoardBodyPosition.RightGlove.PreferredUserInfo();
            }
        }

        private async Task WriteUserInfoAsync(IBoardUserInfo userInfo) {
            var addonCoder = new BoardUserInfoCoder();

            // Abnormal case there to raise exception so we should not catch it
            await Peripheral.UserInfo.WriteAsync(addonCoder.Encode(userInfo));
        }
    }
}
