using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic Scene Script - The basic sript for every scene
public class BasicSceneScript : MonoBehaviour
{

    //Called in initialization 
    void Start () {
		
	}
	
	//Called once per frame
	void Update ()
    {
        //Clean the console
        ClearConsole();
    }

    //Clean the console (for debugging)
    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }
}
