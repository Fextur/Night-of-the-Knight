using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostScript : MonoBehaviour
{
    //Saves the side the boss is pointing to (1 = left, -1 = right)
    bool side = true;
    //Saves the renderer component, aniimation component and audio component
    public SpriteRenderer sprite;
    public AudioSource audioSource;
    Animator anim;
    //Saves the ghost and player position
    Vector3 pos;
    Vector3 playerPos;
    //Saves the life count of the ghost
    int life = 5;
    //Saves the player height
    float playerHeight;
    //Flag for if the shot has been counted
    bool nextShoot = true;
    //Saves the time the death animation will end
    float deathTime = 0;
    //Saves the current scene
    Scene CurrentScene;
    //Saves the floor the ghost at
    // int ghostFloor = -1;
    //Called in initialization
    void Start ()
    {
        //Saves the renderer component, aniimation component and audio component
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //Saves the player height
        playerHeight = GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.y;
    }
	
	//Called once per frame
	void Update ()
    {
        //If death animation end time has been seted
        if (deathTime != 0)
        {
            //If the death animation ended, hides
            if (deathTime <= Time.time) sprite.sortingLayerName = "BTS";
            return;
        }
        //If no audio is playing, plays the ghost sound
        if (!audioSource.isPlaying)
        {
            audioSource.clip = Resources.Load<AudioClip>("Audio/GhostSounds/Boo");
            audioSource.Play();
        }
        //Saves the ghost position, current sceme and player position
        pos = transform.position;
        CurrentScene = SceneManager.GetActiveScene();
        playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //Flips the ghost so that he will always look on the player direction
        if (playerPos.x > pos.x && side)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            side = false;
        }
        if (playerPos.x < pos.x && !side)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            side = true;
        }
        //If in Armory room
        if (CurrentScene.name == "Armory")
        {
            //Check at which floor the ghost, if player not in the same floor as the ghost, exits
            float[] heights = GameObject.Find("Armory").GetComponent<RoomScript>().heights;
            int playerFloor = GameObject.Find("Armory").GetComponent<RoomScript>().GetFloor(playerPos.y);
            int ghostFloor = 10;
            if (pos.y >= heights[2]) ghostFloor = 2;
            else if (pos.y >= heights[1]) ghostFloor = 1;
            else if (pos.y >= heights[0]) ghostFloor = 0;
            if (playerFloor != ghostFloor) return;

        }
        //Moves the ghost towards the player
        transform.position = Vector3.MoveTowards(pos, playerPos, 2f * Time.deltaTime);
        //Saves the player width and height
        float width = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the sides of the ghost
        float GhostLeft = pos.x - width / 2;
        float GhostRight = pos.x + width / 2;
        float GhostDown = pos.y - height / 2;
        float GhostUp = pos.y + height / 2;
        //Saves the down and up sides of the player
        float playerFeet = playerPos.y - playerHeight / 2;
        float playerHead = playerPos.y + playerHeight / 2;
        //Saves the player animation component
        Animator playerAnim = GameObject.Find("DuncanJr").GetComponent<Animator>();
        //If the player isn't shooting, reset the next shoot
        if (!playerAnim.GetBool("Shoot"))
            nextShoot = true;
        //If the player is shooting and the shot hasn't being counted yet
        if (playerAnim.GetBool("Shoot") && nextShoot)
        {
            //Turning the flag that the shot has been counted
            nextShoot = false;
            //If the player shot hit the ghost
            if (((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x + 6.095 > GhostLeft && playerPos.x < GhostRight) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x - 6.095 < GhostRight && playerPos.x > GhostLeft)) && playerPos.y + 0.2f > GhostDown + 0.3f && playerPos.y + 0.2f < GhostUp - 0.3f)
            {
                //If the player has took the Thrones item, minus 2 lifes, else minus 1 life 
                life -= GameObject.Find("DuncanJr").GetComponent<DuncanControl>().Damage;
                //Plays the audio clip for a hit
                audioSource.clip = Resources.Load<AudioClip>("Audio/Hit");
                audioSource.Play();
            }
        }
        //If the lifes reaches 0, goes to Death function
        if (life <= 0)
        {
            Death();
            return;
        }
        //If the boss is attacking and it hit the player and the player is not using the orb, goes to death screen
        if (!(playerPos.x > GhostLeft && playerPos.x < GhostRight && ((playerFeet < GhostUp - 0.8f && playerFeet > GhostDown - 0.2f) || (playerHead - 0.1f > GhostUp - 0.2f && playerFeet < GhostDown + 0.2f) || (playerHead - 0.2f < pos.y + 0.3 && playerHead - 0.2f > pos.y - 0.25f))))
            return;
        if (GameObject.Find("DuncanJr").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Orbing")
        {
            if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
            if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
            {
                GameObject.Find("GSD").GetComponent<GSDScript>().life--;
            }
        }
    }


    void Death()
    {
        //Sets the animator death flag
        anim.SetBool("Death", true);
        //Saves the time the death animation will end
        deathTime = Time.time + 1/3f;
        //Adds points to the score and to the Kills
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Score += 50;
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Kills += 1;
    }
}
