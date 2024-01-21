using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ItemScript - Script for all the items
public class ItemScript : MonoBehaviour
{
    //Saves the renderer and audio component
    public SpriteRenderer sprite;
    public AudioSource audioSource;

    //Calld in initialization
    void Start ()
    {
        //Saves the renderer and audio component
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //Called once per frame
    void Update()
    {
        //If the object has already been taken, exit
        if (sprite.sortingLayerName == "BTS")
            return;
        //If already taken, goes to Hide function
        if (GameObject.Find("GSD"))
        {
            switch (name)
            {
                case "Sugar":
                    if (GameObject.Find("GSD").GetComponent<GSDScript>().Sugar) Hide();
                    break;
                case "Mushroom":
                    if (GameObject.Find("GSD").GetComponent<GSDScript>().Mushroom) Hide();
                    break;
                case "Orb":
                    if (GameObject.Find("GSD").GetComponent<GSDScript>().Orb) Hide();
                    break;
                case "Thrones":
                    if (GameObject.Find("GSD").GetComponent<GSDScript>().Thrones == 1) Hide();
                    break;
                case "Book":
                    if (GameObject.Find("GSD").GetComponent<GSDScript>().Book == 1) Hide();
                    break;
            }
        }
        //Saves the position of the item
        Vector3 pos = transform.position;
        //Saves the lengths of the item
        float width = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the position of the player
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //Calcluates the sides of the item
        float ItemLeft = pos.x - width / 2;
        float ItemRight = pos.x + width / 2;
        float ItemDown = pos.y - height / 2;
        float ItemUp = pos.y + height / 2;
        //Check if player is in the item range and input E
        if (playerPos.x > ItemLeft && playerPos.x < ItemRight && playerPos.y < ItemUp && playerPos.y + 1.5f> ItemDown  && Input.GetKey(KeyCode.E))
        {
            //Hides the item and adds points to the score, and plays the audio clip
            Hide();
            audioSource.Play();
            //Turn on the flags in the GSD (if can't find GSD, turn on the effects of the items)
            switch (name)
            {
                case "Sugar":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Sugar = true;
                    else GameObject.Find("DuncanJr").GetComponent<DuncanControl>().Sugar = 2f;
                    //When sugar is taken, change the rat limit to 3
                    GameObject.Find("RatLair").GetComponent<RatLairScript>().limit = 3f;
                    break;
                case "Mushroom":
                    if(GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Mushroom = true;
                    break;
                case "Thorns":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Thrones = 1;
                    else GameObject.Find("DuncanJr").GetComponent<Animator>().SetBool("Spike", true);
                    break;
                case "Book":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Book = 1;
                    else GameObject.Find("DuncanJr").GetComponent<Animator>().SetBool("Book", true);
                    break;
                case "Orb":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Orb = true;
                    else GameObject.Find("DuncanJr").GetComponent<DuncanControl>().Orb = true;
                    break;
            }
        }
    }

    //Hides the object and adds points to the score
    void Hide()
    {
        sprite.sortingLayerName = "BTS";
        if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().Score += 100;
    }
}
