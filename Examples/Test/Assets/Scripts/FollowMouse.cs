using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
	public float distance = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = distance;
		transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
	//	 Debug.Log("- Script FollowMouse finished ");
    }
}
