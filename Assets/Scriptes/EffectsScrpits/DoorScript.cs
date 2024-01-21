using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Door Script - The script for all the doors
public class DoorScript : MonoBehaviour
{
    //Saves the renderer component
    public SpriteRenderer sprite;
    //Saves the position of the door
    Vector3 pos;
    //Saves the current scene
    Scene CurrentScene;

    //Called in initialization
    void Start()
    {
        //Saves the position of the door
        pos = transform.position;
        //Saves the renderer component
        sprite = GetComponent<SpriteRenderer>();
        //goes to Check function
        if (GameObject.Find("GSD")) Check();
        else Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "ArmoryDoor" && !GameObject.Find("DoorCloser").GetComponent<Animator>().GetBool("Burnt")) return;
        //Saves the current scene
        CurrentScene = SceneManager.GetActiveScene();
        //Saves the width and height of the door
        float width = GetComponent<SpriteRenderer>().bounds.size.x /2;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        //Saves the position of the player
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //Saves the door sides
        float DoorLeft = pos.x - width/2;
        float DoorRight = pos.x + width / 2;
        float DoorDown = pos.y - height / 2;
        float DoorUp = pos.y + height / 2;
        //If it's the catacombs door and it already has been open, exits
        if ((name == "CatacombsDoor" && GameObject.Find("Mausoleum") && GameObject.Find("Mausoleum").GetComponent<SpriteRenderer>().sortingLayerName == "BackEffects")) return;
        //If the player is in the range of the door and press "E" then show the open door (exept catacombs door), saves it in the GSD and the player position and loads the new scene
        if (playerPos.x > DoorLeft && playerPos.x < DoorRight && playerPos.y < DoorUp && playerPos.y > DoorDown - 0.5f && Input.GetKey(KeyCode.E))
        {
            if (name != "CatacombsDoor") Show();
            switch (name)
            {
                case "ArmoryDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().ArmoryDoor = true;
                    SceneManager.LoadScene("Armory");
                    break;
                case "KitchenDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().KitchenDoor = true;
                    SceneManager.LoadScene("Kitchen");
                    break;
                case "LibraryDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().LibraryDoor = true;
                    SceneManager.LoadScene("Library");
                    break;
                case "GardenDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().GardenDoor = true;
                    SceneManager.LoadScene("Garden");
                    break;
                case "CatacombsDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().CatacombsDoor = true;
                    if (GameObject.Find("Mausoleum")) GameObject.Find("Mausoleum").GetComponent<SpriteRenderer>().sortingLayerName = "BackEffects";
                    if (GameObject.Find("Orb"))
                    {
                        if (GameObject.Find("GSD"))
                        {
                            if (GameObject.Find("GSD").GetComponent<GSDScript>().Orb)
                                GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
                            else
                                GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                        }
                        else GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                    }
                    break;
                case "KitchenHallRoomDoor":
                    SceneManager.LoadScene("HallRoom");
                    break;
                case "ThroneRoomDoor":
                    if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().ThroneRoomDoor = true;
                    SceneManager.LoadScene("ThroneRoom");
                    break;
                case "ThroneHallRoomDoor":
                    SceneManager.LoadScene("HallRoom");
                    break;
                case "AromryHallRoomDoor":
                    SceneManager.LoadScene("HallRoom");
                    break;
                case "LibraryHallRoomDoor":
                    SceneManager.LoadScene("HallRoom");
                    break;
                default:
                    break;
            }
        }

    }

    //Hides the sprite(moves it to the BTS - Behind the scene layer)
    void Hide()
    {
        sprite.sortingLayerName = "BTS";
    }

    //Shows the sprite(moves it to the Effects layer)
    void Show()
    {
        sprite.sortingLayerName = "BackEffects";
    }

    //Checks from GSD if the doors are opened or closed
    void Check()
    {
        //Check if the door has been opended, if do goes to Show function, else goes to Hide function
        switch (name)
        {
            case "ArmoryDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ArmoryDoor) Show();
                else Hide();
                break;
            case "KitchenDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().KitchenDoor) Show();
                else Hide();
                break;
            case "LibraryDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().LibraryDoor) Show();
                else Hide();
                break;
            case "LibraryHallRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().LibraryDoor) Show();
                else Hide();
                break;
            case "GardenDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().GardenDoor) Show();
                else Hide();
                break;
            case "CatacombsDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().CatacombsDoor)
                {
                    //If the orb has been taken, doesn't show it
                    if (GameObject.Find("Orb"))
                    {
                        if(GameObject.Find("GSD").GetComponent<GSDScript>().Orb) GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
                        else GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "Items";
                    }
                    if (GameObject.Find("Mausoleum")) GameObject.Find("Mausoleum").GetComponent<SpriteRenderer>().sortingLayerName = "BackEffects";
                }
                else
                {
                    if (GameObject.Find("Orb")) GameObject.Find("Orb").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
                    if (GameObject.Find("Mausoleum")) GameObject.Find("Mausoleum").GetComponent<SpriteRenderer>().sortingLayerName = "BTS";
                }
                break;
            case "KitchenHallRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().KitchenDoor) Show();
                else Hide();
                break;
            case "ThroneRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ThroneRoomDoor) Show();
                else Hide();
                break;
            case "ThroneHallRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ThroneRoomDoor) Show();
                else Hide();
                break;
            case "GardenHallRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().GardenDoor) Show();
                else Hide();
                break;
            case "ArmoryHallRoomDoor":
                if (GameObject.Find("GSD").GetComponent<GSDScript>().ArmoryDoor) Show();
                else Hide();
                break;
        }
    }
}
