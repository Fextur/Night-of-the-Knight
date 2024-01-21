using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookScript : MonoBehaviour
{
    Animator anim;
    float playerHeight;
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        playerHeight = GameObject.Find("DuncanJr").GetComponent<SpriteRenderer>().bounds.size.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (name.Length <15 && name.Length > 5) return;
        Vector3 pos = transform.position;
        if (pos.y <= -4.5)
        {
            anim.SetBool("End", true);
            return;
        }
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        float BookLeft = pos.x - width / 2;
        float BookRight = pos.x + width / 2;
        float BookDown = pos.y - height / 2;
        float BookUp = pos.y + height / 2;
        Vector3 playerPos = GameObject.Find("DuncanJr").GetComponent<Transform>().position;
        float playerHead = playerPos.y + playerHeight / 2;

        if (playerPos.x <= BookRight && playerPos.x >= BookLeft && playerHead >= pos.y && GameObject.Find("DuncanJr").GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name != "Orbing")
        {
            if (!GameObject.Find("GSD")) SceneManager.LoadScene("DeathScreen");
            if (GameObject.Find("GSD").GetComponent<GSDScript>().ImmunityTime < Time.time)
            {
                GameObject.Find("GSD").GetComponent<GSDScript>().life--;
            }
        }
        pos.y -= 5f * Time.deltaTime;
        transform.position = pos;
	}
}
