using UnityEngine;
using UnityEngine.SceneManagement;

//RatScript - Script for the rats
public class RatScript : MonoBehaviour
{
    //Saves the side the boss is pointing to (1 = left, -1 = right)
    bool side = false;
    //Saves the renderer component and audio component
    public SpriteRenderer sprite;
    public AudioSource audioSource;
    //Saves the rat position
    Vector3 pos;
    //Saves the player height
    float playerHeight;
    //saves the current time
    float currentTime;
    //Saves the time the rat died
    float deathTime = 0;

    //Called in initialization
    void Start ()
    {
        //Saves the renderer component and audio component
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        //Plays the rat audio clip, turn on looping audio
        audioSource.clip = Resources.Load<AudioClip>("Audio/RatSounds/Squicks");
        audioSource.loop = true;
        audioSource.Play();
        //Saves the player height
        playerHeight = GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.y;
        //Flips the rat
        Vector3 theScale = transform.localScale;
        if(theScale.x > 0)
            theScale.x *= -1;
        transform.localScale = theScale;
        //Goes to Show function
        Show();
        //Adds to the rat count
        GameObject.Find("RatLair").GetComponent<RatLairScript>().ratCount++;
    }
	
	//Called once per frame
	void Update ()
    {
        //Saves the current time
        currentTime = Time.time;
        //If the rat is in death animation
        if (GetComponent<Animator>().GetBool("Death"))
        {
            //If death animation finished, hides the rat
            if(deathTime != 0 && currentTime - deathTime >= 1f/3f)
                sprite.sortingLayerName = "BTS";
            return;
        }
        //Saves the rat position
        pos = transform.position;
        //Moves the rat according to the sides he's faceing
        if (side) pos.x += 8f * Time.deltaTime;
        else pos.x -= 8f * Time.deltaTime;
        //If got to the sides of the screen, flips the rat
        if (pos.x > 8.5f)
        {
            side = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            return;
        }
        if (pos.x < -8.5f)
        {
            side = true;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            return;
        }
        //Updates the rat position
        transform.position = pos;
        //Saves the width and height of the player
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the position of the player
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //Saves the sides of the rat
        float RatLeft = pos.x - width / 2;
        float RatRight = pos.x + width / 2;
        float RatDown = pos.y - height / 2;
        float RatUp = pos.y + height / 2;
        //Saves the down and up sides of the player
        float playerFeet = playerPos.y - playerHeight / 2;
        float playerHead = playerPos.y + playerHeight / 2;
        //Saves the player animation component
        Animator playerAnim = GameObject.Find("DuncanJr").GetComponent<Animator>();
        //If the player is shooting and shoots touch the ratm goes to Death function
        if (playerAnim.GetBool("Crouch") && playerAnim.GetBool("Shoot"))
        {
            if (!((playerFeet < RatUp - 0.4f && playerFeet > RatDown - 0.2f) || (playerPos.y > RatUp - 0.1f && playerFeet < RatDown + 0.1f) || (playerPos.y < RatUp - 0.1f && playerPos.y > RatDown + 0.1f)))
                return;
            if (playerPos.x  > RatLeft && playerPos.x < RatRight)
            {
                if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
                {
                    GameObject.Find("GSD").GetComponent<GSDScript>().life--;
                }
            }
            else if((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x + GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.x/2 > RatLeft && playerPos.x < RatRight) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x - GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.x / 2 < RatRight && playerPos.x > RatLeft))
            {
                Death();
            }
        }
        else if (playerAnim.GetBool("Shoot"))
        {
            if (((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x + GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.x / 2 > RatLeft && playerPos.x < RatRight) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && playerPos.x - GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.x / 2 < RatRight && playerPos.x > RatLeft)) && playerPos.y + 0.2f > RatDown + 0.3f && playerPos.y + 0.2f < RatUp - 0.3f)
            {
                Death();
            }
        }
        else if (playerAnim.GetBool("Crouch"))
        {
            if (playerPos.x > RatLeft && playerPos.x < RatRight && ((playerFeet < RatUp - 0.4f && playerFeet > RatDown - 0.2f) || (playerPos.y > RatUp - 0.1f && playerFeet < RatDown + 0.1f) || (playerPos.y< RatUp - 0.1f && playerPos.y> RatDown + 0.1f)))
            {
                if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
                {
                    GameObject.Find("GSD").GetComponent<GSDScript>().life--;
                }
            }
            else return;
        }
        //If the rat didn't hit the player, exits
        if (!(playerPos.x > RatLeft && playerPos.x < RatRight && ((playerFeet < RatUp - 0.4f && playerFeet > RatDown - 0.2f) || (playerHead - 0.1f > RatUp - 0.2f && playerFeet < RatDown + 0.2f) || (playerHead - 0.2f < pos.y+0.3 && playerHead - 0.2f > pos.y - 0.25f))))
            return;
        //If the player isn't using the orb, goes to death screen
        if (GameObject.Find("DuncanJr").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Orbing")
        {
            if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
            if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
            {
                GameObject.Find("GSD").GetComponent<GSDScript>().life--;
            }
        }
    }

    //Shows the rat
    void Show()
    {
        sprite.sortingLayerName = "Creatures";
    }

    //Death function (when the rat dies)
    void Death()
    {
        //Plays the hit sound and stop the audio from looping 
        audioSource.clip = Resources.Load<AudioClip>("Audio/Hit");
        audioSource.Play();
        audioSource.loop = false;
        //Sets the animator death flag
        GetComponent<Animator>().SetBool("Death", true);
        //Down the rat count
        GameObject.Find("RatLair").GetComponent<RatLairScript>().ratCount--;
        //Adds points to the score and to the Kills
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Score += 10;
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Kills += 1;
        //Saves the death time
        deathTime = Time.time;
    }
    

}
