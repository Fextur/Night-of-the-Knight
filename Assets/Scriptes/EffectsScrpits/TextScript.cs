using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TextScript - Script for all the text
public class TextScript : MonoBehaviour
{
    //Saves the text component
    public TextMeshProUGUI printer;

    //Called in initialization
    void Start ()
    {

        //Saves the text component and sets it to nothing
        printer = GetComponent<TextMeshProUGUI>();
        printer.text = "";
    }
	
	//Called once per frame
	void Update ()
    {

	}
}
