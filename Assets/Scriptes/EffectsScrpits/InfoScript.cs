using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//InfoScript - The script for the information panel
public class InfoScript : MonoBehaviour
{
    //Flag for if the panel needs to go up or down
    bool goUp = false;
    //Saves the animation component
    Animator anim;

    //Called in initialization
    void Start ()
    {
        //Saves the animation component
        anim = GetComponent<Animator>();
    }
	
	//Called once per frame
	void Update ()
    {
        //Saves the position and change the position towards where he needs to go
        Vector3 pos = transform.position;
        Vector3 posNew = pos;
        if (goUp)
        {
            posNew.y = 0;
            transform.position = Vector3.MoveTowards(pos, posNew, 50f * Time.deltaTime);
        }
        else
        {
            posNew.y = -4.693f;
            transform.position = Vector3.MoveTowards(pos, posNew, 50f * Time.deltaTime);
        }
        //If pushed I, close the panel if opened and open if it's closed
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (anim.GetBool("Open"))
            {
                goUp = false;
                anim.SetBool("Open", false);
                anim.SetBool("Further", false);
            }
            else
            {
                goUp = true;
                anim.SetBool("Open", true);
            }
        }
        //If the panel is open and pushed enter, change to further information if not in it, else goes back to controlls
        if (Input.GetKeyDown(KeyCode.Return) && anim.GetBool("Open"))
        {
            if (anim.GetBool("Further"))
                anim.SetBool("Further", false);
            else
                anim.SetBool("Further", true);
        }
    }
}
