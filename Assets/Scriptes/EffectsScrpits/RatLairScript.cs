using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RatLairScript - Script for the rat lair
public class RatLairScript : MonoBehaviour
{
    //Saves the last time a rat has been spawned
    float lastSpawnTime;
    //Saves the rat count and the limits
    public float ratCount = 0;
    public float limit = 2f;

    //Called in initialization
    void Start ()
    {
        //Saves the last time a rat has been spawned as initialization time
        lastSpawnTime = Time.time;
    }

    //Called once per frame
    void Update ()
    {
        //If since the last time a rat has been spawned being more than 5 sec and there is less than the rat limit
        if (Time.time - lastSpawnTime >= 5f && ratCount < limit)
        {
            //Updates the last spawn time and copy a new rat
            lastSpawnTime = Time.time;
            Instantiate(GameObject.Find("Rat"), transform.position, Quaternion.identity);
        }
    }
}
