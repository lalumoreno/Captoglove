using GSdkNet.Base.Board;
using GSdkNet.Base.Core;
using GSdkNet.Base.Peripheral;
using GSdkNet.Carrier;
using GSdkNet.BLE.Winapi;
using UnityEngine;
using System.ComponentModel;
using System;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Linq;


public class GloveScript : MonoBehaviour, ILoggerProvider {
    private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral;
	
	public Transform Dedo;
	public Vector3 temp;
	public var newval;
    // Use this for initialization
    void Start() {
        Debug.Log("Start");
        Debug.Log("Looking for peripheral");
		temp = Dedo.localEulerAngles;
		newval = 0;
        GSdkNet.BLE.Winapi.Package.LoggerProvider = this;
        GSdkNet.Carrier.Package.LoggerProvider = this;
        var adapterScanner = new AdapterScanner();
        var boardFactory = new BoardFactory(adapterScanner.FindAdapter());
        Central = boardFactory.GetBoardCentral();
        Central.StartScan(new ScanOptions() { PreferredInterval = 5 });
        Central.PeripheralsChanged += Central_PeripheralsChanged;

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
		
		 Debug.Log("- Property changed : " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
            Debug.Log("Board status: " + Peripheral.Status.ToString());
            if (Peripheral.Status == PeripheralStatus.Connected) {
				
				await Peripheral.BatteryLevel.ReadAsync();
				await Peripheral.Temperature.ReadAsync();			
				await Peripheral.StreamTimeslots.ReadAsync();
				await Peripheral.EmulationModes.ReadAsync();
				await Peripheral.EmulationState.ReadAsync();
				await Peripheral.UserInfo.ReadAsync();
				
				ConfigSensors();
				
		//		await Task.Delay(2000);
			/*	
                await Peripheral.StreamTimeslots.WriteAsync(new StreamTimeslots() {
					Mouse = 6, 
					Keyboard = 6, 
					Joystick = 6, 
					TaredQuaternion = 0, 
					AbsoluteAltitude = 0, 
					TaredAltitude = 0, 
					LinearAcceleration = 0, 
					SensorsState = 0, 
					RawAcceleration = 0, 
					CalibratedMagnetism = 1, 
					RawMagnetism = 0, 
					Pressure = 0,
                });
				*/
			//	await Peripheral.StreamTimeslots.ReadAsync();
		/*		
				for (int i = 0; i < 10; i += 1) {
                    await Peripheral.SensorDescriptors[i].ReadAsync();
				}
				Debug.Log("Peripheral wrote balanced sensor descriptors");
       */
				
            }
        }
    }

    private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		
        if (e.StreamType == BoardStreamType.SensorsState) {
            var args = e as BoardFloatSequenceEventArgs;
            var value = FloatsToString(args.Value);
			List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received sensors state: " + value);
			
			oneValue.Reverse(); // For right hand 

            Debug.Log("Pequeño: " + (oneValue[1]));
			Debug.Log("Anular : " + (oneValue[3]));
			Debug.Log("Medio  : " + (oneValue[5]));
			Debug.Log("Indice : " + (oneValue[7]));
			Debug.Log("Presion: " + (oneValue[8]));
	//		Debug.Log("Pulgar : " + (oneValue[9]));
		//	oneValue[7].Replace('.', ',');
			float indexval = float.Parse(oneValue[7]);
			
			Debug.Log("------------------------------------------------------");
			Debug.Log(indexval);
			Debug.Log((indexval-4187.436f));
			Debug.Log("------------------------------------------------------");
			
			
		//	Dedos[0].eulerAngles = new Vector3(45,45,45);
		//	Vector3 temp1 = Dedos[1].localEulerAngles;
		//	Debug.Log("------------------------------------------------------");
		//	temp= new Vector3((indexval-4187.436f)*90f,temp.y,temp.z);
		//	Debug.Log("------------------------------------------------------");
		//	Dedos[0].localEulerAngles=temp;
			
		//	Vector3 temp = new Vector3((indexval-4187.436f)*90f,0,0);
		//	Debug.Log("------------------------------------------------------");
	
			newval = (indexval-5.436f)*90f;
		//Debug.Log("TEMP " + temp.x);
		
		

			Debug.Log("------------------------------------------------------");
			
        } else if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
            Debug.Log("Received tared quaternion: " + args.Value);
			
        } else if (e.StreamType == BoardStreamType.LinearAcceleration) {
            var args = e as BoardFloatVectorEventArgs;
            Debug.Log("Received linear acceleration: " + args.Value);
        }
    }

    public async void WriteBalancedDescriptorParametersAsync() {
		/*
        for (int i = 0; i < 10; i += 1) {
            var descriptor = SensorDescriptor.Predefined(SensorConfiguration.Balanced);
            await Peripheral.SensorDescriptors[i].WriteAsync(descriptor);
        }
		
        Debug.Log("Peripheral wrote balanced sensor descriptors");
		*/
		for (int i = 0; i < 10; i += 1) {
			//var descriptor = SensorDescriptor.Predefined(SensorConfiguration.Balanced);
            await Peripheral.SensorDescriptors[i].ReadAsync();
        }
		
        Debug.Log("Peripheral read balanced sensor descriptors");
    
		Debug.Log("- Init simulation");
	//	await Peripheral.EmulationState.WriteAsync(true);
				
    }
 
	public async void Stop() {
        Debug.Log("Stopping");

        if (Peripheral != null) {
            await Peripheral?.EmulationState.WriteAsync(false);
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
		
		if(newval!= 0)
			Dedo.transform.localEulerAngles = new Vector3(temp.x,temp.y,newval);
    }

    private void OnDestroy() {
        Debug.Log("OnDestroy");
        Stop();
    }

    private void ConfigSensors() {
        Debug.Log("ConfigSensors");
        WriteBalancedDescriptorParametersAsync();
    }
	
    public GSdkNet.Base.Core.ILogger GetLogger(string name) {
        return new Logger();
    }

    private sealed class Logger : GSdkNet.Base.Core.ILogger {
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
