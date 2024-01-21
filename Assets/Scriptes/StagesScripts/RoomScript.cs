using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//RoomScript - Script for every room
public class RoomScript : MonoBehaviour
{
    //Saves the current floor
    public int floor;
    //Saves the direction of the stairs (1 - up from left to right or down from right to left, -1 - up from right to left or down from left to right)
    float direction = 1f;
    //Saves the height of each floor
    public float[] heights;
    //Saves the limits of each floor
    Vector3 limits;
    //Saves the x of the start and end of each staircase
    float[] upStairs1 = new float[2];
    float[] downStairs1 = null;
    float[] upStairs2 = new float[2];
    float[] downStairs2 = new float[2];
    float[] upStairs3 = new float[2];
    float[] downStairs3 = new float[2];
    float[] upStairs4 = new float[2];
    float[] downStairs4 = new float[2];
    float[] upStairs5 = null;
    float[] downStairs5 = new float[2];
    //Saves the stairs upward or downward
    float[] upStairs;
    float[] downStairs;
    //Saves the current scene
    Scene CurrentScene;

    //Called in initialization
    void Start()
    {

        if(GameObject.Find("Life") && GameObject.Find("GSD")) GameObject.Find("Life").GetComponent<Animator>().SetInteger("Life", GameObject.Find("GSD").GetComponent<GSDScript>().life);
        //Saves the current scene
        Scene CurrentScene = SceneManager.GetActiveScene();
        //going over all room posibilities, if it is HallRoom, Kitchen or Aromry updates the stairs location, if it's Garden or ThroneRoom updates limits
        switch (CurrentScene.name)
        {
            case "HallRoom":
                floor = 0;
                heights = new float[5] { -3.5f, -1.4f, 0.2f, 1.0f, 2.6f};
                //Update player position to the last position at which he was at HallRoom
                if (GameObject.Find("GSD") && GameObject.Find("GSD").GetComponent<GSDScript>().PlayerPosHall.y != 0)
                {
                    GameObject.Find("DuncanJr").GetComponent<Transform>().position = GameObject.Find("GSD").GetComponent<GSDScript>().PlayerPosHall;
                    floor = GetFloor(GameObject.Find("GSD").GetComponent<GSDScript>().PlayerPosHall.y);
                }
                upStairs1[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x - GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs1[1] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x + GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs2[0] = GameObject.Find("Stairs3").GetComponent<Transform>().position.x + GameObject.Find("Stairs3").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs2[1] = GameObject.Find("Stairs4").GetComponent<Transform>().position.x - GameObject.Find("Stairs4").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs3[0] = GameObject.Find("Stairs5").GetComponent<Transform>().position.x - GameObject.Find("Stairs5").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs3[1] = GameObject.Find("Stairs6").GetComponent<Transform>().position.x + GameObject.Find("Stairs6").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs4[0] = GameObject.Find("Stairs7").GetComponent<Transform>().position.x + GameObject.Find("Stairs7").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs4[1] = GameObject.Find("Stairs8").GetComponent<Transform>().position.x - GameObject.Find("Stairs8").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs2[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x + GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs2[1] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x - GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs3[0] = GameObject.Find("Stairs3").GetComponent<Transform>().position.x - GameObject.Find("Stairs3").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs3[1] = GameObject.Find("Stairs4").GetComponent<Transform>().position.x + GameObject.Find("Stairs4").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs4[0] = GameObject.Find("Stairs5").GetComponent<Transform>().position.x + GameObject.Find("Stairs5").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs4[1] = GameObject.Find("Stairs6").GetComponent<Transform>().position.x - GameObject.Find("Stairs6").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs5[0] = GameObject.Find("Stairs7").GetComponent<Transform>().position.x - GameObject.Find("Stairs7").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs5[1] = GameObject.Find("Stairs8").GetComponent<Transform>().position.x + GameObject.Find("Stairs8").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                break;
            case "Armory":
                floor = 0;
                //heights 4 and 5 are an unreachable number because they don't exist in this room
                heights = new float[5] { -3.55f, -0.3f, 2.9f , 1000f, 1000f};
                upStairs1[1] = upStairs1[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x - GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs2[1] = upStairs2[0] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x + GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs3 = null;
                downStairs2[1] = downStairs2[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x + GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs3[1] = downStairs3[0] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x - GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                break;
            case "Kitchen":
                floor = 2;
                //heights 4 and 5 are an unreachable number because they don't exist in this room
                heights = new float[5] { -3.45f, 0.55f, 3.25f, 1000f, 1000f};
                upStairs1[1] = upStairs1[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x + GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                upStairs2[1] = upStairs2[0] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x - GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs1 = null;
                downStairs2[1] = downStairs2[0] = GameObject.Find("Stairs1").GetComponent<Transform>().position.x - GameObject.Find("Stairs1").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                downStairs3[1] = downStairs3[0] = GameObject.Find("Stairs2").GetComponent<Transform>().position.x + GameObject.Find("Stairs2").GetComponent<SpriteRenderer>().bounds.size.x / 2;
                break;
            case "Garden":
                limits = new Vector3(-50f, 50.0f, -1.85f);
                break;
            case "ThroneRoom":
                limits = new Vector3(-9.3f, 9.3f, -3.49f);
                break;
            case "Library":
                limits = new Vector3(-9.3f, 9.3f, -3.49f);
                break;

        }

    }

    //Called once per frame
    void Update()
    {
        //Updates the current scene and player position
        CurrentScene = SceneManager.GetActiveScene();
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //If in ThroneRoom, exits
        if (CurrentScene.name == "ThroneRoom" || CurrentScene.name == "Library") return;
        //If in Garden, check if reached the limit (and if so changes to HallRoom or VictoryScreen) and then exits
        if (CurrentScene.name == "Garden")
        {
            if (playerPos.x >= 6.74f)
            {
                if (Input.GetKey(KeyCode.D))
                    SceneManager.LoadScene("HallRoom");
            }
            if (playerPos.x <= -9.5f)
            {
                SceneManager.LoadScene("VictoryScreen");
            }
            return;
        }
        //Updates floor
        floor = GetFloor(playerPos.y);
        //Updates upstairs and downstairs based on the floor
        switch (floor)
        {
            case 0:
                upStairs = upStairs1;
                downStairs = downStairs1;
                break;
            case 1:
                upStairs = upStairs2;
                downStairs = downStairs2;
                break;
            case 2:
                upStairs = upStairs3;
                downStairs = downStairs3;
                break;
            case 3:
                upStairs = upStairs4;
                downStairs = downStairs4;
                break;
            case 4:
                upStairs = upStairs5;
                downStairs = downStairs5;
                break;
            default:
                upStairs = downStairs = null;
                break;
        }
    }

    //Return the floor of the height given
    public int GetFloor(float currentHeight)
    {
        //checks to what height of floor he is close to (if didn't find then he is on stairs, so floor = -1)
        if (currentHeight < heights[0] + 0.2f) return 0;
        else if (currentHeight < heights[1] + 0.2f && currentHeight > heights[1] - 0.2f) return 1;
        else if (currentHeight < heights[2] + 0.2f && currentHeight > heights[2] - 0.2f) return 2;
        else if (currentHeight < heights[3] + 0.2f && currentHeight > heights[3] - 0.2f) return 3;
        else if (currentHeight < heights[4] + 0.2f && currentHeight > heights[4] - 0.2f) return 4;
        else return -1;
    }

    //Returns the direction of the stairs (1 - up from left to right or down from right to left, -1 - up from right to left or down from left to right), and updates playerLimit and stairsStatus accordingly
    public float GetDuncanStatus(Vector3 pos, ref string stairsStatus, ref Vector3 playerLimits)
    {
        //If the player is in Garden or ThroneRoom,updates the limits and returns 1 (no stairs at those rooms)
        if (CurrentScene.name == "Garden" || CurrentScene.name == "ThroneRoom" || CurrentScene.name == "Library")
        {
            playerLimits = limits;
            return 1f;
        }
        //Updates the limits 
        playerLimits = GetLimits();
        //If the player is on stairs
        if (floor != -1)
        {
            //Resets stairStatus
            stairsStatus = "none";
            //If the player going upstairs
            if (Input.GetKey(KeyCode.W) && upStairs != null)
            {
                //Going over all stairs leading up
                for (int i = 0; i < upStairs.Length; i++)
                {
                    //If in those stairs range
                    if (pos.x > (upStairs[i] - 0.7f) && pos.x < (upStairs[i] + 0.7f))
                    {
                        //Updates stairStatus and direction
                        stairsStatus = GetUpStairName(i);
                        direction = GetUpDownDirection(stairsStatus);
                        //if the direction and input doesn't fit to going up, then reset StairStatus
                        if (direction > 0 && Input.GetKey(KeyCode.A)) stairsStatus = "none";
                        if (direction < 0 && Input.GetKey(KeyCode.D)) stairsStatus = "none";
                        return direction;
                    }
                }
            }
            //If the player going upstairs
            if (Input.GetKey(KeyCode.S) && downStairs != null)
            {
                //Going over all stairs leading down
                for (int i = 0; i < downStairs.Length; i++)
                {
                    //If in those stairs range
                    if (pos.x > (downStairs[i] - 0.7f) && pos.x < (downStairs[i] + 0.7f))
                    {
                        //Updates stairStatus and direction
                        stairsStatus = GetDownStairName(i);
                        direction = GetUpDownDirection(stairsStatus);
                        //if the direction and input doesn't fit to going down, then reset StairStatus
                        if (direction > 0 && Input.GetKey(KeyCode.D)) stairsStatus = "none";
                        if (direction < 0 && Input.GetKey(KeyCode.A)) stairsStatus = "none";
                        return direction;
                    }
                }
            }
        }
        return direction;
    }
    
    //Returns the limits of the current floor(x - left limit, y - right limit, z - height of the floor)
    public Vector3 GetLimits()
    {
        //Saves the player position
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        //If not on stairs, returns the height of the floor
        if (floor != -1) playerPos.z = heights[floor];
        //Updates playerPos.x to hold the left limit and y to hold the right limit (if in Hallroom than it is depends on the floor)
        if (CurrentScene.name == "HallRoom")
        {
            switch (floor)
            {
                default:
                case 0:
                    playerPos.x = -9.3f;
                    playerPos.y = 9.3f;
                    break;
                case 1:
                    playerPos.x = -3.9f;
                    playerPos.y = 3.5f;
                    break;
                case 2:
                    if (playerPos.x <= -5.65f)
                    {
                        playerPos.x = -9.4f;
                        playerPos.y = -5.7f;
                    }
                    else
                    {
                        playerPos.x = 5.8f;
                        playerPos.y = 9.3f;
                    }
                    break;
                case 3:
                    if (playerPos.x <= -3.4f)
                    {
                        playerPos.x = -6.5f;
                        playerPos.y = -3.4f;
                    }
                    else
                    {
                        playerPos.x = 3.3f;
                        playerPos.y = 6.6f;
                    }
                    break;
                case 4:
                    if (playerPos.x <= -3.9f)
                    {
                        playerPos.x = -9.4f;
                        playerPos.y = -3.9f;
                    }
                    else
                    {
                        playerPos.x = 3.8f;
                        playerPos.y = 9.3f;
                    }
                    break;
            }
        }
        else
        {
            playerPos.x = -9.3f;
            playerPos.y = 9.3f;
            if (CurrentScene.name == "Kitchen" && floor == 0) playerPos.x = -3.2f;
        }
        return playerPos;
    }

    //Finds the stairs at which the player at
    public string GetUpStairName(int index)
    {
        //If not in HallRoom
        if (CurrentScene.name != "HallRoom")
        {
            //If going up in floor 0 than stairs 1, else stairs 2
            if (floor == 0) return "Stairs1";
            return "Stairs2";
        }
        //Finds the current stairs based on the floor and index
        if (floor == 0 && index == 0) return "Stairs1";
        else if (floor == 0) return "Stairs2";
        else if (floor == 1 && index == 0) return "Stairs3";
        else if (floor == 1) return "Stairs4";
        else if (floor == 2 && index == 0) return "Stairs5";
        else if (floor == 2) return "Stairs6";
        else if (floor == 3 && index == 0) return "Stairs7";
        return "Stairs8";
    }
    
    //Finds the stairs at which the player at
    public string GetDownStairName(int index)
    {
        //If not in HallRoom
        if (CurrentScene.name != "HallRoom")
        {
            //If going up in floor 2 than stairs 2, else stairs 1
            if (floor == 2) return "Stairs2";
            return "Stairs1";
        }
        //Finds the current stairs based on the floor and index
        if (floor == 1 && index == 0) return "Stairs1";
        else if (floor == 1) return "Stairs2";
        else if (floor == 2 && index == 0) return "Stairs3";
        else if (floor == 2) return "Stairs4";
        else if (floor == 3 && index == 0) return "Stairs5";
        else if (floor == 3) return "Stairs6";
        else if (floor == 4 && index == 0) return "Stairs7";
        return "Stairs8";
    }

    //Returns the direction of the stairs (1 - up from left to right or down from right to left, -1 - up from right to left or down from left to right)
    public float GetUpDownDirection(string stair)
    {
        //If in the kitchen
        if (CurrentScene.name == "Kitchen")
        {
            //if on stairs1 return -1f else 1f
            if (stair == "Stairs1") return -1f;
            else return 1f;
        }
        //Finds the direction of the stairs (stairs 2,3,6,7 - up from right to left or down from left to right, stairs 1,4,5,8 - up from left to right or down from right to left)
        switch (stair)
        {
            case "Stairs2":
            case "Stairs3":
            case "Stairs6":
            case "Stairs7":
                return -1f;
            case "Stairs1":
            case "Stairs4":
            case "Stairs5":
            case "Stairs8":
            default:
                return 1f;
        }
    }
}
