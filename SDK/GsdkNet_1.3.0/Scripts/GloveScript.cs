using GSdkNet;
using GSdkNet.BLE;
using GSdkNet.BLE.Winapi;
using GSdkNet.Board;
using GSdkNet.Logging;
using GSdkNet.Peripheral;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Security.Permissions;

public class GloveScript : MonoBehaviour, ILoggerProvider {
    private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral;

    // Use this for initialization
    void Start() {
        Debug.Log("Start");
        Debug.Log("Looking for peripheral");
        LogManager.LoggerProvider = this;

        var adapterScanner = new AdapterScanner();
        var adapter = adapterScanner.FindAdapter();
        var configurator = new Configurator(adapter);

        Central = configurator.GetBoardCentral();
        Central.PeripheralsChanged += Central_PeripheralsChanged;
        Central.StartScan(new Dictionary<PeripheralScanFlag, object> {
                { PeripheralScanFlag.ScanType, BleScanType.Balanced }
            });
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


    private async void Central_PeripheralsChanged(object sender, PeripheralsEventArgs e) {
        foreach (var peripheral in e.Inserted) {
            // Enumerate peripherals and run first connected
            try {
                var board = peripheral as IBoardPeripheral;
                Debug.Log("Trying to connect peripheral");
                Debug.Log("- ID: " + board.Id);
                Debug.Log("- Name: " + board.Name);

                Peripheral = board;
                Peripheral.StreamReceived += Peripheral_StreamReceived;
                Peripheral.PropertyChanged += Peripheral_PropertyChanged;
                await board.StartAsync();
                return;
            } catch (Exception ex) {
                Debug.Log("Unable to start board " + ex.Message);
            }
        }
    }

    private async void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == PeripheralProperty.Status) {
            Debug.Log("Board status: " + Peripheral.Status.ToString());
            if (Peripheral.Status == PeripheralStatus.Connected) {
                await Peripheral.StreamTimeslots.WriteAsync(new StreamTimeslots() {
                    SensorsState = 6,
                    TaredQuaternion = 6,
                    LinearAcceleration = 6,
                });
            }
        }
    }

    private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e) {
        if (e.StreamType == BoardStreamType.SensorsState) {
            var args = e as BoardFloatSequenceEventArgs;
            var value = FloatsToString(args.Value);
            Debug.Log("Received sensors state: " + value);
        } else if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
            Debug.Log("Received tared quaternion: " + args.Value);
        } else if (e.StreamType == BoardStreamType.LinearAcceleration) {
            var args = e as BoardFloatVectorEventArgs;
            Debug.Log("Received linear acceleration: " + args.Value);
        }
    }

    public async void Stop() {
        Debug.Log("Stopping");

        if (Peripheral != null) {
            Peripheral.StreamReceived -= Peripheral_StreamReceived;
            Peripheral.PropertyChanged -= Peripheral_PropertyChanged;
            await Peripheral.StopAsync();
            Peripheral.Dispose();
            Peripheral = null;
        }

        if (Central != null) {
            Central.PeripheralsChanged -= Central_PeripheralsChanged;
            Central = null;
        }
        GC.Collect();
        Debug.Log("Stopped");
    }

    void OnDisable() {
        Debug.Log("OnDisable");
    }

    void OnEnable() {
        Debug.Log("OnEnable");
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDestroy() {
        Debug.Log("OnDestroy");
        Stop();
    }

    public GSdkNet.Logging.ILogger GetLogger(string name) {
        return new Logger();
    }

    private sealed class Logger : GSdkNet.Logging.ILogger {
        public void Debug(string message) {
            UnityEngine.Debug.Log(message);
        }

        public void Error(string message) {
            UnityEngine.Debug.LogError(message);
        }

        public void Warning(string message) {
            UnityEngine.Debug.Log(message);
        }

        public void Info(string message) {
            UnityEngine.Debug.Log(message);
        }

        public void Trace(string message) {
            UnityEngine.Debug.Log(message);
        }
    }
}
