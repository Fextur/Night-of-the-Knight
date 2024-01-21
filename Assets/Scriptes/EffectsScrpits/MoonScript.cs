using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MoonScript - The script for the moon in the start screen
public class MoonScript : MonoBehaviour
{

	//Called in initialization
	void Start ()
    {
		
	}
	
	//Called once per frame
	void Update ()
    {
        //change the position towards the wanted y
        Vector3 moonPos = transform.position;
        if (moonPos.y < 3.87f)
            moonPos.y += 20f * Time.deltaTime;
        transform.position = moonPos;
    }
}
