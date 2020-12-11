using System.IO;
using UnityEngine;

//SDK Captoglove
using GSdkNet.Board;

namespace GITEICaptoglove
{
    /* 
        Class: MyArm
        Handles Captoglove module configured as forearm sensor.

    	Author: 
		Laura Moreno - laamorenoro@unal.edu.co 
		
		Copyrigth:		
		Copyrigth 2020 Universidad Nacional de Colombia, all rigths reserved. 	    
    */
    public class MyArm : Module
    {
        /* 
            Enum: ArmType
            List of possible ways to use Captoglove module with this class:

            TYPE_RIGHT_FOREARM - As right forearm sensor
            TYPE_LEFT_FOREARM - As left forearm sensor       
        */
        public enum ArmType
        {
            TYPE_RIGHT_FOREARM,
            TYPE_LEFT_FOREARM
        }

        private ArmType eArmType;
        private ModuleAxis ePitchAxis, eYawAxis, eRollAxis;

        //Angles and equation constants (y= x*M + B)
        private float fArmXangle, fArmYangle, fArmZangle;
        private float fPitchEqM, fPitchEqB, fYawEqM, fYawEqB;

        private Transform tArmObj = null;

        //Save data in file
        private StreamWriter wArmWriter = null;
        private bool bArmFile = false;

        /* 
            Constructor: MyArm
            Initializes variables for Captoglove module configuration.

            Parameters:
            nID - Captoglove ID (4 digits number)
            etype - Captoglove use mode

            Example:
            --- Code
            MyArm RightArm = new MyArm(2469, MyArm.ArmType.TYPE_RIGHT_FOREARM);
            ---
        */
        public MyArm(int nID, ArmType eType)
        {
            SetArmType(eType);

            if (eType == ArmType.TYPE_RIGHT_FOREARM)
            {
                InitModule(nID, Module.ModuleType.TYPE_RIGHT_ARM);
            }
            else
            {
                InitModule(nID, Module.ModuleType.TYPE_LEFT_ARM);
            }

            //Set default rotation limits for hands 3D model provided with this library
            SetDefaultRotLimits();
        }

        /* 
            Function: SetArmType
            Saves Captoglove module use mode.

            Parameters:
            eType - Captoglove module use mode

            Example:
            --- Code
            SetArmType(MyArm.ArmType.TYPE_RIGHT_FOREARM);
            ---
        */
        private void SetArmType(ArmType eType)
        {
            eArmType = eType;
        }

        /* 
            Function: GetArmType
            Returns:
            Captoglove module use mode
        */
        public ArmType GetArmType()
        {
            return eArmType;
        }

        /* 
            Function: SetArmObject
            Attaches Captoglove module movement to arm object.     

            Parameters:
            tArmObj - Forearm object
            
            Returns: 
            0 - Success
            -1 - Object error

            Example:
            --- Code
            SetArmObject(transRA);
            ---

            Notes: 
            Place the arm object horizontally in the scene before assigning it in this function to save default position.        
        */
        public int SetArmObject(Transform tArmObj)
        {
            if (tArmObj == null)
            {
                TraceLog("Arm transform error");
                return -1;
            }

            this.tArmObj = tArmObj;
            //Default angles for 3D model provided with this library
            this.ePitchAxis = Module.ModuleAxis.AXIS_Y;
            this.eYawAxis = Module.ModuleAxis.AXIS_Z;
            this.eRollAxis = Module.ModuleAxis.AXIS_X;
            //Initial rotation 
            fArmXangle = this.tArmObj.localEulerAngles.x;
            fArmYangle = this.tArmObj.localEulerAngles.y;
            fArmZangle = this.tArmObj.localEulerAngles.z;

            return 0;
        }

        /* 
            Function: SetInitialRot
            Saves initial rotation for arm object.

            Parameters:
            fRotX - Object x rotation
            fRotY - Object y rotation
            fRotZ - Object z rotation

            Example:
            --- Code
            SetInitialRot(0, 90, -90);
            ---

            Notes: 
            Use this function to manually set the initial rotation of arm object in case SetArmObject() is saving wrong values by default.
            Use this function after SetArmObject().
        */
        public void SetInitialRot(float fRotX, float fRotY, float fRotZ)
        {
            fArmXangle = fRotX;
            fArmYangle = fRotY;
            fArmZangle = fRotZ;
        }

        /* 
            Function: SetDefaultRotLimits
            Set the limits for the rotation of the arm object.

            Notes: 
            The values configured in this function are valid only for the arm 3D model provided with this library.
        */
        private void SetDefaultRotLimits()
        {
            if (GetArmType() == ArmType.TYPE_RIGHT_FOREARM)
            {
                SetPitchLimits(-90, 90);
                SetYawLimits(0, -180);
            }
            else
            {
                SetPitchLimits(90, -90);
                SetYawLimits(0, 180);
            }
        }

        /* 
            Function: SetPitchLimits
            Creates the algorithm for pitch rotation of the arm. 

            Parameters:
            fMaxUpRotation - Angle of rotation where the arm is pointing upward in the pitch movement
            fMaxDownRotation - Angle of rotation where the arm is pointing downward in the pitch movement

            Example:
            --- Code
            SetPitchLimits(90, -90);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for arm object.
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
            Creates the algorithm for yaw rotation of the arm. 

            Parameters:
            fMaxRightRotation - Angle of rotation where the arm is pointing to the right in the yaw movement
            fMaxLeftRotation - Angle of rotation where the arm is pointing to the left in the yaw movement

            Example:
            --- Code
            SetYawLimits(90, -90);
            ---

            Notes: 
            Values must be set as they are read in Unity Inspector panel for arm object.
        */
        private void SetYawLimits(float fMaxRightRotation, float fMaxLeftRotation)
        {
            float fCaptogloveRightLimit = 0.5f;
            float fCaptogloveLeftLimit = -0.5f;

            fYawEqM = (fMaxLeftRotation - fMaxRightRotation) / (fCaptogloveRightLimit - fCaptogloveLeftLimit);
            fYawEqB = fMaxRightRotation - fYawEqM * fCaptogloveLeftLimit;
        }

        /* 
            Function: MoveArm
            Updates arm object rotation according with Captoglove module movement.

            Notes: 
            Call this function in the Update() function of Unity script to simulate arm movement.
        */
        public void MoveArm()
        {
            if (GetModuleInitialized())
                SetArmAngle();

            //If hand transform was assigned
            if (GetModuleInitialized() && tArmObj != null)
                tArmObj.localEulerAngles = new Vector3(fArmXangle, fArmYangle, fArmZangle);
        }

        /* 
            Function: SetArmAngle
            Calculates arm object rotation according with Captoglove module quaternion rotation.    
        */
        private void SetArmAngle()
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

                pitchAngle  = quaternionX * fPitchEqM + fPitchEqB; //Equation of a line (x= y*M+B)
                yawAngle    = quaternionY * fYawEqM + fYawEqB;
                rollAngle   = fArmXangle; //Roll rotation is handled by hand, not by forearm

                Angle2Axes(pitchAngle, yawAngle, rollAngle);
            }
        }

        /* 
            Function: Angle2Axes
            Set rotation angle to each axis of the arm object. 

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
                    fArmXangle = fPitchA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fArmYangle = fPitchA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fArmZangle = fPitchA;
                    break;
            }

            switch (eYawAxis)
            {
                case ModuleAxis.AXIS_X:
                    fArmXangle = fYawA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fArmYangle = fYawA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fArmZangle = fYawA;
                    break;
            }

            switch (eRollAxis)
            {
                case ModuleAxis.AXIS_X:
                    fArmXangle = fRollA;
                    break;
                case ModuleAxis.AXIS_Y:
                    fArmYangle = fRollA;
                    break;
                case ModuleAxis.AXIS_Z:
                    fArmZangle = fRollA;
                    break;
            }
        }

        /* 
            Function: GetArmPosition
            Returns:
            Global position of arm object
        */
        public Vector3 GetArmPosition()
        {
            if (tArmObj != null)
                return tArmObj.position;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: GetArmRotation
            Returns:
            Global euler angles of arm object
        */
        public Vector3 GetArmRotation()
        {
            if (tArmObj != null)
                return tArmObj.eulerAngles;
            else
                return new Vector3(0, 0, 0);
        }

        /* 
            Function: SaveInFile
            Saves module data in file with following format: x arm rotation; y arm rotation; z arm rotation  [quaternions]

            Parameters:
            sFileName - File name

            Example:
            --- Code
            SaveInFile("RightArmMov.csv");
            ---

            Notes:
            Call this function in Updated() function of Unity script to save data continuously 
        */
        public void SaveInFile(string sFileName)
        {
            var args = sEventTaredQuart as BoardQuaternionEventArgs;
            string serializedData;

            if (args != null)
            {
                if (!bArmFile)
                {
                    wArmWriter = new StreamWriter(sFileName, true);
                    bArmFile = true;
                }

                float quaternionX = args.Value.X;
                float quaternionY = args.Value.Y;
                float quaternionZ = args.Value.Z;

                serializedData =
                    quaternionX.ToString() + ";" +
                    quaternionY.ToString() + ";" +
                    quaternionZ.ToString() + "\n";

                // Write to disk
                if (wArmWriter != null)
                    wArmWriter.Write(serializedData);

            }
        }
    }
}
