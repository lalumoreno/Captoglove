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
using System.IO;

public class SaveSensorData : MonoBehaviour, ILoggerProvider {
    private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral;
    private IBoardPeripheral Peripheral_L;
		
	public float Handx;
	public float Handy;
	public float Handz;
	
	public float pinky;	
	public float ring;	
	public float midd;
	public float idx;
	public float thumb;
	
	public float quaternionX;
	public float quaternionY;
	public float quaternionZ;
	
	private const string SAVE_SEPARATOR = ",";
    // Use this for initialization
    void Start() {
	
        Debug.Log("Start");
		
	    //Get the path of the Game data folder
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("Path : " + m_Path);
		
		Debug.Log("Looking for peripheral");
		
        GSdkNet.BLE.Winapi.Package.LoggerProvider = this;
        GSdkNet.Carrier.Package.LoggerProvider = this;
        var adapterScanner = new AdapterScanner();
        var boardFactory = new BoardFactory(adapterScanner.FindAdapter());
        Central = boardFactory.GetBoardCentral();
        Central.StartScan(new ScanOptions() { PreferredInterval = 5 });
        Central.PeripheralsChanged += Central_PeripheralsChanged; //If peripheral is detected 

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
				
				Debug.Log("--- Calibration");
			//	await Peripheral.CalibrateGyroAsync();
			//	await Peripheral.TareAsync(); //Needed to set direction of the hand 
			//	await Peripheral.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				await Peripheral.StreamTimeslots.ReadAsync();
				StreamTimeslots st = new StreamTimeslots(); 
				//st.Set(0, BoardStreamType.Mouse);
				//Keyboard
				//Joystick
				st.Set(1, BoardStreamType.TaredQuaternion); //This are the 3 axes for hand movement
				st.Set(0, BoardStreamType.AbsoluteAltitude); //This altitude is expressed in meters and absolute.
				st.Set(0, BoardStreamType.LinearAcceleration);
				st.Set(0, BoardStreamType.RawAcceleration);
				st.Set(0, BoardStreamType.CalibratedMagnetism);
				st.Set(0, BoardStreamType.SensorsState);
				st.Set(0, BoardStreamType.GyroRate);
				st.Set(0, BoardStreamType.TaredAltitude);//This altitude is expressed in relative units
				st.Set(0, BoardStreamType.RawMagnetism);
				st.Set(0, BoardStreamType.Pressure);
				
				Peripheral.StreamTimeslots.WriteAsync(st);
				
				await Peripheral.StreamTimeslots.ReadAsync();
            }
				
        }
    }
	
    private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		var skip = true;
		
        if (e.StreamType == BoardStreamType.SensorsState) {
            var args = e as BoardFloatSequenceEventArgs;
            var value = FloatsToString(args.Value);
			List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received sensors state: " + value);
			
			oneValue.Reverse(); // For right hand 

			pinky = float.Parse(oneValue[1]);	
			ring  = float.Parse(oneValue[3]);		
			midd  = float.Parse(oneValue[5]);
			idx   = float.Parse(oneValue[7]);
			thumb = float.Parse(oneValue[9]);
				
			Debug.Log("------------------------------------------------------");
			Debug.Log("p "+pinky); 
			Debug.Log("r "+ring); 
			Debug.Log("m "+midd); 
			Debug.Log("i "+idx); 			
			Debug.Log("t "+thumb); 
			Debug.Log("------------------------------------------------------");
			
        } else if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
            Debug.Log("Received tared quaternion: " + args.Value);
			var value =  args.Value.ToString();
			
			var charsToRemove = new string[] { "(", ")" };
				foreach (var c in charsToRemove)
				{
					value = value.Replace(c, string.Empty);
				}		

				List<string> oneValue = value.Split(',').ToList<string>();
				
				quaternionX  = float.Parse(oneValue[0]);	
				quaternionY  = float.Parse(oneValue[1]);		
				quaternionZ  = float.Parse(oneValue[2]);
			
			//quaternionX
			
        } else if (e.StreamType == BoardStreamType.LinearAcceleration) {
            var args = e as BoardFloatVectorEventArgs;
            Debug.Log("Received linear acceleration: " + args.Value);
        }
		else if (e.StreamType == BoardStreamType.CalibratedMagnetism)
		{
		
				var args = e as BoardFloatVectorEventArgs;
				var value =  args.Value.ToString();
			
				var charsToRemove = new string[] { "(", ")" };
				foreach (var c in charsToRemove)
				{
					value = value.Replace(c, string.Empty);
				}		

				List<string> oneValue = value.Split(',').ToList<string>();
			
				float x  = float.Parse(oneValue[0]);	
				float y  = float.Parse(oneValue[1]);		
				float z  = float.Parse(oneValue[2]);
			
				Handx = x;
				Handy = y;
				Handz = z;
				
				Debug.Log("Received R CalibratedMagnetism state: " + args.Value);
			
		}
    }

	public async void Stop() {
        Debug.Log("Stopping");

        if (Peripheral != null) {
			 Debug.Log("Stop R Peripheral");
            await Peripheral?.EmulationState.WriteAsync(false);
			Peripheral.StreamReceived -= Peripheral_StreamReceived;
            Peripheral.PropertyChanged -= Peripheral_PropertyChanged;
			await Peripheral.StopAsync();
            Peripheral.Dispose();
            Peripheral = null;
        }

        if (Central != null) {
			Debug.Log("Stop Central");
			Central.StopScan();
            Central.PeripheralsChanged -= Central_PeripheralsChanged;
            Central = null;
        }
        //GC.Collect();
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
		
		SaveFile();
	}
	
	  private void SaveFile()
     {
		 /*
		string [] cont = new string[] {
			 ""+Handx,
			 ""+Handy,
			 ""+Handz
		 };
		 */
		 string [] cont = new string[] {
			 ""+quaternionX,
			 ""+quaternionY,
			 ""+quaternionZ
		 };
		 
		/*
		string [] cont = new string[] {
			 ""+pinky,
			 ""+ring,
			 ""+midd,
			 ""+idx,
			 ""+thumb
		 };
			*/
		 string filePath = Application.dataPath + "/others.txt";
		 string savestring = string.Join(SAVE_SEPARATOR, cont);
		 
		 if (!File.Exists(filePath)) 
         {
             File.Create(filePath).Close();
             File.AppendAllText(filePath, "x,y,z"+Environment.NewLine);
         }
		 else 
			File.AppendAllText(filePath, savestring+Environment.NewLine); 
		 
     }

    private void OnDestroy() {
        Debug.Log("OnDestroy");
        Stop();
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
