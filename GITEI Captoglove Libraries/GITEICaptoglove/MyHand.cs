using System.IO;
using UnityEngine;

//SDK Captoglove
using GSdkNet.Board;

namespace GITEICaptoglove
{
    /* 
        Class: MyHand
        Handles Captoglove module configured as hand sensor.

    	Author: 
		Laura Moreno - laamorenoro@unal.edu.co 
		
		Copyrigth:		
		Copyrigth 2020 Universidad Nacional de Colombia, all rigths reserved. 
		
    */
    public class MyHand : Module
    {
        /* 
           Enum: HandType
           List of possible ways to use Captoglove module with this class:

           TYPE_RIGHT_HAND - As right hand sensor
           TYPE_LEFT_HAND - As left hand sensor       
       */
        public enum HandType
        {
            TYPE_RIGHT_HAND,
            TYPE_LEFT_HAND
        }

        private HandType eHandType;
        //hand axis
        private ModuleAxis ePitchAxis, eYawAxis, eRollAxis;
        
        private float fHandXangle, fHandYangle, fHandZangle;
        private int nThumbSensor, nIndexSensor, nMiddleSensor, nRingSensor, nPinkySensor, nPressureSensor;
        private int nOmittedDegrees; //To avoid finger shaking
        private bool bFingerEquationReady; 

        private float[] faSensorValue; //Sensor conductivity
        private float[] faSensorTrigger; //Trigger to detect gestures 

        //Constans of the movement equation (x= y*M + B)
        private float fPitchEqM, fPitchEqB, fYawEqM, fYawEqB, fRollEqM, fRollEqB;
        private float[] faFingerEqB;
        private float[] faFingerEqM;
        private float[] faFingerPh1EqM;
        private float[] faFingerPh1EqB;
        private float[] faFingerPh2EqM;
        private float[] faFingerPh2EqB;

        private Vector3[] vaFingerAngle;
        private Vector3[] vaPrevFingerAngle;
        private Vector3[] vaFingerPh1Angle;
        private Vector3[] vaFingerPh2Angle;

        private float[] faFingerMinRot;
        private float[] faFingerMaxRot;
        private float[] faFingerPh1MinRot;
        private float[] faFingerPh1MaxRot;
        private float[] faFingerPh2MinRot;
        private float[] faFingerPh2MaxRot;

        private Transform tHandObj = null;
        private Transform[] taFingerObj;
        //Dynamic size for finger phalanges
        private Transform[] taThumbPhOj;
        private Transform[] taIndexPhObj;
        private Transform[] taMiddlePhObj;
        private Transform[] taRingPhObj;
        private Transform[] taPinkyPhObj;

        //To save data in a file 
        private StreamWriter wHandWriter = null;
        private StreamWriter wFingerWriter = null;
        private bool bHandFile = false;
        private bool bFingerFile = false;

        /* 
            Constructor: MyHand
            Initializes variables for Captoglove module configuration.

            Parameters:
            nID - Captoglove ID (4 digits number)
            etype - Captoglove use mode

            Example:
            --- Code
            MyHand RightHand = new MyHand(2496, MyHand.HandType.TYPE_RIGHT_HAND);        
            ---
        */
        public MyHand(int nID, HandType eType)
        {
            SetHandType(eType);

            if (eType == HandType.TYPE_RIGHT_HAND)
            {
                InitModule(nID, Module.ModuleType.TYPE_RIGHT_HAND);
                SetFingerSensor(1, 3, 5, 7, 9, 2); //Default connection of the sensors in CaptoSensor
            }
            else
            {
                InitModule(nID, Module.ModuleType.TYPE_LEFT_HAND);
                SetFingerSensor(10, 8, 6, 3, 2, 9);
            }

            SetFingerEquationReady(false);

            //Initialize variables
            faSensorValue = new float[10];
            faSensorTrigger = new float[10];

            faFingerEqM = new float[10];
            faFingerEqB = new float[10];
            faFingerPh1EqM = new float[10];
            faFingerPh1EqB = new float[10];
            faFingerPh2EqM = new float[10];
            faFingerPh2EqB = new float[10];

            vaFingerAngle = new Vector3[10];
            vaPrevFingerAngle = new Vector3[10];
            vaFingerPh1Angle = new Vector3[10];
            vaFingerPh2Angle = new Vector3[10];

            faFingerMinRot = new float[10];
            faFingerMaxRot = new float[10];
            faFingerPh1MinRot = new float[10];
            faFingerPh1MaxRot = new float[10];
            faFingerPh2MinRot = new float[10];
            faFingerPh2MaxRot = new float[10];

            taFingerObj = new Transform[10];

            for (int i = 0; i < 10; i++)
            {
                faSensorValue[i] = 0f;
                faSensorTrigger[i] = 0f;

                faFingerEqM[i] = 0f;
                faFingerEqB[i] = 0f;
                faFingerPh1EqM[i] = 0f;
                faFingerPh1EqB[i] = 0f;
                faFingerPh2EqM[i] = 0f;
                faFingerPh2EqB[i] = 0f;

                vaFingerAngle[i] = new Vector3(0, 0, 0);
                vaPrevFingerAngle[i] = vaFingerAngle[i];
                vaFingerPh1Angle[i] = new Vector3(0, 0, 0);
                vaFingerPh2Angle[i] = new Vector3(0, 0, 0);

                taFingerObj[i] = null;
            }

            SetDefaultRotLimits();
        }

        /* 
             Function: SetHandType
             Saves Captoglove module use mode.

             Parameters:
             eType - Captoglove module use mode

             Example:
             --- Code
             SetHandType(MyHand.HandType.TYPE_RIGHT_HAND);
             ---
         */
        private void SetHandType(HandType eType)
        {
            eHandType = eType;
        }

        /* 
            Function: GetHandtype
            Returns:
            Captoglove module use mode
        */
        public HandType GetHandtype()
        {
            return eHandType;
        }

        /* 
            Function: SetFingerEquationReady
            Saves whether the algorithm for finger movement has been created or not.

            Parameters:
            b - true or false

            Example:
            --- Code
            SetFingerEquationReady(true);
            ---

            Notes: 
            Normally used after SetFingerEquation() function is completed.
        */
        private void SetFingerEquationReady(bool b)
        {
            bFingerEquationReady = b;
        }

        /* 
            Function: GetFingerEquationReady
            Returns:
            true - Finger algorithm has been created
            false - Finger algorithm has NOT been created
        */
        private bool GetFingerEquationReady()
        {
            return bFingerEquationReady;
        }

        /* 
            Function: SetOmittedDegrees
            Saves the number of degrees that must be omitted in the rotation of the fingers to avoid shaking.

            Parameters:
            nDegrees - Number of degrees that must be omitted in the rotation of the fingers

            Example:
            --- Code
            SetOmittedDegrees(2);
            ---

            Notes:
            Usually small values between 0 and 5 to keep fast response in simulation.
        */
        private void SetOmittedDegrees(int nDegrees)
        {
            nOmittedDegrees = nDegrees;
        }

        /* 
            Function: GetOmittedDegrees
            Returns:
            Number of degrees that are omitted in the movement of the fingers
        */
        public int GetOmittedDegrees()
        {
            return nOmittedDegrees;
        }

        /* 
            Function: SetHandObject
            Attaches Captoglove module movement to hand object.     

            Parameters:
            tHandObj - Hand object           

            Returns: 
            0 - Success
            -1 - Object error

            Example:
            --- Code
            SetHandObject(transRH);
            ---

            Notes: 
            Place the hand object horizontally in the scene before assigning it in this function to save default position.
        */
        public int SetHandObject(Transform tHandObj)
        {
            if (tHandObj == null)
            {
                TraceLog("Hand transform error");
                return -1;
            }

            this.tHandObj = tHandObj;
            //Default angles for 3D model provided with this library
            this.ePitchAxis = Module.ModuleAxis.AXIS_X;
            this.eYawAxis = Module.ModuleAxis.AXIS_Z;
            this.eRollAxis = Module.ModuleAxis.AXIS_Y;
            //Initial rotation 
            fHandXangle = this.tHandObj.localEulerAngles.x;
            fHandYangle = this.tHandObj.localEulerAngles.y;
            fHandZangle = this.tHandObj.localEulerAngles.z;

            return 0;
        }

        /* 
            Function: SetFingerObject
            Attaches Captoglove sensor movement to finger object. 

            Parameters:
            tThumbObj  - Thumb finger object
            tIndexObj  - Index finger object
            tMiddleObj - Middle finger object
            tRingObj   - Ring finger object
            tPinkyObj  - Pinky finger object        

            Returns: 
            0 - Success
            -1 - Finger object error
            -2 - Child object error

            Example:
            --- Code
            SetFingerObject(transThuR,transIndR,transMidR,transRinR, transPinR);
            ---

            Notes: 
            _This function expects each finger object to have at least 2 children to simulate phalanges movement._ 
            Place the finger object horizontally in the scene before assigning it in this function to save default position.    

    */
        public int SetFingerObject(Transform tThumbObj, Transform tIndexObj, Transform tMiddleObj,
                                       Transform tRingObj, Transform tPinkyObj)
        {
            int nChildCnt = 2; //two phalanges

            if (tThumbObj == null ||
                tIndexObj == null ||
                tMiddleObj == null ||
                tRingObj == null ||
                tPinkyObj == null)
            {
                TraceLog("Finger transform error");
                return -1;
            }
            //Get object children
            taThumbPhOj     = tThumbObj.GetComponentsInChildren<Transform>();
            taIndexPhObj    = tIndexObj.GetComponentsInChildren<Transform>();
            taMiddlePhObj   = tMiddleObj.GetComponentsInChildren<Transform>();
            taRingPhObj     = tRingObj.GetComponentsInChildren<Transform>();
            taPinkyPhObj    = tPinkyObj.GetComponentsInChildren<Transform>();

            if (taThumbPhOj.Length < nChildCnt ||
                taIndexPhObj.Length < nChildCnt ||
                taMiddlePhObj.Length < nChildCnt ||
                taRingPhObj.Length < nChildCnt ||
                taPinkyPhObj.Length < nChildCnt)
            {
                TraceLog("Child transform error");
                return -2;
            }
            //Save objects
            taFingerObj[nThumbSensor]   = tThumbObj;
            taFingerObj[nIndexSensor]   = tIndexObj;
            taFingerObj[nMiddleSensor]  = tMiddleObj;
            taFingerObj[nRingSensor]    = tRingObj;
            taFingerObj[nPinkySensor]   = tPinkyObj;

            //Save initial rotation of fingers
            for (int i = 0; i < 10; i++)
            {
                if (taFingerObj[i] != null)
                    vaFingerAngle[i] = taFingerObj[i].localEulerAngles;
            }
            //Save initial rotation of phalanges
            vaFingerPh1Angle[nThumbSensor] = taThumbPhOj[nChildCnt - 1].localEulerAngles;
            vaFingerPh1Angle[nIndexSensor] = taIndexPhObj[nChildCnt - 1].localEulerAngles;
            vaFingerPh1Angle[nMiddleSensor] = taMiddlePhObj[nChildCnt - 1].localEulerAngles;
            vaFingerPh1Angle[nRingSensor] = taRingPhObj[nChildCnt - 1].localEulerAngles;
            vaFingerPh1Angle[nPinkySensor] = taPinkyPhObj[nChildCnt - 1].localEulerAngles;

            vaFingerPh2Angle[nThumbSensor] = taThumbPhOj[nChildCnt].localEulerAngles;
            vaFingerPh2Angle[nIndexSensor] = taIndexPhObj[nChildCnt].localEulerAngles;
            vaFingerPh2Angle[nMiddleSensor] = taMiddlePhObj[nChildCnt].localEulerAngles;
            vaFingerPh2Angle[nRingSensor] = taRingPhObj[nChildCnt].localEulerAngles;
            vaFingerPh2Angle[nPinkySensor] = taPinkyPhObj[nChildCnt].localEulerAngles;

            return 0;
        }


        /* 
            Function: SetFingerSensor
            Saves the sensor ID connected in Captoglove module to each finger.

            Parameters:
            nThumbSensor - Captoglove sensor ID for thumb finger
            nIndexSensor - Captoglove sensor ID for index finger
            nMiddleSensor - Captoglove sensor ID for middle finger 
            nRingSensor - Captoglove sensor ID for ring finger
            nPinkySensor - Captoglove sensor ID for pinky finger
            nPressureSensor - Captoglove sensor ID for pressure sensor

            Example:
            --- Code
            SetFingerSensor(1, 3, 5, 7, 9, 2);
            ---

            Returns:
            0 - Success
            -1 - Error: Sensor ID error

            Notes:
            Sensor ID can be verified in Captoglove documentation. Usually a number between 1 and 10.
        */
        private int SetFingerSensor(int nThumbSensor, int nIndexSensor, int nMiddleSensor, int nRingSensor, int nPinkySensor,
                                       int nPressureSensor)
        {
            if (nThumbSensor < 0 || nThumbSensor > 10 ||
                nIndexSensor < 0 || nIndexSensor > 10 ||
                nMiddleSensor < 0 || nMiddleSensor > 10 ||
                nRingSensor < 0 || nRingSensor > 10 ||
                nPinkySensor < 0 || nPinkySensor > 10 ||
                nPressureSensor < 0 || nPressureSensor > 10)
            {
                TraceLog("Sensor ID error");
                return -1;
            }

            //ArrayPos = SensorID - 1 
            this.nThumbSensor = nThumbSensor - 1;
            this.nIndexSensor = nIndexSensor - 1;
            this.nMiddleSensor = nMiddleSensor - 1;
            this.nRingSensor = nRingSensor - 1;
            this.nPinkySensor = nPinkySensor - 1;
            this.nPressureSensor = nPressureSensor - 1;

            return 0;
        }

        /* 
            Function: SetDefaultRotLimits
            Set the limits for the rotation of the hand object and each finger object.

            Notes: 
            The values configured in this function are valid only for the hand 3D model provided with this library.
        */
        private void SetDefaultRotLimits()
        {
            if (GetHandtype() == HandType.TYPE_RIGHT_HAND)
            {
                SetPitchLimits(90, -90);
                SetYawLimits(90, -90);
                SetRollLimits(-180, 180);

                SetThumbRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
                SetIndexRotLimits(-23.606f, -80, 5.069f, -75, 2.359f, -75);
                SetMiddleRotLimits(-26.575f, -80, 10.864f, -75, -3.127f, -75);
                SetRingRotLimits(-27.302f, -80, 11.405f, -75, -1.038f, -75);
                SetPinkyRotLimits(-24.763f, -80, 6.326f, -75, 5.373f, -75);
            }
            else
            {
                SetPitchLimits(-90, 90);
                SetYawLimits(-90, 90);
                SetRollLimits(-180, 180);

                SetThumbRotLimits(12.681f, 60, -0.992f, 50, 6.269001f, 50);
                SetIndexRotLimits(21.155f, 80, -5.408f, 75, -2.203f, 75);
                SetMiddleRotLimits(24.201f, 80, -10.915f, 75, 3.174f, 75);
                SetRingRotLimits(24.854f, 80, -10.759f, 75, 0.541f, 75);
                SetPinkyRotLimits(22.229f, 80, -5.971f, 75, -5.211f, 75);
            }

            SetOmittedDegrees(2);
        }

        /* 
            Function: SetPitchLimits
            Creates the algorithm for pitch rotation of the hand. 

            Parameters:
            fMaxUpRotation - Angle of rotation where the hand is pointing upward in the pitch movement
            fMaxDownRotation - Angle of rotation where the hand is pointing downward in the pitch movement

            Example:
            --- Code
            SetPitchLimits(90, -90);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for hand object.
        */
        private void SetPitchLimits(float fMaxUpRotation, float fMaxDownRotation)
        {
            float fCaptogloveUpLimit = 0.5f;
            float fCaptogloveDownLimit = -0.5f;

            fPitchEqM = (fMaxUpRotation - fMaxDownRotation) / (fCaptogloveUpLimit - fCaptogloveDownLimit);
            fPitchEqB = fMaxDownRotation - fPitchEqM * fCaptogloveDownLimit;
        }

        /* 
            Function: SetYawLimits
            Creates the algorithm for yaw rotation of the hand. 

            Parameters:
            fMaxRightRotation - Angle of rotation where the hand is pointing to the right in the yaw movement
            fMaxLeftRotation - Angle of rotation where the hand is pointing to the left in the yaw movement

            Example:
            --- Code
            SetYawLimits(90, -90);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for hand object.
        */
        private void SetYawLimits(float fMaxRightRotation, float fMaxLeftRotation)
        {
            float fCaptogloveRightLimit = 0.5f;
            float fCaptogloveLeftLimit = -0.5f;

            fYawEqM = (fMaxLeftRotation - fMaxRightRotation) / (fCaptogloveRightLimit - fCaptogloveLeftLimit);
            fYawEqB = fMaxRightRotation - fYawEqM * fCaptogloveLeftLimit;
        }

        /* 
            Function: SetRollLimits
            Creates the algorithm for roll rotation of the hand. 

            Parameters:
            fMaxRightRotation - Angle of rotation where the hand is face up after turning it to the right.
            fMaxLeftRotation - Angle of rotation where the hand is face up after turning it to the left.

            Example:
            --- Code
            SetRollLimits(90, -90);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for hand object.
        */
        private void SetRollLimits(float fMaxRightRotation, float fMaxLeftRotation)
        {
            float fCaptogloveRightLimit = 1f;
            float fCaptogloveLeftLimit = -1f;

            fRollEqM = (fMaxLeftRotation - fMaxRightRotation) / (fCaptogloveRightLimit - fCaptogloveLeftLimit);
            fRollEqB = fMaxRightRotation - fRollEqM * fCaptogloveLeftLimit;
        }

        /* 
            Function: SetThumbRotLimits
            Saves the rotation limits for the thumb finger object.

            Parameters:
            fMinRotation - Angle of rotation where the thumb finger is fully extended
            fMaxRotation - Angle of rotation where the thumb finger is fully bent
            fMinRotation1 - Angle of rotation where the first child is fully extended
            fMaxRotation1 - Angle of rotation where the first child is fully bent
            fMinRotation2 - Angle of rotation where the second child is fully extended
            fMaxRotation2 - Angle of rotation where the second child is fully bent

            Example:
            --- Code
            SetThumbRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for finger object.
        */
        private void SetThumbRotLimits(float fMinRotation, float fMaxRotation,
                                     float fMinRotation1, float fMaxRotation1,
                                     float fMinRotation2, float fMaxRotation2)
        {
            faFingerMinRot[nThumbSensor] = fMinRotation;
            faFingerMaxRot[nThumbSensor] = fMaxRotation;

            faFingerPh1MinRot[nThumbSensor] = fMinRotation1;
            faFingerPh1MaxRot[nThumbSensor] = fMaxRotation1;

            faFingerPh2MinRot[nThumbSensor] = fMinRotation2;
            faFingerPh2MaxRot[nThumbSensor] = fMaxRotation2;
        }

        /* 
            Function: SetIndexRotLimits
            Saves the rotation limits for the index finger object.

            Parameters:
            fMinRotation - Angle of rotation where the index finger is fully extended
            fMaxRotation - Angle of rotation where the index finger is fully bent
            fMinRotation1 - Angle of rotation where the first child is fully extended
            fMaxRotation1 - Angle of rotation where the first child is fully bent
            fMinRotation2 - Angle of rotation where the second child is fully extended
            fMaxRotation2 - Angle of rotation where the second child is fully bent

            Example:
            --- Code
            SetIndexRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for finger object.
        */
        private void SetIndexRotLimits(float fMinRotation, float fMaxRotation,
                                     float fMinRotation1, float fMaxRotation1,
                                     float fMinRotation2, float fMaxRotation2)
        {
            faFingerMinRot[nIndexSensor] = fMinRotation;
            faFingerMaxRot[nIndexSensor] = fMaxRotation;

            faFingerPh1MinRot[nIndexSensor] = fMinRotation1;
            faFingerPh1MaxRot[nIndexSensor] = fMaxRotation1;

            faFingerPh2MinRot[nIndexSensor] = fMinRotation2;
            faFingerPh2MaxRot[nIndexSensor] = fMaxRotation2;
        }

        /* 
            Function: SetMiddleRotLimits
            Saves the rotation limits for the middle finger object.

            Parameters:
            fMinRotation - Angle of rotation where the middle finger is fully extended
            fMaxRotation - Angle of rotation where the middle finger is fully bent
            fMinRotation1 - Angle of rotation where the first child is fully extended
            fMaxRotation1 - Angle of rotation where the first child is fully bent
            fMinRotation2 - Angle of rotation where the second child is fully extended
            fMaxRotation2 - Angle of rotation where the second child is fully bent

            Example:
            --- Code
            SetMiddleRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for finger object.
        */
        private void SetMiddleRotLimits(float fMinRotation, float fMaxRotation,
                                      float fMinRotation1, float fMaxRotation1,
                                      float fMinRotation2, float fMaxRotation2)
        {
            faFingerMinRot[nMiddleSensor] = fMinRotation;
            faFingerMaxRot[nMiddleSensor] = fMaxRotation;

            faFingerPh1MinRot[nMiddleSensor] = fMinRotation1;
            faFingerPh1MaxRot[nMiddleSensor] = fMaxRotation1;

            faFingerPh2MinRot[nMiddleSensor] = fMinRotation2;
            faFingerPh2MaxRot[nMiddleSensor] = fMaxRotation2;
        }

        /* 
            Function: SetRingRotLimits
            Saves the rotation limits for the ring finger object.

            Parameters:
            fMinRotation - Angle of rotation where the ring finger is fully extended
            fMaxRotation - Angle of rotation where the ring finger is fully bent
            fMinRotation1 - Angle of rotation where the first child is fully extended
            fMaxRotation1 - Angle of rotation where the first child is fully bent
            fMinRotation2 - Angle of rotation where the second child is fully extended
            fMaxRotation2 - Angle of rotation where the second child is fully bent

            Example:
            --- Code
            SetRingRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for finger object.
        */
        private void SetRingRotLimits(float fMinRotation, float fMaxRotation,
                                    float fMinRotation1, float fMaxRotation1,
                                    float fMinRotation2, float fMaxRotation2)
        {
            faFingerMinRot[nRingSensor] = fMinRotation;
            faFingerMaxRot[nRingSensor] = fMaxRotation;

            faFingerPh1MinRot[nRingSensor] = fMinRotation1;
            faFingerPh1MaxRot[nRingSensor] = fMaxRotation1;

            faFingerPh2MinRot[nRingSensor] = fMinRotation2;
            faFingerPh2MaxRot[nRingSensor] = fMaxRotation2;
        }

        /* 
            Function: SetPinkyRotLimits
            Saves the rotation limits for the pinky finger object.

            Parameters:
            fMinRotation - Angle of rotation where the pinky finger is fully extended
            fMaxRotation - Angle of rotation where the pinky finger is fully bent
            fMinRotation1 - Angle of rotation where the first child is fully extended
            fMaxRotation1 - Angle of rotation where the first child is fully bent
            fMinRotation2 - Angle of rotation where the second child is fully extended
            fMaxRotation2 - Angle of rotation where the second child is fully bent

            Example:
            --- Code
            SetPinkyRotLimits(-9.475f, -60, -6.888f, -50, -6.334f, -50);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for finger object.
        */
        private void SetPinkyRotLimits(float fMinRotation, float fMaxRotation,
                                     float fMinRotation1, float fMaxRotation1,
                                     float fMinRotation2, float fMaxRotation2)
        {
            faFingerMinRot[nPinkySensor] = fMinRotation;
            faFingerMaxRot[nPinkySensor] = fMaxRotation;

            faFingerPh1MinRot[nPinkySensor] = fMinRotation1;
            faFingerPh1MaxRot[nPinkySensor] = fMaxRotation1;

            faFingerPh2MinRot[nPinkySensor] = fMinRotation2;
            faFingerPh2MaxRot[nPinkySensor] = fMaxRotation2;
        }

        /* 
            Function: MoveHand
            Updates hand object rotation according with Captoglove module movement.

            Notes: 
            Call this function in the Update() function of Unity script.
        */
        public void MoveHand()
        {
                                    //If hand object was assigned
            if (GetModuleStarted() && tHandObj != null)
            {
                SetHandAngle(true);
                tHandObj.localEulerAngles = new Vector3(fHandXangle, fHandYangle, fHandZangle);
            }
        }

        /* 
            Function: MoveHandNoYaw
            Updates hand object rotation according with Captoglove module movement. Yaw movement is ommited.

            Notes: 
            Call this function in the Update() function of Unity script.
            Normally used when arm simulation is also running so the yaw movement is done by the arm.
        */
        public void MoveHandNoYaw()
        {
            if (GetModuleStarted())
                SetHandAngle(false);

            //If hand object was assigned
            if (tHandObj != null)
                tHandObj.localEulerAngles = new Vector3(fHandXangle, fHandYangle, fHandZangle);
        }

        /* 
            Function: SetHandAngle
            Calculates hand object rotation according with Captoglove module movement.   

            Parameters:
            bYaw - true or false to simulate yaw movement

        */
        private void SetHandAngle(bool bYaw)
        {
            var args = sEventTaredQuart as BoardQuaternionEventArgs;
            float pitchAngle;
            float yawAngle;
            float rollAngle;

            if (args != null)
            {
                float quaternionX = args.Value.X;
                float quaternionY = args.Value.Y;
                float quaternionZ = args.Value.Z;

                //Equation of a line (x= y*M + B)
                pitchAngle  = quaternionX * fPitchEqM + fPitchEqB;
                yawAngle    = quaternionY * fYawEqM + fYawEqB;
                rollAngle   = quaternionZ * fRollEqM + fRollEqB;

                if (!bYaw)
                    yawAngle = 0;

               Angle2Axes(pitchAngle, yawAngle, rollAngle);

            }
        }

        /* 
            Function: Angle2Axes
            Set rotation angle to each axis of the hand object. 

            Parameters:
            fPitchA - Angle of pitch rotation
            fYawA - Angle of yaw rotation
            fRollA - Angle of roll rotation
        */
        private void Angle2Axes(float fPitchA, float fYawA, float fRollA)
        {
            switch (ePitchAxis)
            {
                case ModuleAxis.AXIS_X:
                    fHandXangle = fPitchA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fHandYangle = fPitchA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fHandZangle = fPitchA;
                    break;
            }

            switch (eYawAxis)
            {
                case ModuleAxis.AXIS_X:
                    fHandXangle = fYawA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fHandYangle = fYawA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fHandZangle = fYawA;
                    break;
            }

            switch (eRollAxis)
            {
                case ModuleAxis.AXIS_X:
                    fHandXangle = fRollA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fHandYangle = fRollA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fHandZangle = fRollA;
                    break;
            }
        }

        /* 
            Function: MoveFingers
            Updates each finger object rotation according with Captoglove sensor value.

            Notes: 
            Call this function in the Update() function of Unity script to simulate fingers movement.
        */
        public void MoveFingers()
        {
            float temp;

            //Needed to capture fingers gestures
            if (GetModuleStarted())
                SetFingersAngle();
            
            //If finger objects were assigne, update model rotation
            if (taFingerObj[nThumbSensor] != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    //Calculate new rotation
                    temp = faSensorValue[i] * faFingerEqM[i] + faFingerEqB[i];              
                    
                    //Avoid shaking
                    if (Mathf.Abs(temp - vaFingerAngle[i].x) > nOmittedDegrees)
                    {
                        vaFingerAngle[i].x = temp;
                        vaFingerPh1Angle[i].x = faSensorValue[i] * faFingerPh1EqM[i] + faFingerPh1EqB[i];
                        vaFingerPh2Angle[i].x = faSensorValue[i] * faFingerPh2EqM[i] + faFingerPh2EqB[i];
                    }

                    //Evaluate rotation limits
                    if (GetHandtype() == HandType.TYPE_RIGHT_HAND &&
                        (vaFingerAngle[i].x > faFingerMinRot[i]))
                    {
                        vaFingerAngle[i].x = faFingerMinRot[i];
                        vaFingerPh1Angle[i].x = faFingerPh1MinRot[i];
                        vaFingerPh2Angle[i].x = faFingerPh2MinRot[i];
                    }
                    else if (GetHandtype() == HandType.TYPE_LEFT_HAND &&
                            (vaFingerAngle[i].x < faFingerMinRot[i]))
                    {
                        vaFingerAngle[i].x = faFingerMinRot[i];
                        vaFingerPh1Angle[i].x = faFingerPh1MinRot[i];
                        vaFingerPh2Angle[i].x = faFingerPh2MinRot[i];
                    }

                    //Set finger new rotation
                    if (taFingerObj[i] != null)
                        taFingerObj[i].localEulerAngles = vaFingerAngle[i];
                }
                
                //Set phalanges new rotation
                taThumbPhOj[1].localEulerAngles = vaFingerPh1Angle[nThumbSensor];
                taIndexPhObj[1].localEulerAngles = vaFingerPh1Angle[nIndexSensor];
                taMiddlePhObj[1].localEulerAngles = vaFingerPh1Angle[nMiddleSensor];
                taRingPhObj[1].localEulerAngles = vaFingerPh1Angle[nRingSensor];
                taPinkyPhObj[1].localEulerAngles = vaFingerPh1Angle[nPinkySensor];

                taThumbPhOj[2].localEulerAngles = vaFingerPh2Angle[nThumbSensor];
                taIndexPhObj[2].localEulerAngles = vaFingerPh2Angle[nIndexSensor];
                taMiddlePhObj[2].localEulerAngles = vaFingerPh2Angle[nMiddleSensor];
                taRingPhObj[2].localEulerAngles = vaFingerPh2Angle[nRingSensor];
                taPinkyPhObj[2].localEulerAngles = vaFingerPh2Angle[nPinkySensor];
            }            
        }

        /* 
            Function: SetFingersAngle
            Calculates each finger object rotation according with Captoglove sensor value.              
        */
        private void SetFingersAngle()
        {
            var args = sEventSensorState as BoardFloatSequenceEventArgs;            

            if (args != null)
            {
                if (!GetFingerEquationReady())
                {
                    SetFingerEquation();
                    for (int i = 0; i < 10; i++)
                    {
                        faSensorTrigger[i] = (faFingerSensorMaxValue[i] - faFingerSensorMinValue[i]) / 3;
                    }
                }
                
                for (int i = 0; i < 10; i++)
                {
                    faSensorValue[i] = args.Value[i];
                }
            }
        }

        /* 
            Function: SetFingerEquation
            Creates algorithm for finger movement according with sensor calibration.
        */
        private void SetFingerEquation()
        {
            float num = 0f;

            for (int i = 0; i < 10; i++)
            {
                num = faFingerSensorMinValue[i] - faFingerSensorMaxValue[i];

                if (num != 0f)
                {
                    faFingerEqM[i]      = (faFingerMaxRot[i] - faFingerMinRot[i]) / num;
                    faFingerPh1EqM[i]   = (faFingerPh1MaxRot[i] - faFingerPh1MinRot[i]) / num;
                    faFingerPh2EqM[i]   = (faFingerPh2MaxRot[i] - faFingerPh2MinRot[i]) / num;
                }

                faFingerEqB[i]    = faFingerMinRot[i] - (faFingerEqM[i] * faFingerSensorMaxValue[i]);
                faFingerPh1EqB[i] = faFingerPh1MinRot[i] - (faFingerPh1EqM[i] * faFingerSensorMaxValue[i]);
                faFingerPh2EqB[i] = faFingerPh2MinRot[i] - (faFingerPh2EqM[i] * faFingerSensorMaxValue[i]);
            }

            //M and B are set correctly only after module properties are read
            if (GetPropertiesRead())
            {
                TraceLog("Finger algorithm ready");
                SetFingerEquationReady(true);
            }
        }

        /* 
            Function: GetHandPosition
            Returns:
            Global position of hand object
        */
        public Vector3 GetHandPosition()
        {
            if (tHandObj != null)
                return tHandObj.position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetHandRotation
            Returns:
            Global euler angles of hand object
        */
        public Vector3 GetHandRotation()
        {
            if (tHandObj != null)
                return tHandObj.eulerAngles;
            else
                return new Vector3(0, 0, 0);
        }

         /* 
            Function: GetThumbPosition
            Returns:
            Global position of thumb finger object
        */
        public Vector3 GetThumbPosition()
        {
            if (taFingerObj[nThumbSensor] != null)
                return taFingerObj[nThumbSensor].position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetIndexPosition
            Returns:
            Global position of index finger object
        */
        public Vector3 GetIndexPosition()
        {
            if (taFingerObj[nIndexSensor] != null)
                return taFingerObj[nIndexSensor].position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetMiddlePosition
            Returns:
            Global position of middle finger object
        */
        public Vector3 GetMiddlePosition()
        {
            if (taFingerObj[nMiddleSensor] != null)
                return taFingerObj[nMiddleSensor].position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetRingPosition
            Returns:
            Global position of ring finger object
        */
        public Vector3 GetRingPosition()
        {
            if (taFingerObj[nRingSensor] != null)
                return taFingerObj[nRingSensor].position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetPinkyPosition
            Returns:
            Global position of pinky finger object
        */
        public Vector3 GetPinkyPosition()
        {
            if (taFingerObj[nPinkySensor] != null)
                return taFingerObj[nPinkySensor].position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: SensorPressed
            Returns:
            true - Pressure sensor is being pressed
            false - Pressure sensor is released        
        */
        public bool SensorPressed()
        {
            bool bRet = false;

            if (GetModuleStarted())
            {
                if (faSensorValue[nPressureSensor] > faSensorTrigger[nPressureSensor])
                    bRet = true;
            }

            return bRet;
        }

        /* 
            Function: HandClosed
            Returns:
            true - All fingers are bent more than 50%
            false - All fingers are extended or bent less than 50%
        */
        public bool HandClosed()
        {
            bool bRet = false;

            if (GetModuleStarted())
            {
                if (faSensorValue[nIndexSensor] < faSensorTrigger[nIndexSensor] &&
                    faSensorValue[nMiddleSensor] < faSensorTrigger[nMiddleSensor] &&
                    faSensorValue[nRingSensor] < faSensorTrigger[nRingSensor] &&
                    faSensorValue[nPinkySensor] < faSensorTrigger[nPinkySensor])
                {

                    bRet = true;
                }
            }

            return bRet;
        }

        /* 
            Function: FingerGesture1
            Returns:
            true - Index finger is extended or bent less than 50% and the other fingers are bent more than 50%
            false - Condition is not met
        */
        public bool FingerGesture1()
        {
            bool bRet = false;

            if (GetModuleStarted())
            {
                if (faSensorValue[nIndexSensor] > faSensorTrigger[nIndexSensor] &&
                    faSensorValue[nMiddleSensor] < faSensorTrigger[nMiddleSensor] &&
                    faSensorValue[nRingSensor] < faSensorTrigger[nRingSensor] &&
                    faSensorValue[nPinkySensor] < faSensorTrigger[nPinkySensor])
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        /* 
            Function: FingerGesture2
            Returns:
            true - Index and middle finger are extended or bent less than 50% and the other fingers are bent more than 50%
            false - Condition is not met
        */
        public bool FingerGesture2()
        {
            bool bRet = false;

            if (GetModuleStarted())
            {
                if (faSensorValue[nIndexSensor] > faSensorTrigger[nIndexSensor] &&
                    faSensorValue[nMiddleSensor] > faSensorTrigger[nMiddleSensor] &&
                    faSensorValue[nRingSensor] < faSensorTrigger[nRingSensor] &&
                    faSensorValue[nPinkySensor] < faSensorTrigger[nPinkySensor])
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        /* 
            Function: FingerGesture3
            Returns:
            true - Index, middle and ring finger are extended or bent less than 50% and the other fingers are bent more than 50%
            false - Condition is not met
        */
        public bool FingerGesture3()
        {
            bool bRet = false;

            if (GetModuleStarted())
            {
                if (faSensorValue[nIndexSensor] > faSensorTrigger[nIndexSensor] &&
                    faSensorValue[nMiddleSensor] > faSensorTrigger[nMiddleSensor] &&
                    faSensorValue[nRingSensor] > faSensorTrigger[nRingSensor] &&
                    faSensorValue[nPinkySensor] < faSensorTrigger[nPinkySensor])
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        /* 
            Function: SaveHandInFile
            Saves module data in file with following format: x hand rotation; y hand rotation; z hand rotation [quaternions]

            Parameters:
            sFileName - File name

            Example:
            --- Code
            SaveHandInFile("RightHandMov.csv");
            ---

            Notes:
            Call this function in Updated() of Unity script to save data continuously 
        */
        public void SaveHandInFile(string sFileName)
        {
            var args = sEventTaredQuart as BoardQuaternionEventArgs;
            string serializedData;

            if (args != null)
            {
                if (!bHandFile)
                {
                    wHandWriter = new StreamWriter(sFileName, true);
                    bHandFile = true;
                }

                float quaternionX = args.Value.X;
                float quaternionY = args.Value.Y;
                float quaternionZ = args.Value.Z;

                serializedData =
                    quaternionX.ToString() + ";" +
                    quaternionY.ToString() + ";" +
                    quaternionZ.ToString() + "\n";

                // Write to disk
                if (wHandWriter != null)
                    wHandWriter.Write(serializedData);

            }
        }

        /* 
            Function: SaveFingerInFile
            Saves sensor data in file with following format: thumb finger; index finger; middle finger; ring finger; pinky finger; pressure sensor [conductivity S/m]

            Parameters:
            sFileName - File name

            Example:
            --- Code
            SaveFingerInFile("RightFingerMov.csv");
            ---

            Notes:
            Call this function in Updated() of your app to save data continuously 
        */
        public void SaveFingerInFile(string sFileName)
        {
            var args = sEventSensorState as BoardFloatSequenceEventArgs;
            string serializedData;

            if (args != null)
            {
                if (!bFingerFile)
                {
                    wFingerWriter = new StreamWriter(sFileName, true);
                    bFingerFile = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    faSensorValue[i] = args.Value[i];
                }

                serializedData =
                    faSensorValue[nThumbSensor].ToString() + ";" +
                    faSensorValue[nIndexSensor].ToString() + ";" +
                    faSensorValue[nMiddleSensor].ToString() + ";" +
                    faSensorValue[nRingSensor].ToString() + ";" +
                    faSensorValue[nPinkySensor].ToString() + ";" +
                    faSensorValue[nPressureSensor].ToString() + "\n";

                // Write to disk
                if (wFingerWriter != null)
                    wFingerWriter.Write(serializedData);

            }
        }

    }
}
