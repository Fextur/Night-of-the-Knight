using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Fire Arrow Script - The script of the fire arrow in the garden
public class FireArrowScript : MonoBehaviour
{
    //Saves the sprite of the door
    SpriteRenderer sprite;
    //Saves the position of the arrow
    Vector3 pos;
    //Flag for if the character passed the shooting point
    bool shootPoint = false;
    //Flag for if the arrow failed
    bool fail = false;
    //Saves the target
    Vector3 target;

    //Called in initialization
    void Start ()
    {
        //Saves the sprite of the door
        sprite = GetComponent<SpriteRenderer>();
    }
	
	//Called once per frame
	void Update ()
    {
        //Saves the position of the arrow and the player
        pos = transform.position;
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        //If the arrow didn't fail, target is the player position
        if (!fail) target = playerPos;
        //Saves the width and height of the door
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the sides of the arrow
        float ArrowLeft = pos.x - width / 2;
        float ArrowRight = pos.x + width / 2;
        float ArrowDown = pos.y - height / 2;
        float ArrowUp = pos.y + height / 2;
        //If the player passed the shoot point (0, in the middle of the screen), sets shootPoint to true
        if (playerPos.x <= 0) shootPoint = true;
        //If the player passed the shoot point already, moves the arrow to the player
        if (shootPoint) transform.position = Vector3.MoveTowards(pos, target, 8f * Time.deltaTime);
        //If the arrow is in the range of the player and hasn't failed
        if (!fail && playerPos.x > ArrowLeft && playerPos.x < ArrowRight && playerPos.y < ArrowUp && playerPos.y > ArrowDown - 0.5f)
        {
            //If the player uses the orb
            if (GameObject.Find("DuncanJr").GetComponent<Animator>().GetBool("Orbing"))
            {
                //Turn fail flag true and change the target to the ground
                fail = true;
                target = pos;
                target.y = -2f;
                target.x -= 2f;
            }
            //Else, change to Death Screen
            else SceneManager.LoadScene("DeathScreen");
        }

    }
}
