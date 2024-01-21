using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//DeathScreenScript - The script for the death screen
public class DeathScreenScript : MonoBehaviour
{

    //Called in initialization
    void Start ()
    {
		
	}
	
	//Called once per frame
	void Update ()
    {
        if (GameObject.Find("GSD"))
        {
            //Shows the score
            TextMeshProUGUI score = GameObject.Find("Score").GetComponent<TextScript>().printer;
            score.text = "" + GameObject.Find("GSD").GetComponent<GSDScript>().Score;
        }
        //If the player press R, load the hallroom scene
        if (Input.anyKey)
        {
            SceneManager.LoadScene("HallRoom");
            if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<AudioSource>().Play();
        }
    }
}
