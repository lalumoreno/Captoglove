using UnityEngine;
using System.Threading;

public class Example : MonoBehaviour
{
    // Assign an absolute rotation using eulerAngles
    float yRotation = 5.0f;
	public Transform dedos;
	public Vector3 temp;
	
    void Start()
    {
		temp = dedos.localEulerAngles;
        // Print the rotation around the global X Axis
        print(transform.eulerAngles.x);
		print(temp.x);
        // Print the rotation around the global Y Axis
        print(transform.eulerAngles.y);
        // Print the rotation around the global Z Axis
        print(transform.eulerAngles.z);
		
		Thread.Sleep(5);
		
    }

    void Update()
    {
        yRotation = transform.eulerAngles.y + 1;
        transform.eulerAngles = new Vector3(10, yRotation, 0);
		print("Rotate");
		
		temp.x = temp.x+1;
		dedos.transform.localEulerAngles = new Vector3(temp.x,temp.y, temp.z);
    }
}
