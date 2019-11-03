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


public class BothHand : MonoBehaviour, ILoggerProvider {

	private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral_R;
	private IBoardPeripheral Peripheral_L;
	
	public Transform   Hand_R;
	public Transform   Hand_L;

	public Vector3   initial_angle_RH;
	public Vector3   initial_angle_LH;
	
	public float   newY_angle_RH;
	public float   newX_angle_RH;
	public float   newZ_angle_RH;

	public float   newY_angle_LH;
	public float   newX_angle_LH;
	public float   newZ_angle_LH;
	
	public float   initial_x_angle_RH;
	public float   initial_x_angle_LH;
	
	public float quaternionX_RH;
	public float quaternionY_RH;
	public float quaternionZ_RH;
	
	public float quaternionX_LH;
	public float quaternionY_LH;
	public float quaternionZ_LH;
	
	// Use this for initialization
	void Start () {
		
		Debug.Log("Start");
		Debug.Log("Looking for peripheral");
		
		initial_angle_RH = Hand_R.localEulerAngles;
		initial_angle_LH = Hand_L.localEulerAngles;
		
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
		
		if(newY_angle_RH < 0)
			Hand_R.transform.localEulerAngles = new Vector3(newZ_angle_RH, newY_angle_RH , -newX_angle_RH );
		else
			Hand_R.transform.localEulerAngles = new Vector3(newX_angle_RH, newY_angle_RH , newZ_angle_RH);
		
		if(newY_angle_LH > 0)
			Hand_L.transform.localEulerAngles = new Vector3(-newZ_angle_LH, 90, newX_angle_LH ); //solve this axe
		else
			Hand_L.transform.localEulerAngles = new Vector3(newX_angle_LH, newY_angle_LH , newZ_angle_LH);
		
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
                Peripheral_R = board;
                Peripheral_R.StreamReceived += Peripheral_StreamReceived_R;
                Peripheral_R.PropertyChanged += Peripheral_PropertyChanged_R;
                await board.StartAsync();
				} //Left Hand
				else if (board.Name == "CaptoGlove2469"){
				Peripheral_L = board;
                Peripheral_L.StreamReceived += Peripheral_StreamReceived_L;
                Peripheral_L.PropertyChanged += Peripheral_PropertyChanged_L;
                await board.StartAsync();				
				}
                return;
            } catch (Exception ex) {
                Debug.Log("Unable to start board " + ex.Message);
            }
        }
    }

	private async void Peripheral_PropertyChanged_R(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_R.Status.ToString());
			
            if (Peripheral_R.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_R.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_R.CalibrateGyroAsync();
				await Peripheral_R.TareAsync();
				await Peripheral_R.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(1, BoardStreamType.TaredQuaternion);
				Peripheral_R.StreamTimeslots.WriteAsync(st);				
				await Peripheral_R.StreamTimeslots.ReadAsync();
            }
        }
    }

	private async void Peripheral_PropertyChanged_L(object sender, PropertyChangedEventArgs e) {
		
		Debug.Log("- Property changed R: " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral_L.Status.ToString());
			
            if (Peripheral_L.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral_L.FirmwareVersion);
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral_L.CalibrateGyroAsync();
				await Peripheral_L.TareAsync();
				await Peripheral_L.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				st.Set(1, BoardStreamType.TaredQuaternion);
				Peripheral_L.StreamTimeslots.WriteAsync(st);				
				await Peripheral_L.StreamTimeslots.ReadAsync();
            }
        }
    }
	
	 private void Peripheral_StreamReceived_R(object sender, BoardStreamEventArgs e) {
		
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
				/*
				a=180f;
				b=0f;				
				
				newX_angle_RH = b + quaternionX*a;//pitch
				newZ_angle_RH = -quaternionY*a;//yaw
				newY_angle_RH = 90+quaternionZ*a;//roll
				*/
				
				/*limitations not big difference*/
				a=150f;
				b=0f;				
				
				newX_angle_RH = b + quaternionX_RH*a;//pitch
				
				
				a=120f;
				b=0f;				
				
				newZ_angle_RH = b-quaternionY_RH*a;//yaw
				
				a=168.75f;
				b=78.75f;				
				
				newY_angle_RH = b+quaternionZ_RH*a;//roll
				
		}
    }
	
	 private void Peripheral_StreamReceived_L(object sender, BoardStreamEventArgs e) {
		
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
			b=0f;				
				
			newX_angle_LH = b - quaternionX_LH*a;//pitch
			newZ_angle_LH = quaternionY_LH*a;//yaw
			newY_angle_LH = -90+quaternionZ_LH*a;//roll
			
				
		}
    }
	
	public async void Stop() {
        Debug.Log("Stopping");
		
        if (Peripheral_R != null) {
			 Debug.Log("Stop Peripheral_R");
            await Peripheral_R?.EmulationState.WriteAsync(false);
			Peripheral_R.StreamReceived -= Peripheral_StreamReceived_R;
            Peripheral_R.PropertyChanged -= Peripheral_PropertyChanged_R;
			await Peripheral_R.StopAsync();
            Peripheral_R.Dispose();
            Peripheral_R = null;
        }
		
		if (Peripheral_L != null) {
			 Debug.Log("Stop L Peripheral");
            await Peripheral_L?.EmulationState.WriteAsync(false);
			Peripheral_L.StreamReceived -= Peripheral_StreamReceived_L;
            Peripheral_L.PropertyChanged -= Peripheral_PropertyChanged_L;
			await Peripheral_L.StopAsync();
            Peripheral_L.Dispose();
            Peripheral_L = null;
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
