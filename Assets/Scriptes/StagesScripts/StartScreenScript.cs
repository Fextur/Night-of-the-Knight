using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//StartScreenScript - Script for the start screen
public class StartScreenScript : MonoBehaviour
{

	//Called in initialization
	void Start ()
    {
		
	}
	
	//Called once per frame
	void Update ()
    {
        //If pessed 1 sec since the game begun
        if (Time.time >= 1f)
        {
            //Show "press any key"
            GameObject.Find("PressAnyKey").GetComponent<SpriteRenderer>().sortingLayerName = "BackEffects";
            //Any input change to hallroom
            if (Input.anyKey)
            {
                GameObject.Find("GSD").GetComponent<GSDScript>().deathTime = Time.time;
                SceneManager.LoadScene("HallRoom");
            }
        }
        else
        {
            //Hide "press any key"
            GameObject.Find("PressAnyKey").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
        }
	}
}
