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


public class BothArms : MonoBehaviour, ILoggerProvider {

	private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral_RH;
	private IBoardPeripheral Peripheral_LH;
	private IBoardPeripheral Peripheral_RLA;
	private IBoardPeripheral Peripheral_LLA;
	
	public Transform   Hand_R;
	public Transform   Hand_L;
	public Transform   LowArm_R;
	public Transform   LowArm_L;
	public Transform   UpArm_R;
	public Transform   UpArm_L;
	
	public float   newY_angle_RH;
	public float   newX_angle_RH;
	public float   newZ_angle_RH;

	public float   newY_angle_RLA;
	public float   newX_angle_RLA;
	public float   newZ_angle_RLA;

	public float   newY_angle_RUA;
	public float   newX_angle_RUA;
	public float   newZ_angle_RUA;
	
	public float   newY_angle_LH;
	public float   newX_angle_LH;
	public float   newZ_angle_LH;

	public float   newY_angle_LLA;
	public float   newX_angle_LLA;
	public float   newZ_angle_LLA;

	public float   newY_angle_LUA;
	public float   newX_angle_LUA;
	public float   newZ_angle_LUA;
	
	public float quaternionX_RH;
	public float quaternionY_RH;
	public float quaternionZ_RH;

	public float quaternionX_RLA;
	public float quaternionY_RLA;
	public float quaternionZ_RLA;

	public float quaternionX_RUA;
	public float quaternionY_RUA;
	public float quaternionZ_RUA;
	
	public float quaternionX_LH;
	public float quaternionY_LH;
	public float quaternionZ_LH;

	public float quaternionX_LLA;
	public float quaternionY_LLA;
	public float quaternionZ_LLA;
	
	public float quaternionX_LUA;
	public float quaternionY_LUA;
	public float quaternionZ_LUA;
	
	// Use this for initialization
	void Start () {
		/*
		Initial position
		*/
		
		newX_angle_RH = 0;
		newY_angle_RH = 90;
		newZ_angle_RH = 0;
		
		newX_angle_RUA = -100;
		newY_angle_RUA = 0;
		newZ_angle_RUA = 0;
		
		newX_angle_LH = 0;
		newY_angle_LH = -90;
		newZ_angle_LH = 0; 
		
		newX_angle_LUA = 80;
		newY_angle_LUA = 180;
		newZ_angle_LUA = 180;
		
		/*---------------------*/
		
		
		Debug.Log("Start");
		Debug.Log("Looking for peripheral");
		
		GSdkNet.BLE.Winapi.Package.LoggerProvider = this;
        GSdkNet.Carrier.Package.LoggerProvider = this;
        var adapterScanner = new AdapterScanner();
        var boardFactory = new BoardFactory(adapterScanner.FindAdapter());
        Central = boardFactory.GetBoardCentral();
        Central.StartScan(new ScanOptions() { PreferredInterval = 5 });
        Central.PeripheralsChanged += Central_PeripheralsChanged; //If peripheral is detected 
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//Right Hand
		if(newY_angle_RH < 0)
			Hand_R.transform.localEulerAngles = new Vector3(newZ_angle_RH, newY_angle_RH , -newX_angle_RH );
		else
			Hand_R.transform.localEulerAngles = new Vector3(newX_angle_RH, newY_angle_RH , newZ_angle_RH);
		
		
		//Left Hand 
		if(newY_angle_LH > 0)
			Hand_L.transform.localEulerAngles = new Vector3(-newZ_angle_LH, 90, newX_angle_LH ); //solve this axe
		else
			Hand_L.transform.localEulerAngles = new Vector3(newX_angle_LH, newY_angle_LH , newZ_angle_LH);
		
		//Low Arms
		LowArm_R.transform.localEulerAngles = new Vector3(newX_angle_RLA, newY_angle_RLA , 0);
		LowArm_L.transform.localEulerAngles = new Vector3(newX_angle_LLA, 0 , 0);
		
		//Up Arms 
		UpArm_L.transform.localEulerAngles = new Vector3(newX_angle_LUA, newY_angle_LUA, newZ_angle_LUA);
		UpArm_R.transform.localEulerAngles = new Vector3(newX_angle_RUA, newY_angle_RUA, newZ_angle_RUA);
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
		
				//Right Hand 
				if(board.Name == "CaptoGlove2480"){
                Peripheral_RH = board;
                Peripheral_RH.StreamReceived += Peripheral_StreamReceived_RH;
                Peripheral_RH.PropertyChanged += Peripheral_PropertyChanged_RH;
                await Peripheral_RH.StartAsync();
				} //Left Hand
				else if (board.Name == "CaptoGlove2469"){
				Peripheral_LH = board;
                Peripheral_LH.StreamReceived += Peripheral_StreamReceived_LH;
                Peripheral_LH.PropertyChanged += Peripheral_PropertyChanged_LH;
                await Peripheral_LH.StartAsync();				
				} //Right Low Arm
				else if (board.Name == "CaptoGlove2443"){
				Peripheral_RLA = board;
                Peripheral_RLA.StreamReceived += Peripheral_StreamReceived_RLA;
                Peripheral_RLA.PropertyChanged += Peripheral_PropertyChanged_RLA;
                await Peripheral_RLA.StartAsync();				
				} //Left Low Arm
				else if (board.Name == "CaptoGlove2502"){
				Peripheral_LLA = board;
                Peripheral_LLA.StreamReceived += Peripheral_StreamReceived_LLA;
                Peripheral_LLA.PropertyChanged += Peripheral_PropertyChanged_LLA;
                await Peripheral_LLA.StartAsync();				
				}
				
                return;
            } catch (Exception ex) {
                Debug.Log("Unable to start board " + ex.Message);
            }
        }
    }

	private async void Peripheral_PropertyChanged_RH(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_RH.Status.ToString());
			
            if (Peripheral_RH.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_RH.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_RH.CalibrateGyroAsync();
				await Peripheral_RH.TareAsync();
				await Peripheral_RH.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(1, BoardStreamType.TaredQuaternion);
				await Peripheral_RH.StreamTimeslots.WriteAsync(st);				
				await Peripheral_RH.StreamTimeslots.ReadAsync();
            }
        }
    }

	private async void Peripheral_PropertyChanged_RLA(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_RLA.Status.ToString());
			
            if (Peripheral_RLA.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_RLA.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_RLA.CalibrateGyroAsync();
				await Peripheral_RLA.TareAsync();
				await Peripheral_RLA.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(3, BoardStreamType.TaredQuaternion);
				await Peripheral_RLA.StreamTimeslots.WriteAsync(st);				
				await Peripheral_RLA.StreamTimeslots.ReadAsync();
            }
        }
    }
	
	private async void Peripheral_PropertyChanged_LH(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_LH.Status.ToString());
			
            if (Peripheral_LH.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_LH.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_LH.CalibrateGyroAsync();
				await Peripheral_LH.TareAsync();
				await Peripheral_LH.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(1, BoardStreamType.TaredQuaternion);
				await Peripheral_LH.StreamTimeslots.WriteAsync(st);				
				await Peripheral_LH.StreamTimeslots.ReadAsync();
            }
        }
    }
	
	private async void Peripheral_PropertyChanged_LLA(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_LLA.Status.ToString());
			
            if (Peripheral_LLA.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_LLA.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_LLA.CalibrateGyroAsync();
				await Peripheral_LLA.TareAsync();
				await Peripheral_LLA.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(1, BoardStreamType.TaredQuaternion);
				await Peripheral_LLA.StreamTimeslots.WriteAsync(st);				
				await Peripheral_LLA.StreamTimeslots.ReadAsync();
            }
        }
    }	
	
	private void Peripheral_StreamReceived_RH(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
	
		float a;
		float b;
		
		if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
			
            Debug.Log("Received tared quaternion: " + args.Value);
			
			var value =  args.Value.ToString();			
			var charsToRemove = new string[] { "(", ")" };
			foreach (var c in charsToRemove)
			{
				value = value.Replace(c, string.Empty);
			}		

			List<string> oneValue = value.Split(',').ToList<string>();
				
			quaternionX_RH  = float.Parse(oneValue[0]);	
			quaternionY_RH  = float.Parse(oneValue[1]);		
			quaternionZ_RH  = float.Parse(oneValue[2]);
				
			/* no limitations*/
				
			a=180f;
			b=90f;				
				
			newX_angle_RH = quaternionX_RH*a;	//pitch
			newZ_angle_RH = -quaternionY_RH*a;	//yaw
	
			newY_angle_RH = quaternionZ_RH*a + b;//roll	
		}
    }
	
	private void Peripheral_StreamReceived_RLA(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
	
		float a;
		float b;
		
		if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
			
            Debug.Log("Received tared quaternion: " + args.Value);
			
			var value =  args.Value.ToString();			
			var charsToRemove = new string[] { "(", ")" };
			foreach (var c in charsToRemove)
			{
				value = value.Replace(c, string.Empty);
			}		

			List<string> oneValue = value.Split(',').ToList<string>();
				
			quaternionX_RLA  = float.Parse(oneValue[0]);	
			quaternionY_RLA  = float.Parse(oneValue[1]);		
			quaternionZ_RLA  = float.Parse(oneValue[2]);
				
			/* no limitations*/
			a=180f;
			b=0f;				
				
			newX_angle_RLA = -quaternionY_RLA*a; //yaw 
			newY_angle_RLA = quaternionZ_RLA*a;	//roll - only low arm	
			newZ_angle_RUA = quaternionX_RLA*(-90f);  // pitch  in up arm 

		}
    }
	
	private void Peripheral_StreamReceived_LH(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
	
		float a;
		float b;
		
		if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
			
            Debug.Log("Received tared quaternion: " + args.Value);
			
			var value =  args.Value.ToString();			
			var charsToRemove = new string[] { "(", ")" };
			foreach (var c in charsToRemove)
			{
				value = value.Replace(c, string.Empty);
			}		

			List<string> oneValue = value.Split(',').ToList<string>();
				
			quaternionX_LH  = float.Parse(oneValue[0]);	
			quaternionY_LH  = float.Parse(oneValue[1]);		
			quaternionZ_LH  = float.Parse(oneValue[2]);
				
			/* no limitations*/
				
			a=180f;
			b=90f;				
				
			newX_angle_LH = -quaternionX_LH*a;	//pitch
			newZ_angle_LH = quaternionY_LH*a;	//yaw
			newY_angle_LH = quaternionZ_LH*a - b;//roll
			//newY_angle_LLA = quaternionZ_LH*a;	// roll low arm	
			//newY_angle_LUA = -quaternionZ_LH*a + 360;
			
				
		}
    }

	private void Peripheral_StreamReceived_LLA(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
	
		float a;
		float b;
		
		if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
			
            Debug.Log("Received tared quaternion: " + args.Value);
			
			var value =  args.Value.ToString();			
			var charsToRemove = new string[] { "(", ")" };
			foreach (var c in charsToRemove)
			{
				value = value.Replace(c, string.Empty);
			}		

			List<string> oneValue = value.Split(',').ToList<string>();
				
			quaternionX_LLA  = float.Parse(oneValue[0]);	
			quaternionY_LLA  = float.Parse(oneValue[1]);		
			quaternionZ_LLA  = float.Parse(oneValue[2]);
				
			/* no limitations*/
			a=180f;
			b=0f;				
			
			//newZ_angle_LUA = -quaternionX_LLA*a + a;			
			//newX_angle_LLA = -quaternionY_LLA*a;	//yaw 
			//newY_angle_LLA = quaternionZ_LLA*a;	//roll - only low arm		
			newZ_angle_LUA = quaternionX_LLA*(-90f)+180f;
				
				
		}
    }
	
	public async void Stop() {
        Debug.Log("Stopping");
		
        if (Peripheral_RH != null) {
			 Debug.Log("Stop RH Peripheral");
            await Peripheral_RH?.EmulationState.WriteAsync(false);
			Peripheral_RH.StreamReceived -= Peripheral_StreamReceived_RH;
            Peripheral_RH.PropertyChanged -= Peripheral_PropertyChanged_RH;
			await Peripheral_RH.StopAsync();
            Peripheral_RH.Dispose();
            Peripheral_RH = null;
        }
		
		if (Peripheral_LH != null) {
			 Debug.Log("Stop LH Peripheral");
            await Peripheral_LH?.EmulationState.WriteAsync(false);
			Peripheral_LH.StreamReceived -= Peripheral_StreamReceived_LH;
            Peripheral_LH.PropertyChanged -= Peripheral_PropertyChanged_LH;
			await Peripheral_LH.StopAsync();
            Peripheral_LH.Dispose();
            Peripheral_LH = null;
        }
		
		if (Peripheral_RLA != null) {
			 Debug.Log("Stop RLA Peripheral");
            await Peripheral_RLA?.EmulationState.WriteAsync(false);
			Peripheral_RLA.StreamReceived -= Peripheral_StreamReceived_RLA;
            Peripheral_RLA.PropertyChanged -= Peripheral_PropertyChanged_RLA;
			await Peripheral_RLA.StopAsync();
            Peripheral_RLA.Dispose();
            Peripheral_RLA = null;
        }
		
		if (Peripheral_LLA != null) {
			 Debug.Log("Stop LLA Peripheral");
            await Peripheral_LLA?.EmulationState.WriteAsync(false);
			Peripheral_LLA.StreamReceived -= Peripheral_StreamReceived_LLA;
            Peripheral_LLA.PropertyChanged -= Peripheral_PropertyChanged_LLA;
			await Peripheral_LLA.StopAsync();
            Peripheral_LLA.Dispose();
            Peripheral_LLA = null;
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

	private void OnDestroy() {
        Debug.Log("OnDestroy");
        Stop();
    }	
	
	 public GSdkNet.Base.Core.ILogger GetLogger(string name) {
        return new Logger();
    }

    private sealed class Logger : GSdkNet.Base.Core.ILogger {
        public void Debug(string message) {
        //    UnityEngine.Debug.Log(message);
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
