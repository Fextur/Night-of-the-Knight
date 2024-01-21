using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//BryantScript - Script for the boss
public class BryantScript : MonoBehaviour
{
    //Saves the side the boss is pointing to (1 = left, -1 = right)
    public int side = 1;
    //Saves the renderer component, animation component, audio component
    Animator anim;
    public AudioSource audioSource;
    public SpriteRenderer sprite;
    //Saves the time which the boss stood, the time he will start pre attack and attack
    float standTime = 0;
    float preAttackTime = -1f;
    float attackTime = -1f;
    //Saves the Stage the boss at
    int Stage = 0;
    //Saves the boss current position, and the left and right positions
    Vector3 pos;
    Vector3 leftPos;
    Vector3 rightPos;
    //Saves the boss lifes
    int life = 60;
    //Flag for if the current shoot that the boss recieved has already being counted
    bool nextShoot = true;
    //Flag for when the boss got to his position
    bool justStopped = true;
    //Saves the time the boss death animation ends
    float deathTime = 0;
    //Saves the player height
    float playerHeight;

    //Called in initialization
    void Start()
    {
        //Saves the renderer component, animation component, audio component
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //Saves the left and right positions
        leftPos.Set(-8.5f, -3.124857f,0);
        rightPos.Set(8.5f, -3.124857f,0);
        //Saves the player height
        playerHeight = GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.y;
    }

    //Called once per frame
    void Update()
    {
        Debug.Log(life);
        //If the boss has already died, hides him and exists
        if (GameObject.Find("GSD") && !GameObject.Find("GSD").GetComponent<GSDScript>().Bryant)
        {
            sprite.sortingLayerName = "BTS";
            return;
        }
        //If the boss has died
        if (deathTime != 0)
        {
            //if the death animation ends, hides him and turn his flag in GSD
            if (deathTime <= Time.time)
            {
                sprite.sortingLayerName = "BTS";
                GameObject.Find("GSD").GetComponent<GSDScript>().Bryant = false;
            }
            return;
        }
        //If the lifes reaches 0, goes to Death function
        if (life <= 0)
            Death();
        //If time for pre attack came and animator flag for pre attack is false
        if (Time.time >= preAttackTime && preAttackTime != -1f && !anim.GetBool("PreAttack"))
        {
            //Turns true the pre attack flag at the animator and starts the corresponding audio clip
            anim.SetBool("PreAttack", true);
            audioSource.clip = Resources.Load<AudioClip>("Audio/BryantSounds/SwordDrawn");
            audioSource.Play();
        }
        //If time for attack came and animator flag for attack is false
        if (Time.time >= attackTime && attackTime != -1f && !anim.GetBool("Attack"))
        {
            //Turns true the pre attack flag at the animator and starts the corresponding audio clip
            audioSource.clip = Resources.Load<AudioClip>("Audio/BryantSounds/SwordSlash");
            audioSource.Play();
            justStopped = true;
            anim.SetBool("Attack", true);
        }
        //If there is less than 5 life and the Shield flag at the animator is true
        if (life < 5 && anim.GetBool("Shield"))
        {
            //Turns false the shield flag at the animator and starts the corresponding audio clip
            anim.SetBool("Shield", false);
            audioSource.clip = Resources.Load<AudioClip>("Audio/BryantSounds/SheildBreak");
            audioSource.Play();
        }
        //Saves boss position
        pos = transform.position;
        //If stage = 0, goes to Stage0 function, else to Cycle function
        if (Stage == 0) Stage0();
        else Cycle();
    }

    //Stage 0 function (before the boss started to attack)
    void Stage0()
    {
        //If both ghosts die, sets the stand flag in the animator true, else exits
        if (GameObject.Find("Ghost1").GetComponent<SpriteRenderer>().sortingLayerName == "BTS" && GameObject.Find("Ghost2").GetComponent<SpriteRenderer>().sortingLayerName == "BTS")
            anim.SetBool("Stand", true);
        else
            return;
        //If in the stand animation and just started standing
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Stand" && standTime == 0)
        {
            //Updates the time the boss started to stand, and starts the corresponding audio sound
            standTime = Time.time;
            audioSource.clip = Resources.Load<AudioClip>("Audio/BryantSounds/Teleport2");
            audioSource.Play();
        }
        //If the boss has standed for 1 sec
        if (standTime != 0 && Time.time - standTime >= 1f)
        {
            //Moves the boss downstairs and starts stage 1 and updates the time for the pre attack and attack
            pos.y = -3.124857f;
            transform.position = pos;
            Stage = 1;
            preAttackTime = Time.time + 2f;
            attackTime = Time.time + 4f;
        }
    }

    //The movement cycle function (cycles between stand, pre attack, attack and stand again)
    void Cycle()
    {
        //If in the right position with the corresponding side
        if ((pos == rightPos && side == -1) || (pos == leftPos && side == 1))
        {
            //If the boss just got to the position
            if (justStopped)
            {
                //Sets the pre attack and attack flags in the animator
                anim.SetBool("PreAttack", false);
                anim.SetBool("Attack", false);
                //Turn the flag for when just stopped false and updates the time for the pre attack and attack
                justStopped = false;
                preAttackTime = Time.time + 4f;
                attackTime = Time.time + 4.5f;
            }
            //flips the boss
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            side = side * -1;
        }
        //If the boss in the Attack animation
        else if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack")
        {
            //Goes to the position that at the edge of the side the boss looks too
            if (side == -1)
                transform.position = Vector3.MoveTowards(pos, rightPos, 75f * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(pos, leftPos, 75f * Time.deltaTime);
            
        }
        //Saves the width and height of the player
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the position of the player
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //Saves the sides of the boss
        float BryantLeft = pos.x - width / 2;
        float BryantRight = pos.x + width / 2;
        float BryantDown = pos.y - height / 2;
        float BryantUp = pos.y + height / 2;
        //Saves the down side of the player
        float playerFeet = playerPos.y - playerHeight / 2;
        //Saves the player animator
        Animator playerAnim = GameObject.Find("DuncanJr").GetComponent<Animator>();
        //If the boss not in attack animation
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack")
        {
            //If the player isn't shooting, reset the next shoot
            if (!playerAnim.GetBool("Shoot"))
                nextShoot = true;
            //If the player is shooting and the shot hasn't being counted yet
            if (playerAnim.GetBool("Shoot") && nextShoot)
            {
                //Turning the flag that the shot has been counted
                nextShoot = false;
                //If the player shot hit the boss
                if ((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x + 5.5 > BryantLeft && playerPos.x < BryantRight) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x - 5.5 < BryantRight && playerPos.x > BryantLeft))
                {
                    life -= GameObject.Find("DuncanJr").GetComponent<DuncanControl>().Damage;
                    if (life == 4) return;
                    //Plays the audio clip for a hit
                    audioSource.clip = Resources.Load<AudioClip>("Audio/Hit");
                    audioSource.Play();
                }
            }
        }
        //If the boss is attacking and it hit the player and the player is not using the orb, goes to death screen
        else if (((side > 0 && playerPos.x < pos.x && playerPos.x > BryantLeft) || (side < 0 && playerPos.x > pos.x && playerPos.x < BryantRight)) && playerFeet < BryantUp - 0.8f && GameObject.Find("DuncanJr").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Orbing")
        {
            if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
            if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
            {
                GameObject.Find("GSD").GetComponent<GSDScript>().life--;
            }
        }
    }

    //Death function (when the boss has died)
    void Death()
    {
        //Updates the boss poistion because of the death animation offset
        pos.y += 0.865067f;
        pos.x -= 0.39189f;
        transform.position = pos;
        //Starts the death audio clip, turns true the death flag in the animator 
        audioSource.clip = Resources.Load<AudioClip>("Audio/BryantSounds/DeathLaugh");
        audioSource.Play();
        anim.SetBool("Death", true);
        //Updates the time at which the death animation will end
        deathTime = Time.time + 1f;
        //Adds 1000 points to the score and adds 1 kill
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Score += 1000;
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Kills += 1;
    }
}
