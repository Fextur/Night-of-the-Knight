using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloserScript : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sprite;
    // Use this for initialization
    void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.Find("GSD").GetComponent<GSDScript>().doorCloserTime != 0 && GameObject.Find("GSD").GetComponent<GSDScript>().doorCloserTime < Time.time)
        {
            sprite.sortingLayerName = "BTS";
            return;
        }
        if (anim.GetBool("Burnt")) return;
        if (GameObject.Find("DuncanJr").GetComponent<Transform>().position.x > -3.9f) return;
        if (GameObject.Find("DuncanJr").GetComponent<Animator>().GetBool("Book") && GameObject.Find("HallRoom").GetComponent<RoomScript>().floor == 4)
        {
            Debug.Log("aye1");
            if (((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && GameObject.Find("DuncanJr").GetComponent<Transform>().position.x <= transform.position.x) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && GameObject.Find("DuncanJr").GetComponent<Transform>().position.x >= transform.position.x)) && GameObject.Find("DuncanJr").GetComponent<Animator>().GetBool("Shoot"))
            {
                Debug.Log("aye2");
                anim.SetBool("Burnt", true);
                if (GameObject.Find("GSD")) GameObject.Find("GSD").GetComponent<GSDScript>().doorCloserTime = Time.time + 1f;
            }
        }
        Debug.Log((GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && GameObject.Find("DuncanJr").GetComponent<Transform>().position.x <= transform.position.x) || (!GameObject.Find("DuncanJr").GetComponent<DuncanControl>().side && GameObject.Find("DuncanJr").GetComponent<Transform>().position.x >= transform.position.x));
	}
}
