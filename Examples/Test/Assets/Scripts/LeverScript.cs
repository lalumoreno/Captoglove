using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this object needs to have Rigidbody
public class LeverScript : MonoBehaviour {

	public Transform door;
	public Transform lever;
	
	public Vector3 leverpos; 
	public Vector3 initialpos;
	public float euler, tm; 
	private bool stay = false; 
	
	bool doorOpen = false; 	
	Vector3 closedPosition = new Vector3(-5.2f,13f,55.91f);
	Vector3 openedPosition = new Vector3(-5.2f,30.9f,55.91f);
	float openSpeed = 5;	
	
	void Update()
	{
		
		//if(doorOpen)
		//if (Input.GetMouseButtonDown(0))
		if (Input.GetKeyDown(KeyCode.Q))
		{
			door.position = Vector3.Lerp(door.position,
				openedPosition, Time.deltaTime * openSpeed);
		} //		else
		else if (Input.GetKeyDown(KeyCode.W))	
		{
			door.position = Vector3.Lerp(door.position,
				closedPosition, Time.deltaTime * openSpeed);
			
		}
		
		lever.transform.localPosition = new Vector3(leverpos.x,leverpos.y,leverpos.z);
	}
	
	void Start()
	{
		initialpos = lever.localPosition; //initial pos 
		leverpos = initialpos;
		
	}
	
	//If something is inside the trigger
	void OnTriggerEnter(Collider col)
	{
		//If you press the key E once
		if(col.tag =="Player")
		{
			OpenDoor();
			stay = true; 
		}
		
	}
	
	void OnTriggerStay(Collider col)
	{	
		if(col.tag =="Player")
		{
			GameObject gHit = col.gameObject;
			Transform tHit  = gHit.transform;
			Vector3 newpos  = new Vector3(tHit.localEulerAngles.x,
									  tHit.localEulerAngles.y,
									  tHit.localEulerAngles.z);
		//Vector3 newpos = col.transform.localEulerAngles;
			
			euler = newpos.z;
			
			if(newpos.z >= 124f)
			{
				tm = newpos.z/76f;
				leverpos.y = -tm; 
			}/*
			else 
			{
				tm = -newpos.z/76f; 
				leverpos.y = tm; 
			}*/
		}
										
	}
	
	void OnTriggerExit(Collider col)
	{
		//If you press the key E once
		if(col.tag =="Player")
		{		
			CloseDoor();			
			//leverpos.x = initialpos.x;
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
