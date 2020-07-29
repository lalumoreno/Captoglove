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

public class RHand : MonoBehaviour, ILoggerProvider {
    private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral;
	 private IBoardPeripheral Peripheral_L;
	
	public Transform   Hand_R;

	public Vector3   initial_angle_RH;
	
	public float   newY_angle_RH;
	public float   newX_angle_RH;
	public float   newZ_angle_RH;
	
	public float   initial_x_angle_RH;
	
	public float   prev_sensor_x_RH;
	public float   prev_sensor_y_RH;
	public float   prev_sensor_z_RH;
	
	public float sensorX_RH;
	public float sensorY_RH;
	public float sensorZ_RH;
	
	public float   stable_RH_x;
	public float   stable_RH_y;
	public float   stable_RH_z;
	
	public float stable_coor_X;
	public float stable_coor_y;
	public float stable_coor_z;
	
	private const string SAVE_SEPARATOR = ",";
    // Use this for initialization
    void Start() {
			
        Debug.Log("Start");
		Debug.Log("Looking for peripheral");
		
		stable_RH_x = 1.26f;
		stable_RH_x = -23f;
		stable_RH_y = -47.84f; //Value of sensor when hand is stable
		stable_RH_z = 54.43f;
		
		initial_angle_RH = Hand_R.localEulerAngles;
			
		newY_angle_RH = 0;
		newX_angle_RH = 0;
		newZ_angle_RH = 0;

		prev_sensor_x_RH = 0;
		prev_sensor_y_RH = 0;
		prev_sensor_z_RH = 0;
		
		stable_coor_X = 0;
		stable_coor_y = 90;
		stable_coor_z = 0;
		
		//InvokeRepeating("Update", 1.0f, 0.3f);
		
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
		
				//Right Hand 
				if(board.Name == "CaptoGlove2480"){
                Peripheral = board;
                Peripheral.StreamReceived += Peripheral_StreamReceived;
                Peripheral.PropertyChanged += Peripheral_PropertyChanged;
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

    private async void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e) {
		
		 Debug.Log("- Property changed : " + e.PropertyName.ToString());  
		 
        if (e.PropertyName == PeripheralProperty.Status) {
			
            Debug.Log("Board status: " + Peripheral.Status.ToString());
			
            if (Peripheral.Status == PeripheralStatus.Connected) {
				
				Debug.Log("----- Read Attributes -------"); 
				
				Debug.Log("FW: " + Peripheral.FirmwareVersion);
				await Peripheral.MagnetismUsed.ReadAsync();
				//await Peripheral.AutoGyroBiasUsed.ReadAsync(); //invalid register
				//await Peripheral.AutoGyroBiasTared.ReadAsync();//invalid
				
				await Peripheral.StreamTimeslots.ReadAsync();
				await Peripheral.BatteryLevel.ReadAsync();
				await Peripheral.Temperature.ReadAsync();			
				
				await Peripheral.SerialNumber.ReadAsync();
				
				await Peripheral.UserInfo.ReadAsync();
				await Peripheral.IndicatorColor.ReadAsync();
				
				//await Peripheral.AccelerometerCalibration.ReadAsync(); //invalid register
				//await Peripheral.MagnetometerCalibration.ReadAsync();//invalid
				//await Peripheral.GyroCalibration.ReadAsync();//invalid
		
				for (int i = 0; i < 10; i += 1) {
					await Peripheral.SensorDescriptors[i].ReadAsync();
				}
				
				//await Peripheral.MotionControllerConfig.ReadAsync();//not working
				
				
				await Peripheral.EmulationState.ReadAsync();
				await Peripheral.EmulationModes.ReadAsync();
				
				for (int i = 0; i < 3; i += 1) {
					await Peripheral.MouseMotionLinks[i].ReadAsync(); 
					await Peripheral.JoystickMotionLinks[i].ReadAsync(); 
					await Peripheral.AccelAxisInfos[i].ReadAsync();
					await Peripheral.InputAxisInfos[i].ReadAsync();
					await Peripheral.BarometerTriggers[i].ReadAsync();
				}
				
				//await Peripheral.SensorTriggers.ReadAsync(); //2 dimensions list 
				//await Peripheral.AccelTriggers.ReadAsync(); //2 dimensions
				//await Peripheral.InputAxisTriggers.ReadAsync(); //2 dimensions
				
				
				
				Debug.Log("--- Test Functions ");
				
				await Peripheral.CalibrateGyroAsync();
				await Peripheral.TareAsync();
				//await SetConnectionIntervalAsync(); // not sure what is this 
				await Peripheral.CommitChangesAsync();
				
				Debug.Log("--- Set timeslot");
				StreamTimeslots st = new StreamTimeslots(); 
				//st.Set(0, BoardStreamType.Mouse);
				//Keyboard
				//Joystick
				st.Set(1, BoardStreamType.TaredQuaternion);
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

	private async void Peripheral_PropertyChanged_L(object sender, PropertyChangedEventArgs e) {
		
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

            }
        }
    }
	
    private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		var skip = true;
		
		if(e.StreamType == BoardStreamType.GyroRate)
		{
				var args = e as BoardFloatVectorEventArgs;
				var value =  args.Value.ToString();
		Debug.Log("Received GyroRate state: " + args.Value);
		}
		if(e.StreamType == BoardStreamType.TaredAltitude)
		{
					var args = e as BoardFloatVectorEventArgs;
				var value =  args.Value.ToString();
		Debug.Log("Received TaredAltitude state: " + args.Value);
		}
	/*
		//RIGHT HAND
		if (e.StreamType == BoardStreamType.CalibratedMagnetism)
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
			
				sensorX_RH = x;
				sensorY_RH = y;
				sensorZ_RH = z;
			
				float a = 0;
				float b = 0;
				float margin5 = 5f;
				float margin10 = 10f;
				float margin3 = 3.5f;
				
				//Pitch
				if (y < (prev_sensor_y_RH - margin5) || y > (prev_sensor_y_RH + margin5))
					{
						if(y>stable_RH_y)
						{
							a=0.948549995f;
							b=45.38072779f;

							newX_angle_RH = b + y*a;
				
							if (newX_angle_RH <-80f)
								newX_angle_RH = -80f;
				
							if(newX_angle_RH > 80f)
								newX_angle_RH = 80f;
						
						}					
						else
						{
							a=1.78322043f;
							b=85.31320574f;

							newX_angle_RH = b + y*a;
				
							if (newX_angle_RH <-80f)
								newX_angle_RH = -80f;
				
							if(newX_angle_RH > 80f)
								newX_angle_RH = 80f;
						}
							
						
						prev_sensor_y_RH = y;
					}
				
					//Yaw
					if (x < (prev_sensor_x_RH - margin5) || x > (prev_sensor_x_RH + margin5))
					{
						if(x>stable_RH_x)
						{
							a=0.718601453f;
							b=-1.397669563f;

							newZ_angle_RH = b + x*a;
				
							if (newZ_angle_RH <-40f)
								newZ_angle_RH = -40f;
				
							if(newZ_angle_RH > 40f)
								newZ_angle_RH = 40f;
						}
						else{
							a=0.764980052f;
							b=-1.487875276f;

							newZ_angle_RH = b + x*a;
				
							if (newZ_angle_RH <-40f)
								newZ_angle_RH = -40f;
				
							if(newZ_angle_RH > 40f)
								newZ_angle_RH = 40f;						
							
						}					
						
						prev_sensor_x_RH = x;
								
					}	
					
					prev_sensor_z_RH = z;					
			/*
				//If z is changing
				if(z < (stable_RH_z - margin10) || z > (stable_RH_z + margin10))
				{
				}
				else //If z is not changing
				{
					//Roll
					newX_angle_RH = initial_x_angle_RH;
				//	newZ_angle_RH = stable_RH_z;
					//turn to rigth
						if(x>stable_RH_x)
						{
							a=-0.99941557f;
							b=91.94384901f;

							newY_angle_RH = b + x*a;
				
							if (newY_angle_RH <30f)
								newY_angle_RH = 30f;
				
							if(newY_angle_RH > 90f)
								newY_angle_RH = 90f;
						}				
				
				}*/
				
			
	//	}
    }

 private void Peripheral_StreamReceived_L(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		
		if (e.StreamType == BoardStreamType.CalibratedMagnetism)
		{
			var args = e as BoardFloatVectorEventArgs;
			//List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received L CalibratedMagnetism state: " + args.Value);
			
		}
    }
 
	public async void Stop() {
        Debug.Log("Stopping");

		Central.StopScan();
		
        if (Peripheral != null) {
			 Debug.Log("Stop R Peripheral");
            await Peripheral?.EmulationState.WriteAsync(false);
			Peripheral.StreamReceived -= Peripheral_StreamReceived;
            Peripheral.PropertyChanged -= Peripheral_PropertyChanged;
			await Peripheral.StopAsync();
            Peripheral.Dispose();
            Peripheral = null;
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

    // Update is called once per frame
    void Update() {
			
		Hand_R.transform.localEulerAngles = new Vector3(newX_angle_RH, stable_coor_y , newZ_angle_RH);

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
