using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this object needs to have Rigidbody
public class LeverScript : MonoBehaviour {

	public Transform door;
	
	bool doorOpen = false; 	
	Vector3 closedPosition = new Vector3(-5.2f,13f,55.91f);
	Vector3 openedPosition = new Vector3(-5.2f,30.9f,55.91f);
	float openSpeed = 5;	
	
	void Update()
	{
		if(doorOpen)
		{
			door.position = Vector3.Lerp(door.position,
				openedPosition, Time.deltaTime * openSpeed);
		}
		else
		{
			door.position = Vector3.Lerp(door.position,
				closedPosition, Time.deltaTime * openSpeed);
			
		}
		
	}
	
	//If something is inside the trigger
	void OnTriggerEnter(Collider col)
	{
		//If you press the key E once
		if(col.tag =="Player")
		{
			OpenDoor();
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		//If you press the key E once
		if(col.tag =="Player")
		{		
			CloseDoor();			
		}
	}
	
	void CloseDoor()
	{
		doorOpen = false;
		Debug.Log("Door closed");
	}
	
	void OpenDoor()
	{
		doorOpen = true; 
		Debug.Log ("Door opened");					
	}
}
