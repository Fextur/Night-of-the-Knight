using UnityEngine;
using UnityEngine.SceneManagement;

//Duncan Control - The script for the player control
public class DuncanControl : MonoBehaviour
{
    //Saves the side the player is pointing to (true = right, false = left)
    public bool side = true;
    //Saves the renderer component, animation component, audio component
    public SpriteRenderer sprite;
    Animator anim;
    public AudioSource audioSource;
    //Saves the current time, and the time when the shooting and jumping started
    float currentTime;
    public float jumpTime = 0;
    float shootTime = 0;
    //Saves the direction of the stairs (1 - up from left to right or down from right to left, -1 - up from right to left or down from left to right)
    float updown = 1f;
    //x - Points to left limit, y - Points to right limit, in case of stairs: x - Top limit, y - Bottom limit
    Vector3 playerLimits;
    //Saves the position of the player
    Vector3 pos;
    //Saves the Y of the player when he started jumping
    float startY;
    //Saves the current scene
    Scene CurrentScene;
    //Saves the stair status (if on stairs or not)
    public string stairsStatus = "none";
    //Changes to 2 if the player took the suger from the kitchen, multiplayer of the speed
    public float Sugar = 1f;
    //Changes to true if the player took the orb from the garden (indicator for the animator)
    public bool Orb = false;
    //Saves the time in which the orb will be unuesable if use constantly
    float orbTime = 0;
    //Saves the time in which the orb will be usable again
    float orbBreakTime = 0;
    //Flag for the audio when the orb is usable again
    bool orbJustReturned = true;
    //Saves the current audio clip name
    public string currentClipName;
    public int Damage = 1;

    //Called in initialization
    void Start()
    {
        //Saves the renderer component, animation component, audio component
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //Called once per frame
    void Update()
    {
        //Saves the current position, time and scene
        pos = transform.position;
        currentTime = Time.time;
        CurrentScene = SceneManager.GetActiveScene();
        //Checks items (sugar, spikes and orb), changes sugar to 2, sets the animator bool for spike true, turn the orb bool true
        if (GameObject.Find("GSD") && !GameObject.Find("GSD").GetComponent<GSDScript>().Sugar) Sugar = 1f;
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Sugar) Sugar = 2f;
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Thrones == 0) anim.SetBool("Spike",false);
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Thrones == 1) anim.SetBool("Spike", true);
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Book == 0) anim.SetBool("Book", false);
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Book == 1) anim.SetBool("Book", true);
        if (GameObject.Find("GSD") && !GameObject.Find("GSD").GetComponent<GSDScript>().Orb) Orb = false;
        if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().Orb) Orb = true;
        if (Orb) anim.SetBool("Orb", true);
        if (GameObject.Find("GSD")) Damage = 1 + GameObject.Find("GSD").GetComponent<GSDScript>().Thrones + GameObject.Find("GSD").GetComponent<GSDScript>().Book;
        //If the orb has broken, checks if he can becone usable again (plays the corresponding audio if true)
        if (Orb && orbBreakTime < Time.time && orbJustReturned)
        {
            audioSource.clip = Resources.Load<AudioClip>("Audio/DuncanSounds/OrbUp");
            currentClipName = "OrbUp";
            audioSource.Play();
            orbJustReturned = false;
        }
        //If ain't jumping, update updown
        if (!anim.GetBool("Jump")) updown = GameObject.Find(CurrentScene.name).GetComponent<RoomScript>().GetDuncanStatus(pos, ref stairsStatus, ref playerLimits);
        //If climbing the stairs (stairStatus ain't "none", pressing w/a/s/d and not in the middle of other action), goes to function Stairs
        if (stairsStatus != "none" && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !anim.GetBool("Crouch") && !anim.GetBool("Shoot") && jumpTime == 0)
        {
            Stairs();
            return;
        }
        //If not ckimbing the stairs, turn their flag in the animator false
        anim.SetBool("UpStairs", false);
        anim.SetBool("DownStairs", false);
        //If the player pressed Space or the player started to jump and he isn't in the middle of other action, goes to function Jump
        if ((Input.GetKeyDown(KeyCode.Space) || jumpTime != 0) && shootTime == 0 && !anim.GetBool("Crouch") && !anim.GetBool("Orbing"))
            Jump();
        //If the player pressed Left Shift or the player started to shoot and he isn't in the middle of other action, goes to function Shoot
        else if ((Input.GetKeyDown(KeyCode.LeftShift) || shootTime != 0) && jumpTime == 0 && !anim.GetBool("Crouch") && !anim.GetBool("Orbing"))
            Shoot();
        //If the player press C and he isn't in the middle of other action, goes to function Crouch
        else if (Input.GetKey(KeyCode.C) && jumpTime == 0 && !anim.GetBool("Orbing"))
            Crouch();
        //If the player press A or D, and he isn't in the middle of other action, goes to function Walk
        else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !anim.GetBool("Crouch") && !anim.GetBool("Orbing"))
            Walk();
        //If the player press V and he has the orb and he isn't in the middle of other action or the player was already using the orb, goes to function Orbing
        else if ((Orb && Input.GetKey(KeyCode.V) && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" && orbBreakTime < Time.time) || anim.GetBool("Orbing"))
            Orbing();
        //Else, Idle
        else Idle();
    }

    //Stairs Function (when climbing the stairs)
    void Stairs()
    {
        //0 < up from left to right or down from right to left, 0 > -up from right to left or down from left to right
        if (updown > 0)
        {
            //if input D, going right and up
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += 4.5f * Sugar  * Time.deltaTime;
                pos.y += 2.5f * Sugar  * Time.deltaTime;
                //updates the animator UpStairs flag
                anim.SetBool("UpStairs", true);
                //flips the character if needed
                if (!side)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                    side = true;
                }
            }
            //if input A, going left and down
            else if (Input.GetKey(KeyCode.A))
            {
                pos.x -= 4.5f * Sugar * Time.deltaTime;
                pos.y -= 2.5f * Sugar * Time.deltaTime;
                //updates the animator DownStairs flag
                anim.SetBool("DownStairs", true);
                //flips the character if needed
                if (side)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                    side = false;
                }
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetBool("UpStairs", false);
                anim.SetBool("DownStairs", false);
            }
        }
        else
        {
            //if input D, going right and down
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += 5f * Sugar * Time.deltaTime;
                pos.y -= 2.5f * Sugar * Time.deltaTime;
                //updates the animator DownStairs flag
                anim.SetBool("DownStairs", true);
                //flips the character if needed
                if (!side)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                    side = true;
                }
            }
            //if input A, going left and up
            else if (Input.GetKey(KeyCode.A))
            {
                pos.x -= 5f * Sugar * Time.deltaTime;
                pos.y += 2.5f * Sugar * Time.deltaTime;
                //updates the animator UpStairs flag
                anim.SetBool("UpStairs", true);
                //flips the character if needed
                if (side)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                    side = false;
                }
            }
            else
            {
                anim.SetBool("Run", false);
                anim.SetBool("UpStairs", false);
                anim.SetBool("DownStairs", false);
            }
        }
        //updates player position
        transform.position = pos;
    }

    //Jump Function (when the player is jumping)
    void Jump()
    {
        //If the player only started to jump, saves the time it started, the Y it started in, and sets the Jump flag in the animator to true, and plays the correspoinding audio clip
        if (jumpTime == 0)
        {
            jumpTime = Time.time;
            startY = pos.y;
            anim.SetBool("Jump", true);
            audioSource.clip = Resources.Load<AudioClip>("Audio/DuncanSounds/Jump");
            currentClipName = "Jump";
            audioSource.Play();
            //If the player is in the kitchen on the stairs or first floor changes the sorting layer so he won't go through some objects in the backgrounds
            if (CurrentScene.name == "Kitchen" && (GameObject.Find("Kitchen").GetComponent<RoomScript>().floor == 0 || GameObject.Find("Kitchen").GetComponent<RoomScript>().floor == -1))
                sprite.sortingLayerName = "FrontEffects2";
        }
        //If the delta time from when the player started to jump to now is betweeen 1/9 to 1/3 sec then moves up
        if (currentTime - jumpTime >= (2f / 3f) / 6f && currentTime - jumpTime <= (((2f / 3f) * 3f) / 6f))
        {
            if (Sugar == 2)
                pos.y += 30f * Time.deltaTime;
            else pos.y += 20f * Time.deltaTime;
        }
        //If the delta time from when the player started to jump to now is betweeen 1/3 to 5/9 sec then moves down
        else if (currentTime - jumpTime >= ((2f / 3f) * 3f) / 6f && currentTime - jumpTime <= (((2f / 3f) * 5f) / 6f))
        {
            if (Sugar == 2)
                pos.y -= 30f * Time.deltaTime;
            else pos.y -= 20f * Sugar  * Time.deltaTime;
        }
        //If the player finished to jump(takes 5/9 sec), reset the time it started, change to the Y it started in, and sets the Jump flag in the animator to false
        if (currentTime - jumpTime >= (2f / 3f))
        {
            sprite.sortingLayerName = "Player";
            jumpTime = 0;
            pos.y = startY;
            anim.SetBool("Jump", false);
        }
        //Updating the position 
        transform.position = pos;

    }

    //Shoot Function (when the player shoots his yo-yo)
    void Shoot()
    {
        //If the player only started to shoot, saves the time it started and sets the Shoot flag in the animator to true, and plays the correspoinding audio clip
        if (shootTime == 0)
        {
            shootTime = Time.time;
            anim.SetBool("Shoot", true);
            audioSource.clip = Resources.Load<AudioClip>("Audio/DuncanSounds/YoYo");
            currentClipName = "YoYo";
            audioSource.Play();
        }
        //If the player finished to shoot(takes 0.65 sec), reset the time it started and sets the Shoot flag in the animator to false
        if (currentTime - shootTime >= 0.65f)
        {
            shootTime = 0;
            anim.SetBool("Shoot", false);
        }
    }

    //Crouch Function (when the player is crouching)
    void Crouch()
    {
        //Set the crouch flag in the animator true
        anim.SetBool("Crouch", true);
        //If the player pressed Left Shift or the player started to shoot,, goes to function Shoot (shooting while crouching)
        if (Input.GetKeyDown(KeyCode.LeftShift) || shootTime != 0)
            Shoot();
    }

    //Walk Function (when the player is walking)
    void Walk()
    {
        //if no audio plays, plays the walking audio clip
        if (!audioSource.isPlaying)
        {
            audioSource.clip = Resources.Load<AudioClip>("Audio/DuncanSounds/Walking31");
            currentClipName = "Walking";
            audioSource.Play();
        }
        //If the player pressed A, moves left and flips the side if needed
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= 5f * Sugar * Time.deltaTime;
            if (side)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                side = false;
            }
        }
        //If the player pressed D, moves right and flips the side if needed
        else if (Input.GetKey(KeyCode.D))
        {
            pos.x += 5f * Sugar * Time.deltaTime;
            if (!side)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                side = true;
            }
        }
        //Sets the Run flag in the animation component to true if needed
        if (!anim.GetBool("Run"))
            anim.SetBool("Run", true);
        //If player is trying to pass the limits of the room, change x to the limits (so he won't cross them)
        if (pos.x < playerLimits.x) pos.x = playerLimits.x;
        else if (pos.x > playerLimits.y) pos.x = playerLimits.y;
        pos.y = playerLimits.z;
        //Update player position
        transform.position = pos;
    }

    //Orbing Function (when the player is using his orb)
    void Orbing()
    {
        //If the player just pressed V, update the time at which the orb will be unusable if used consistantly
        if (Input.GetKeyDown(KeyCode.V))
        {
            orbTime = Time.time + 1f;
        }
        //If in the animation of using the orb and the player lifted V, change the orbing flag in animator to false and resets the orbTime
        if (anim.GetBool("Orbing") && Input.GetKeyUp(KeyCode.V))
        {
            anim.SetBool("Orbing", false);
            orbTime = 0;
            return;
        }
        //If the player used the orb consistantly until the time he will be unusable
        if (orbTime < Time.time)
        {
            //change the orbing bool in animator to false and update the time the orb will be usable again, and plays the corresponding audio clip and turns true the flag for the audio when the orb will be usable again
            orbBreakTime = Time.time + 5f;
            anim.SetBool("Orbing", false);
            audioSource.clip = Resources.Load<AudioClip>("Audio/DuncanSounds/OrbDown");
            currentClipName = "OrbDown";
            audioSource.Play();
            orbJustReturned = true;
        }
        //else, turns the orbing flag in animator to true 
        else anim.SetBool("Orbing", true);
    }
 
    // Idle Funciton (when the player does nothing)
    void Idle()
    {
        //If playing the walking audio clip, turns it off
        if (currentClipName == "Walking")
        audioSource.Stop();
        //turns of any flag in the animator if needed
        if (anim.GetBool("Run"))
            anim.SetBool("Run", false);
        if (anim.GetBool("Crouch"))
            anim.SetBool("Crouch", false);
        if (anim.GetBool("Shoot"))
            anim.SetBool("Shoot", false);
    }
}
