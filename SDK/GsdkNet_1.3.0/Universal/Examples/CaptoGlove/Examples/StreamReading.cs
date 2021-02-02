using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GSdkNet.BLE.Winapi;
using GSdkNet.Peripheral;
using GSdkNet.Board;
using GSdkNet.BLE;
using System.Collections.Generic;
using GSdkNet.Core;

namespace GSdkNet.Carrier.Example
{
    class StreamReading : BasicExample
    {
        private IPeripheralCentral Central;
        private IBoardPeripheral Peripheral;

        public override Task StartAsync() {
            var adapterScanner = new AdapterScanner();
            var adapter = adapterScanner.FindAdapter();
            var configurator = new Configurator(adapter);

            Central = configurator.GetBoardCentral();
            Central.StartScan( new Dictionary<PeripheralScanFlag, object> {
                { PeripheralScanFlag.ScanType, BleScanType.Balanced }
            });
            Central.PeripheralsChanged += Central_PeripheralsChanged;
            return Task.FromResult(Type.Missing);
        }

        public override async Task StopAsync() {
            Central.StopScan();
            if (Peripheral != null) {
                await Peripheral.StopAsync();
                Peripheral.StreamReceived -= Peripheral_StreamReceived;
            }
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
                    Peripheral.StreamReceived += Peripheral_StreamReceived;
                    await Peripheral.StartAsync();
                    return;
                }
                catch (Exception ex) {
                    Console.WriteLine("Unable to start board " + ex.Message);
                }
            }
        }

        private async void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == PeripheralProperty.Status) {
                Console.WriteLine("Board status: " + Peripheral.Status.ToString());
                if (Peripheral.Status == PeripheralStatus.Connected) {
                    // You can assign to 20 timeslots (StreamTimeslots.MaxTimeslots)
                    // distributed between different measurement. Please take a note,
                    // HID mouse, joystick and keyboard timeslots are dedicated from
                    // the shared amount of timeslots.
                    var streamTimeslots = new StreamTimeslots();
                    streamTimeslots.Set(3, BoardStreamType.TaredQuaternion);
                    streamTimeslots.Set(3, BoardStreamType.SensorsState);
                    streamTimeslots.Set(3, BoardStreamType.LinearAcceleration);
                    await Peripheral.StreamTimeslots.WriteAsync(streamTimeslots);
                }
            }
            else {
                PrintInfo("Changed peripheral property " + e.PropertyName);
            }
        }

        private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e) {
            // On stream callback. Stream can contain different kind of arguments.
            // Please bear in mind, this stream is not necessary should be run on main thread.
            // In case of not necessary stream type you just skioing it handling.
            if (e.StreamType == BoardStreamType.TaredQuaternion) {
                // Reading tared quaternion data
                var args = e as BoardQuaternionEventArgs;
                var quaternion = args.Value;
                var noseVector = quaternion.Rotate(new Vector3f(0, 0, 1));
                var noseDirection = noseVector.PrimaryDirection();
                PrintInfo("Received quaternion, which rotates board nose to " + noseDirection.ToString());
            }
            else if (e.StreamType == BoardStreamType.SensorsState) {
                // Reading sensors state data
                var args = e as BoardFloatSequenceEventArgs;
                var value = FloatsToString(args.Value);
                PrintInfo("Received sensors " + value);
            }
            else if (e.StreamType == BoardStreamType.LinearAcceleration) {
                // Reading linear acceleration data
                var args = e as BoardFloatVectorEventArgs;
                var acceleration = args.Value;
                var primaryDirection = acceleration.PrimaryDirection();
                PrintInfo("Received acceleration in direction " + acceleration.ToString());
            }
        }

        private static string FloatsToString(float[] value) {
            string result = "";
            var index = 0;
            foreach (var element in value) {
                if (index != 0) {
                    result += ", ";
                }
                result += element.ToString();
                index += 1;
            }
            return result;
        }
    }
}
