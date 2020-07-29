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

    private IPeripheralCentral Central_L;
    private IBoardPeripheral Peripheral_L;
	
	public Transform[] Fingers;
	public Vector3[] temp;
	public float[] newXPox;
	public float[] stableXPos;
	public float[] prevSensorVal;
	public float[] refValue;
	
    // Use this for initialization
    void Start() {
        Debug.Log("Start");
        Debug.Log("Looking for peripheral");
		
		// Left Hand 
		refValue[0] = 3364.699f;		//pL1
		refValue[1] = refValue[0];	//pL2
		refValue[2] = refValue[0];	//pL3
		
		refValue[3] = 5547.451f;		//rL1
		refValue[4] = refValue[3]; 	//rL2
		refValue[5] = refValue[3]; 	//rL3
		
		refValue[6] = 5928.343f; 	//mL1
		refValue[7] = refValue[6]; 	//mL2
		refValue[8] = refValue[6]; 	//mL3
		
		refValue[9] = 3377.868f;		//iL1
		refValue[10] = refValue[9];	//iL2
		refValue[11] = refValue[9];	//iL2
		
		refValue[12] = 2672.114f;	//tL1
		refValue[13] = refValue[12];//tL2
		refValue[14] = refValue[12];//tL3

		for(int i =0 ; i<14; i++)
		{
			temp[i] = Fingers[i].localEulerAngles;		
			newXPox[i] = 0;
			stableXPos[i] = temp[i].x;
			prevSensorVal[i] = refValue[i];
		}
		
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
		
				//Right Hand 
				if(board.Name == "CaptoGlove2480"){
                Peripheral = board;
                Peripheral.StreamReceived += Peripheral_StreamReceived;
                Peripheral.PropertyChanged += Peripheral_PropertyChanged;
                await board.StartAsync();
				} //Left Hand
				else if (board.Name == "CaptoGlove2464"){
				Peripheral_L = board;
                Peripheral_L.StreamReceived += Peripheral_StreamReceived_left;
                Peripheral_L.PropertyChanged += Peripheral_PropertyChanged_left;
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

private async void Peripheral_PropertyChanged_left(object sender, PropertyChangedEventArgs e) {
		
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
				
				//ConfigSensors();
				
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

			float pinkyLVal = float.Parse(oneValue[1]);	
			float ringLVal  = float.Parse(oneValue[3]);		
			float middLVal  = float.Parse(oneValue[5]);
			float idxLVal   = float.Parse(oneValue[7]);
			float thumbLVal = float.Parse(oneValue[9]);
				
			Debug.Log("------------------------------------------------------");
			Debug.Log("L p "+pinkyLVal); //3
			Debug.Log("L r "+ringLVal); //2
			Debug.Log("L m "+middLVal); //1
			Debug.Log("L i "+idxLVal); //0			
			Debug.Log("L t "+thumbLVal); //4
			Debug.Log("------------------------------------------------------");
			
			float a = 0;
			float b = 0;
			float marg = 20f;
			float ref3 =1260.854f;
			
			if (pinkyLVal < (prevSensorVal[0] - marg) || pinkyLVal > (prevSensorVal[0] + marg))
			{
				a=-0.024199199f;
				b= 81.42302231f;
				
				// 2
				newXPox[0] = b + pinkyLVal*a;
				
				if (newXPox[0] < 0)
					newXPox[0] = 0;
				
				if(newXPox[0] > 80)
					newXPox[0] = 80;
				
				prevSensorVal[0] = pinkyLVal;
				
				newXPox[1] = newXPox[0];
				
				if(pinkyLVal<ref3)
					newXPox[2] = newXPox[0];
				else
					newXPox[2] = stableXPos[2];
				
			}
			
			if (ringLVal < (prevSensorVal[3] - marg) || ringLVal > (prevSensorVal[3] + marg))
			{
				a=-0.015849954f;
				b= 87.92684422f;
				
				newXPox[3] = b + ringLVal*a;
				
				if (newXPox[3] < 0)
					newXPox[3] = 0;
				
				if(newXPox[3] > 80)
					newXPox[3] = 80;
				
				prevSensorVal[3] = ringLVal;	

				newXPox[4] = newXPox[3];

				if(ringLVal<ref3)
					newXPox[5] = newXPox[3];
				else
					newXPox[5] = stableXPos[5];				
			}
			
			if (middLVal < (prevSensorVal[6] - marg) || middLVal > (prevSensorVal[6] + marg))
			{
				a=-0.013786264f;
				b= 81.72970189f;
				
				newXPox[6] = b + middLVal*a;
				
				if (newXPox[6]<0)
					newXPox[6] = 0;
				
				if(newXPox[6] > 80)
					newXPox[6] = 80;
				
				prevSensorVal[6] = middLVal;	

				newXPox[7] = newXPox[6];				
				
				if(middLVal<ref3)
					newXPox[8] = newXPox[6];
				else
					newXPox[8] = stableXPos[8];
			}
			
			if (idxLVal < (prevSensorVal[9] - marg) || idxLVal > (prevSensorVal[9] + marg))
			{
				a=-0.024304017f;
				b= 82.09576289f;
				
				newXPox[9] = b + idxLVal*a;
				
				if (newXPox[9]<0)
					newXPox[9] = 0;
				
				if(newXPox[9] > 80)
					newXPox[9] = 80;
								
				prevSensorVal[9] = idxLVal;
				
				newXPox[10] = newXPox[9];
				
				if(idxLVal<ref3)
					newXPox[11] = newXPox[9];
				else
					newXPox[11] = stableXPos[11];
			}
			
			if (thumbLVal < (prevSensorVal[12] - marg) || thumbLVal > (prevSensorVal[12] + marg))
			{
				// 1
				a=-0.031239896f;
				b=73.47656305f;
				
				newXPox[12] = b + thumbLVal*a;
				
				if(newXPox[12] < -10)
					newXPox[12] = -10;
				 
				if(newXPox[12] > 30)
					newXPox[12] = 30;
				
				prevSensorVal[12] = thumbLVal;

				// 2
				a=-0.03904987f;
				b=104.3457038f;
				
				newXPox[13] = b + thumbLVal*a;
				
				if(newXPox[13] < -0)
					newXPox[13] = 0;
				 
				if(newXPox[13] > 50)
					newXPox[13] = 50;
				
				prevSensorVal[13] = thumbLVal;
				
				if(thumbLVal<3444.49f)
				{
					// 3
					a=-0.023429922f;
					b=62.60742229f;
				
					newXPox[14] = b + thumbLVal*a;
				
					if(newXPox[14] < -0)
						newXPox[14] = 0;
				 
					if(newXPox[14] > 30)
						newXPox[14] = 30;
				
					prevSensorVal[14] = thumbLVal;
				}
				else
					newXPox[14] = stableXPos[14];				
			}
		
			
        } else if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
            Debug.Log("Received tared quaternion: " + args.Value);
			
        } else if (e.StreamType == BoardStreamType.LinearAcceleration) {
            var args = e as BoardFloatVectorEventArgs;
            Debug.Log("Received linear acceleration: " + args.Value);
        }
		else if (e.StreamType == BoardStreamType.CalibratedMagnetism)
		{
			//newXPox = (newXPox+1);
			
		}
    }

 private void Peripheral_StreamReceived_left(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		
        if (e.StreamType == BoardStreamType.SensorsState) {
            var args = e as BoardFloatSequenceEventArgs;
            var value = FloatsToString(args.Value);
			List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received sensors state: " + value);
			
			//oneValue.Reverse(); // For right hand 

			float pinkyLVal = float.Parse(oneValue[1]);	
			float ringLVal  = float.Parse(oneValue[3]);		
			float middLVal  = float.Parse(oneValue[5]);
			float idxLVal   = float.Parse(oneValue[7]);
			float thumbLVal = float.Parse(oneValue[9]);
				
			Debug.Log("------------------------------------------------------");
			Debug.Log("L p "+pinkyLVal); //3
			Debug.Log("L r "+ringLVal); //2
			Debug.Log("L m "+middLVal); //1
			Debug.Log("L i "+idxLVal); //0			
			Debug.Log("L t "+thumbLVal); //4
			Debug.Log("------------------------------------------------------");
		/*	
			float a = 0;
			float b = 0;
			float marg = 20f;
			float ref3 =1260.854f;
			
			if (pinkyLVal < (prevSensorVal[0] - marg) || pinkyLVal > (prevSensorVal[0] + marg))
			{
				a=-0.024199199f;
				b= 81.42302231f;
				
				// 2
				newXPox[0] = b + pinkyLVal*a;
				
				if (newXPox[0] < 0)
					newXPox[0] = 0;
				
				if(newXPox[0] > 80)
					newXPox[0] = 80;
				
				prevSensorVal[0] = pinkyLVal;
				
				newXPox[1] = newXPox[0];
				
				if(pinkyLVal<ref3)
					newXPox[2] = newXPox[0];
				else
					newXPox[2] = stableXPos[2];
				
			}
			
			if (ringLVal < (prevSensorVal[3] - marg) || ringLVal > (prevSensorVal[3] + marg))
			{
				a=-0.015849954f;
				b= 87.92684422f;
				
				newXPox[3] = b + ringLVal*a;
				
				if (newXPox[3] < 0)
					newXPox[3] = 0;
				
				if(newXPox[3] > 80)
					newXPox[3] = 80;
				
				prevSensorVal[3] = ringLVal;	

				newXPox[4] = newXPox[3];

				if(ringLVal<ref3)
					newXPox[5] = newXPox[3];
				else
					newXPox[5] = stableXPos[5];				
			}
			
			if (middLVal < (prevSensorVal[6] - marg) || middLVal > (prevSensorVal[6] + marg))
			{
				a=-0.013786264f;
				b= 81.72970189f;
				
				newXPox[6] = b + middLVal*a;
				
				if (newXPox[6]<0)
					newXPox[6] = 0;
				
				if(newXPox[6] > 80)
					newXPox[6] = 80;
				
				prevSensorVal[6] = middLVal;	

				newXPox[7] = newXPox[6];				
				
				if(middLVal<ref3)
					newXPox[8] = newXPox[6];
				else
					newXPox[8] = stableXPos[8];
			}
			
			if (idxLVal < (prevSensorVal[9] - marg) || idxLVal > (prevSensorVal[9] + marg))
			{
				a=-0.024304017f;
				b= 82.09576289f;
				
				newXPox[9] = b + idxLVal*a;
				
				if (newXPox[9]<0)
					newXPox[9] = 0;
				
				if(newXPox[9] > 80)
					newXPox[9] = 80;
								
				prevSensorVal[9] = idxLVal;
				
				newXPox[10] = newXPox[9];
				
				if(idxLVal<ref3)
					newXPox[11] = newXPox[9];
				else
					newXPox[11] = stableXPos[11];
			}
			
			if (thumbLVal < (prevSensorVal[12] - marg) || thumbLVal > (prevSensorVal[12] + marg))
			{
				// 1
				a=-0.031239896f;
				b=73.47656305f;
				
				newXPox[12] = b + thumbLVal*a;
				
				if(newXPox[12] < -10)
					newXPox[12] = -10;
				 
				if(newXPox[12] > 30)
					newXPox[12] = 30;
				
				prevSensorVal[12] = thumbLVal;

				// 2
				a=-0.03904987f;
				b=104.3457038f;
				
				newXPox[13] = b + thumbLVal*a;
				
				if(newXPox[13] < -0)
					newXPox[13] = 0;
				 
				if(newXPox[13] > 50)
					newXPox[13] = 50;
				
				prevSensorVal[13] = thumbLVal;
				
				if(thumbLVal<3444.49f)
				{
					// 3
					a=-0.023429922f;
					b=62.60742229f;
				
					newXPox[14] = b + thumbLVal*a;
				
					if(newXPox[14] < -0)
						newXPox[14] = 0;
				 
					if(newXPox[14] > 30)
						newXPox[14] = 30;
				
					prevSensorVal[14] = thumbLVal;
				}
				else
					newXPox[14] = stableXPos[14];				
			}
		*/
			
        } else if (e.StreamType == BoardStreamType.TaredQuaternion) {
            var args = e as BoardQuaternionEventArgs;
            Debug.Log("Received tared quaternion: " + args.Value);
			
        } else if (e.StreamType == BoardStreamType.LinearAcceleration) {
            var args = e as BoardFloatVectorEventArgs;
            Debug.Log("Received linear acceleration: " + args.Value);
        }
		else if (e.StreamType == BoardStreamType.CalibratedMagnetism)
		{
			//newXPox = (newXPox+1);
			
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
			 Debug.Log("Stop Peripheral");
            await Peripheral?.EmulationState.WriteAsync(false);
			Peripheral.StreamReceived -= Peripheral_StreamReceived;
            Peripheral.PropertyChanged -= Peripheral_PropertyChanged;
			await Peripheral.StopAsync();
            Peripheral.Dispose();
            Peripheral = null;
        }
		
		if (Peripheral_L != null) {
			 Debug.Log("Stop Peripheral_L");
            await Peripheral_L?.EmulationState.WriteAsync(false);
			Peripheral_L.StreamReceived -= Peripheral_StreamReceived;
            Peripheral_L.PropertyChanged -= Peripheral_PropertyChanged;
			await Peripheral_L.StopAsync();
            Peripheral_L.Dispose();
            Peripheral_L = null;
        }

        if (Central != null) {
			 Debug.Log("Stop Central");
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
		
		for(int i = 0 ; i<14 ; i++)
		{	
				Fingers[i].transform.localEulerAngles = new Vector3(newXPox[i], temp[i].y, temp[i].z);
			
		}
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
