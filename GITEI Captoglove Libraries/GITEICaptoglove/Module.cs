using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

//SDK Captoglove
using GSdkNet;
using GSdkNet.BLE;
using GSdkNet.Board;
using GSdkNet.Peripheral;
using GSdkNet.BLE.Winapi;

namespace GITEICaptoglove
{
    /* 
        Class: Module
        Handles Captoglove module. Used by MyHand and MyArm.
    	
        Author: 
		Laura Moreno - laamorenoro@unal.edu.co 
		
		Copyrigth:		
		Copyrigth 2020 Universidad Nacional de Colombia, all rigths reserved. 		    
    */
    public class Module
    {
        /* 
            Group: Types
            Enum: ModuleType
            List of possible ways to use Captoglove module:

            TYPE_RIGHT_HAND - As right hand sensor
            TYPE_LEFT_HAND - As left hand sensor
            TYPE_RIGHT_ARM - As right forearm sensor
            TYPE_LEFT_ARM - As left forearm sensor
        */
        protected enum ModuleType
        {
            TYPE_RIGHT_HAND,
            TYPE_LEFT_HAND,
            TYPE_RIGHT_ARM,
            TYPE_LEFT_ARM
        }

        /* 
            Enum: ModuleAxis
            List of axes:

            AXIS_X - X axis 
            AXIS_Y - Y axis
            AXIS_Z - Z axis
        */
        public enum ModuleAxis
        {
            AXIS_X,
            AXIS_Y,
            AXIS_Z
        }

        private ModuleType eModuleType  = ModuleType.TYPE_RIGHT_HAND;
        //Have Setters and Getters
        private int nModuleID           = 0;
        private string sModuleName      = " ";
        private bool bModuleInitialized = false;
        private bool bModuleStarted     = false;
        private bool bPropertiesRead    = false;
        private bool bLogEnabled        = false;

        private IPeripheralCentral IModuleCentral;
        private IBoardPeripheral IModuleBoard;

        //Max and Min sensor conductivity
        protected float[] faFingerSensorMaxValue;
        protected float[] faFingerSensorMinValue;

        protected BoardStreamEventArgs sEventTaredQuart;
        protected BoardStreamEventArgs sEventSensorState;

        /* 
            Function: InitModule
            Initializes variables for Captoglove module configuration.

            Parameters:
            nID - Captoglove ID (4 digits number)
            eType - Captoglove use mode

            Example:
            --- Code
            InitModule(2496, Module.ModuleType.TYPE_RIGHT_HAND);
            ---
        */
        protected void InitModule(int nID, ModuleType eType)
        {
            SetModuleID(nID);
            SetModuleType(eType);
            SetModuleInitialized(false);
            SetModuleStarted(false);
            SetPropertiesRead(false);
            SetLogEnabled(false);

            //Initialize variables
            faFingerSensorMaxValue = new float[10];
            faFingerSensorMinValue = new float[10];

            for (int i = 0; i < 10; i++)
            {
                faFingerSensorMaxValue[i] = 0f;
                faFingerSensorMinValue[i] = 0f;
            }

            SetModuleInitialized(true);
        }

        /* 
             Function: SetModuleType
             Saves Captoglove module use mode.

             Parameters:
             enType - Captoglove module use mode

             Example:
             --- Code
             SetModuleType(Module.ModuleType.TYPE_RIGHT_HAND);
             ---
         */
        private void SetModuleType(ModuleType eType)
        {
            eModuleType = eType;
        }

        /* 
            Function: GetModuleType
            Returns:
            Captoglove module use mode
        */
        protected ModuleType GetModuleType()
        {
            return eModuleType;
        }

        /* 
            Function: SetModuleID
            Saves Captoglove module ID.

            Parameters:
            nID - Captoglove module ID (4 digits number)

            Example:
            --- Code
            SetModuleID(2496);
            ---
        */
        private void SetModuleID(int nID)
        {
            nModuleID = nID;
            SetModuleName();
        }

        /* 
            Function: GetModuleID
            Returns:
            Captoglove module ID
        */
        protected int GetModuleID()
        {
            return nModuleID;
        }

        /* 
            Function: SetModuleName
            Creates Captoglove module name to search BLE peripheral.

            Notes:
            SetModuleID() must be called first
        */
        private void SetModuleName()
        {
            sModuleName = "CaptoGlove" + nModuleID.ToString();
        }

        /* 
            Function: GetModuleName
            Returns:
            Captoglove module name
        */
        protected string GetModuleName()
        {
            return sModuleName;
        }

        /* 
            Function: SetModuleInitialized
            Saves whether Captoglove module variables are initialized or not.

            Parameters:
            b - true or false

            Example:
            --- Code
            SetModuleInitialized(true);
            ---

            Notes: 
            Normally used after InitModule() function is completed.
        */
        private void SetModuleInitialized(bool b)
        {
            bModuleInitialized = b;
        }

        /* 
            Function: GetModuleInitialized
            Returns:
            true - Captoglove module variables initialized
            false - Captoglove module variables NOT initialized
        */
        protected bool GetModuleInitialized()
        {
            return bModuleInitialized;
        }

        /* 
            Function: SetModuleStarted
            Saves whether Captoglove module is started or not.

            Parameters:
            b - true or false

            Example:
            --- Code
            SetModuleStarted(true);
            ---

            Notes: 
            Normally used after Start() function is completed.
        */
        private void SetModuleStarted(bool b)
        {
            bModuleStarted = b;
        }

        /* 
            Function: GetModuleStarted
            Returns:
            true - Captoglove module started
            false - Captoglove module NOT started     
        */
        protected bool GetModuleStarted()
        {
            return bModuleStarted;
        }

        /* 
            Function: SetPropertiesRead
            Saves whether Captoglove module properties have been read or not.

            Parameters:
            b - true or false

            Example:
            --- Code
            SetPropertiesRead(true);
            ---

            Notes: 
            Normally used after ReadProperties() function is completed.
        */
        private void SetPropertiesRead(bool b)
        {
            bPropertiesRead = b;
        }

        /* 
            Function: GetPropertiesRead
            Returns:
            true - Captoglove module properties have been read
            false - Captoglove module properties have NOT been read         
        */
        public bool GetPropertiesRead()
        {
            return bPropertiesRead;
        }

        /* 
            Function: SetLogEnabled
            Enables or disables log printing during app execution.

            Notes:
            Log can be added with TraceLog() function.
        */
        public void SetLogEnabled(bool b)
        {
            bLogEnabled = b;
        }

        /* 
            Function: GetLogEnabled
            Returns:
                true - Log printing is enabled 
                false - Log printing is disabled
        */
        private bool GetLogEnabled()
        {
            return bLogEnabled;
        }

        /*
            Function: Start
            Starts looking for Captoglove module peripheral.

            Returns:
            0  - Success
            -1 - Error: Module not initialized

            Notes: 
            Call this function from Start() function of Unity script.
        */
        public int Start()
        {
            if (GetModuleInitialized())
            {
                TraceLog("Start, looking for peripheral");

                var adapterScanner = new AdapterScanner();
                var adapter = adapterScanner.FindAdapter();
                var configurator = new Configurator(adapter);

                IModuleCentral = configurator.GetBoardCentral();
                IModuleCentral.PeripheralsChanged += Central_PeripheralsChanged; //If peripheral is detected 
                IModuleCentral.StartScan(new Dictionary<PeripheralScanFlag, object> {
                    { PeripheralScanFlag.ScanType, BleScanType.Balanced }
                });

                SetModuleStarted(true);
            }
            else
            {
                TraceLog("Module not initialized correctly");
                return -1;
            }

            return 0;
        }

        /* 
            Function: Central_PeripheralsChanged
            Looks for Captoglove name among the modules connected to bluetooth.

            Parameres: 
            sender - Unused parameter
            e - String received from Captoglove module  
         */
        private async void Central_PeripheralsChanged(object sender, PeripheralsEventArgs e)
        {
            foreach (var peripheral in e.Inserted)
            {
                // Enumerate peripherals and run first connected
                try
                {
                    var board = peripheral as IBoardPeripheral;
                    TraceLog("Trying to connect peripheral");
                    TraceLog("ID: " + board.Id);
                    TraceLog("Name: " + board.Name);

                    //If module ID is found
                    if (board.Name == sModuleName)
                    {
                        IModuleBoard = board;
                        IModuleBoard.PropertyChanged += Peripheral_PropertyChanged; //Set configurations
                        IModuleBoard.StreamReceived += Peripheral_StreamReceived;   //Read stream
                        await IModuleBoard.StartAsync();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    TraceLog("Unable to start board " + ex.Message);
                }
            }

        }

        /* 
            Function: Peripheral_PropertyChanged
            Detects if Captoglove module is connected to Unity app.

            Parameres: 
            sender - Unused parameter
            e - String received from Captoglove module  
        */
        private void Peripheral_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PeripheralProperty.Status)
            {
                TraceLog("Board status: " + IModuleBoard.Status.ToString());

                //If module is connected
                if (IModuleBoard.Status == PeripheralStatus.Connected)
                {
                    SetProperties();
                    ReadProperties();
                }
            }
        }

        /* 
            Function: Peripheral_StreamReceived
            Captures continuously stream events from Captoglove module connected.

            Parameres: 
            sender - Unused parameter
            e - Stream of events from Captoglove module  
        */
        private void Peripheral_StreamReceived(object sender, BoardStreamEventArgs e)
        {
            //Quaternion rotation of module
            if (e.StreamType == BoardStreamType.TaredQuaternion)
            {
                sEventTaredQuart = e;
            }

            //Sensors conductivity
            if (e.StreamType == BoardStreamType.SensorsState)
            {
                sEventSensorState = e;
            }           
        }

        /* 
            Function: SetProperties
            Set Captoglove module properties by default: Calibrate module, Tare module, Set time slots and Commit changes.

            Notes: 
            This function overwrites previous module configuration. 
        */
        private void SetProperties()
        {
            StreamTimeslots st = new StreamTimeslots();

            TraceLog("1. Calibrate module");
            IModuleBoard.CalibrateGyroAsync();

            TraceLog("2. Tare module");
            IModuleBoard.TareAsync();

            TraceLog("3. Set timeslots");
            st.Set(6, BoardStreamType.TaredQuaternion);     //Capture quaternion rotation

            if (eModuleType == ModuleType.TYPE_LEFT_HAND || eModuleType == ModuleType.TYPE_RIGHT_HAND)
            {
                st.Set(6, BoardStreamType.SensorsState);    //Capture sensors conductivity
            }
            //Write timeslots
            IModuleBoard.StreamTimeslots.WriteAsync(st);

            // Without commit previous configuration is temporal
            TraceLog("4. Commit changes");
            IModuleBoard.CommitChangesAsync();            
        }

        /* 
            Function: ReadProperties
            Read Captoglove module properties: Firmware version, Emulation mode, Time slots and Sensors calibration.       
        */
        private async void ReadProperties()
        {
            SensorDescriptor sensorDescriptor = new SensorDescriptor();
            EmulationModes emulationModes = new EmulationModes();
            StreamTimeslots streamTimeSlots = new StreamTimeslots();

            TraceLog("1. Read firmware version");
            TraceLog("" + IModuleBoard.FirmwareVersion);

            TraceLog("2. Read emulation Mode");
            await IModuleBoard.EmulationModes.ReadAsync();
            emulationModes = IModuleBoard.EmulationModes.Value;
            TraceLog(emulationModes.ToString());

            TraceLog("3. Read timeslots");
            await IModuleBoard.StreamTimeslots.ReadAsync();
            streamTimeSlots = IModuleBoard.StreamTimeslots.Value;
            TraceLog(streamTimeSlots.ToString());

            if (eModuleType == ModuleType.TYPE_LEFT_HAND || eModuleType == ModuleType.TYPE_RIGHT_HAND)
            {
                TraceLog("4. Read sensor calibration");

                for (int i = 0; i < 10; i++)
                {
                    await IModuleBoard.SensorDescriptors[i].ReadAsync();
                    sensorDescriptor = IModuleBoard.SensorDescriptors[i].Value;
                    TraceLog("Sensor [" + i + "]" + sensorDescriptor.ToString());
                    faFingerSensorMinValue[i] = sensorDescriptor.MinValue; //Min sensor conductivity
                    faFingerSensorMaxValue[i] = sensorDescriptor.MaxValue; //Max sensor conductivity
                }
            }

            SetPropertiesRead(true);
        }

        /* 
            Function: Stop
            Stops communication with Captoglove module.

            Notes: 
            Call this function from OnDestroy() function of Unity script.
        */
        public async void Stop()
        {
            TraceLog("Stopping");

            if (IModuleBoard != null)
            {
                TraceLog("Stop Peripheral");
                IModuleBoard.StreamReceived -= Peripheral_StreamReceived;
                IModuleBoard.PropertyChanged -= Peripheral_PropertyChanged;
                await IModuleBoard.StopAsync();
                IModuleBoard.Dispose();
                IModuleBoard = null;
            }

            if (IModuleCentral != null)
            {
                TraceLog("Stop Central");
                IModuleCentral.StopScan();
                IModuleCentral.PeripheralsChanged -= Central_PeripheralsChanged;
                IModuleCentral = null;
            }

            TraceLog("Stopped");
        }

        /* 
            Function: TraceLog
            Prints log lines during app execution.

            Example: 
            --- Code
            TraceLog("This is a log message in my app"); 
            ---

            Notes:  
            Log is printed only if SetLogEnabled(true) function is called first.
        */
        protected void TraceLog(string s)
        {
            if (GetLogEnabled())
                Debug.Log(sModuleName + " >>> " + s);
        }

    }
}

