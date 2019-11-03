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

public class GloveScript : MonoBehaviour, ILoggerProvider {
    private IPeripheralCentral Central;
    private IBoardPeripheral Peripheral;
    private IBoardPeripheral Peripheral_L;
	
	public Transform[] Fingers;
	public Transform[] Fingers_L;
	public Transform   Hand_R;
	
	public Vector3[] temp;
	public Vector3[] temp_L;
	public Vector3   temp_RH;
	
	public float[] newXPox;
	public float[] newXPox_L;
	public float   newXPox_RH;
	public float   newYPox_RH;
	
	public float[] stableXPos;
	public float[] stableXPos_L;
	public float   stableXPos_RH;
	
	public float[] prevSensorVal;
	public float[] prevSensorVal_L;
	public float   prevensorVal_RH_x;
	public float   prevensorVal_RH_y;
	
	public float Handx;
	public float Handy;
	public float Handz;
	
	public float pinkyLVal;	
	public float ringLVal;	
	public float middLVal;
	public float idxLVal;
	public float thumbLVal;
			
	public float[] refValue;
	public float[] refValue_L;
	public float   refValue_RH_x;
	public float   refValue_RH_y;
	
	private const string SAVE_SEPARATOR = ",";
    // Use this for initialization
    void Start() {
		
	
        Debug.Log("Start");
    
	    //Get the path of the Game data folder
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("Path : " + m_Path);
		
		Debug.Log("Looking for peripheral");
		
		// Right Hand 
		refValue[0] = 3364.699f;	//pR1
		refValue[1] = refValue[0];	//pR2
		refValue[2] = refValue[0];	//pR3
		
		refValue[3] = 5547.451f;	//rR1
		refValue[4] = refValue[3]; 	//rR2
		refValue[5] = refValue[3]; 	//rR3
		
		refValue[6] = 5928.343f; 	//mR1
		refValue[7] = refValue[6]; 	//mR2
		refValue[8] = refValue[6]; 	//mR3
		
		refValue[9] = 3377.868f;	//iR1
		refValue[10] = refValue[9];	//iR2
		refValue[11] = refValue[9];	//iR2
		
		refValue[12] = 2672.114f;	//tR1
		refValue[13] = refValue[12];//tR2
		refValue[14] = refValue[12];//tR3
		
		// Left Hand 
		refValue_L[0] = 5974.839f;		//pL1
		refValue[1] = refValue_L[0];	//pL2
		refValue_L[2] = refValue_L[0];	//pL3
		
		refValue_L[3] = 8315184f;		//rL1
		refValue_L[4] = refValue_L[3]; 	//rL2
		refValue_L[5] = refValue_L[3]; 	//rL3
		
		refValue_L[6] = 3063.593f; 		//mL1
		refValue_L[7] = refValue_L[6]; 	//mL2
		refValue_L[8] = refValue_L[6]; 	//mL3
		
		refValue_L[9] = 4653.209f;		//iL1
		refValue_L[10] = refValue_L[9];	//iL2
		refValue_L[11] = refValue_L[9];	//iL2
		
		refValue_L[12] = 3567.74f;		//tL1
		refValue_L[13] = refValue_L[12];//tL2
		refValue_L[14] = refValue_L[12];//tL3

		refValue_RH_x	=  -30f;
		refValue_RH_y = -60f;
		
		for(int i =0 ; i<14; i++)
		{
			temp[i] = Fingers[i].localEulerAngles;	
			temp_L[i] = Fingers_L[i].localEulerAngles;		
			temp_RH = Hand_R.localEulerAngles;
			
			newXPox[i] = 0;
			newXPox_L[i] = 0;
			newXPox_RH = 0;
			newYPox_RH = 0;
			
			stableXPos[i] = temp[i].x;
			stableXPos_L[i] = temp_L[i].x;
			stableXPos_RH = temp_RH.x;
			
			prevSensorVal[i] = refValue[i];
			prevSensorVal_L[i] = refValue_L[i];
			prevensorVal_RH_x = refValue_RH_x;
			prevensorVal_RH_y = refValue_RH_y;
		}
		
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
				
				await Peripheral.BatteryLevel.ReadAsync();
				await Peripheral.Temperature.ReadAsync();			
				await Peripheral.StreamTimeslots.ReadAsync();
				await Peripheral.EmulationModes.ReadAsync();
				await Peripheral.EmulationState.ReadAsync();
				await Peripheral.UserInfo.ReadAsync();
				
				ConfigSensors();
				
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
				
				ConfigSensors();
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

			pinkyLVal = float.Parse(oneValue[1]);	
			ringLVal  = float.Parse(oneValue[3]);		
			middLVal  = float.Parse(oneValue[5]);
			idxLVal   = float.Parse(oneValue[7]);
			thumbLVal = float.Parse(oneValue[9]);
/*				
			Debug.Log("------------------------------------------------------");
			Debug.Log("R p "+pinkyLVal); 
			Debug.Log("R r "+ringLVal); 
			Debug.Log("R m "+middLVal); 
			Debug.Log("R i "+idxLVal); 			
			Debug.Log("R t "+thumbLVal); 
			Debug.Log("------------------------------------------------------");
	*/		
			float a = 0;
			float b = 0;
			float marg = 30f;
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
		//RIGHT HAND
		else if (e.StreamType == BoardStreamType.CalibratedMagnetism)
		{
		//	if (skip==false)
			//{
				//var args = e as BoardFloatSequenceEventArgs;
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
				//Debug.Log("Received R CalibratedMagnetism state: " + args.Value);
		/*		Debug.Log("Received R Magnetism x: " + x);
				Debug.Log("Received R Magnetism Y: " + y);
				Debug.Log("Received R Magnetism z: " + z);*/
			
				float a = 0;
				float b = 0;
				float marg = 10f;
				//float ref3 =1260.854f; //To move third 
			
				if (x < (prevensorVal_RH_x - marg) || x > (prevensorVal_RH_x + marg))
				{
					//a=-2.413025512f;
					//b=53.46733668f;
					a=1.416666667f;
					b=-247.5f;

					newXPox_RH = b + x*a;
				
					if (newXPox_RH < -290f)
						newXPox_RH = -290f;
				
					if(newXPox_RH > -120f)
						newXPox_RH = -120f;
				
					prevensorVal_RH_x = x;
				}
				if (y < (prevensorVal_RH_y - marg) || y > (prevensorVal_RH_y + marg))
				{
					//a=-2.413025512f;
					//b=53.46733668f;
					a=-1.6f;
					b=-64f;

					newYPox_RH = b + y*a;
				
					if (newYPox_RH <-80f)
						newYPox_RH = -80f;
				
					if(newYPox_RH > 80f)
						newYPox_RH = 80f;
				
					prevensorVal_RH_y = y;
				}
			//}
			
		}
    }

 private void Peripheral_StreamReceived_L(object sender, BoardStreamEventArgs e) {
		
		Debug.Log("- Stream Received : " + e.StreamType.ToString());
		
        if (e.StreamType == BoardStreamType.SensorsState) {
            var args = e as BoardFloatSequenceEventArgs;
            var value = FloatsToString(args.Value);
			List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received sensors state: " + value);

			float pinkyLVal = float.Parse(oneValue[1]);	
			float ringLVal  = float.Parse(oneValue[3]);		
			float middLVal  = float.Parse(oneValue[5]);
			float idxLVal   = float.Parse(oneValue[7]);
			float thumbLVal = float.Parse(oneValue[9]);
				
			Debug.Log("------------------------------------------------------");
			Debug.Log("L p "+pinkyLVal); 
			Debug.Log("L r "+ringLVal); 
			Debug.Log("L m "+middLVal);
			Debug.Log("L i "+idxLVal); 			
			Debug.Log("L t "+thumbLVal); 
			Debug.Log("------------------------------------------------------");
			
			float a = 0;
			float b = 0;
			float marg = 20f;
			float ref3 =1260.854f; //To move third 
			
			if (pinkyLVal < (prevSensorVal_L[0] - marg) || pinkyLVal > (prevSensorVal_L[0] + marg))
			{
				a=-0.024199199f;
				b= 81.42302231f;
				
				// 2
				newXPox_L[0] = b + pinkyLVal*a;
				
				if (newXPox_L[0] < 0)
					newXPox_L[0] = 0;
				
				if(newXPox_L[0] > 80)
					newXPox_L[0] = 80;
				
				prevSensorVal_L[0] = pinkyLVal;
				
				newXPox_L[1] = newXPox_L[0];
				
				if(pinkyLVal<ref3)
					newXPox_L[2] = newXPox_L[0];
				else
					newXPox_L[2] = stableXPos_L[2];
				
			}
			
			if (ringLVal < (prevSensorVal_L[3] - marg) || ringLVal > (prevSensorVal_L[3] + marg))
			{
				a=-0.015849954f;
				b= 87.92684422f;
				
				newXPox_L[3] = b + ringLVal*a;
				
				if (newXPox_L[3] < 0)
					newXPox_L[3] = 0;
				
				if(newXPox_L[3] > 80)
					newXPox_L[3] = 80;
				
				prevSensorVal_L[3] = ringLVal;	

				newXPox_L[4] = newXPox_L[3];

				if(ringLVal<ref3)
					newXPox_L[5] = newXPox_L[3];
				else
					newXPox_L[5] = stableXPos_L[5];				
			}
			
			if (middLVal < (prevSensorVal_L[6] - marg) || middLVal > (prevSensorVal_L[6] + marg))
			{
				a=-0.013786264f;
				b= 81.72970189f;
				
				newXPox_L[6] = b + middLVal*a;
				
				if (newXPox_L[6]<0)
					newXPox_L[6] = 0;
				
				if(newXPox_L[6] > 80)
					newXPox_L[6] = 80;
				
				prevSensorVal_L[6] = middLVal;	

				newXPox_L[7] = newXPox_L[6];				
				
				if(middLVal<ref3)
					newXPox_L[8] = newXPox_L[6];
				else
					newXPox_L[8] = stableXPos_L[8];
			}
			
			if (idxLVal < (prevSensorVal_L[9] - marg) || idxLVal > (prevSensorVal_L[9] + marg))
			{
				a=-0.024304017f;
				b= 82.09576289f;
				
				newXPox_L[9] = b + idxLVal*a;
				
				if (newXPox_L[9]<0)
					newXPox_L[9] = 0;
				
				if(newXPox_L[9] > 80)
					newXPox_L[9] = 80;
								
				prevSensorVal_L[9] = idxLVal;
				
				newXPox_L[10] = newXPox_L[9];
				
				if(idxLVal<ref3)
					newXPox_L[11] = newXPox_L[9];
				else
					newXPox_L[11] = stableXPos_L[11];
			}
			
			if (thumbLVal < (prevSensorVal_L[12] - marg) || thumbLVal > (prevSensorVal_L[12] + marg))
			{
				// 1
				a=-0.031239896f;
				b=73.47656305f;
				
				newXPox_L[12] = b + thumbLVal*a;
				
				if(newXPox_L[12] < -10)
					newXPox_L[12] = -10;
				 
				if(newXPox_L[12] > 30)
					newXPox_L[12] = 30;
				
				prevSensorVal_L[12] = thumbLVal;

				// 2
				a=-0.03904987f;
				b=104.3457038f;
				
				newXPox_L[13] = b + thumbLVal*a;
				
				if(newXPox_L[13] < -0)
					newXPox_L[13] = 0;
				 
				if(newXPox_L[13] > 50)
					newXPox_L[13] = 50;
				
				prevSensorVal_L[13] = thumbLVal;
				
				if(thumbLVal<3444.49f)
				{
					// 3
					a=-0.023429922f;
					b=62.60742229f;
				
					newXPox_L[14] = b + thumbLVal*a;
				
					if(newXPox_L[14] < -0)
						newXPox_L[14] = 0;
				 
					if(newXPox_L[14] > 30)
						newXPox_L[14] = 30;
				
					prevSensorVal_L[14] = thumbLVal;
				}
				else
					newXPox_L[14] = stableXPos_L[14];				
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
			var args = e as BoardFloatVectorEventArgs;
			//List<string> oneValue = value.Split(',').ToList<string>();
			
			Debug.Log("Received L CalibratedMagnetism state: " + args.Value);
			
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
    
		for (int i = 0; i < 10; i += 1) {
			//var descriptor = SensorDescriptor.Predefined(SensorConfiguration.Balanced);
            await Peripheral_L.SensorDescriptors[i].ReadAsync();
        }
        Debug.Log("Peripheral read balanced sensor descriptors");
		
		Debug.Log("- Init simulation");
	//	await Peripheral.EmulationState.WriteAsync(true);
				
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
		{	/*
				if(i>=12)
					Fingers[i].transform.localEulerAngles = new Vector3(newXPox[i], temp[i].y, temp[i].z);
				else
					Fingers[i].transform.localEulerAngles = new Vector3(-newXPox[i], temp[i].y, temp[i].z);

				Fingers_L[i].transform.localEulerAngles = new Vector3(newXPox_L[i], temp_L[i].y, temp_L[i].z);
				*/
				Hand_R.transform.localEulerAngles = new Vector3(newYPox_RH, newXPox_RH, temp_RH.z);
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
