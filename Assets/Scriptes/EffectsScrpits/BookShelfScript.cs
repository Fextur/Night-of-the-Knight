using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelfScript : MonoBehaviour
{
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    Vector3 pos5;
    Vector3 pos6;
    float time1;
    float time2;
    float time3;
    float time4;
    float time5;
    float time6;

    // Use this for initialization
    void Start ()
    {
        time1 = Time.time + 4f;
        time2 = Time.time + 1f;
        time3 = Time.time + 2f;
        time4 = Time.time + 3f;
        time5 = Time.time + 1f;
        time6 = Time.time + 2f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time >= time1)
        {
            time1 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(-9.08f, -7.18f), Random.Range(0.03f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
        if (Time.time >= time2)
        {
            time2 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(-2.98f, -2.23f), Random.Range(0.03f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
        if (Time.time >= time3)
        {
            time3 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(-0.929f, 1.35f), Random.Range(1.18f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
        if (Time.time >= time4)
        {
            time4 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(0.05f, 2.92f), Random.Range(1.18f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
        if (Time.time >= time5)
        {
            time5 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(4.11f, 6.43f), Random.Range(0.86f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
        if (Time.time >= time6)
        {
            time6 = Time.time + Random.Range(1f, 3f);
            string sprite = "OriginalBook" + Random.Range(1, 6);
            Vector3 pos = new Vector3(Random.Range(6.7f, 8.78f), Random.Range(2.37f, 3.565f));
            Instantiate(GameObject.Find(sprite), pos, Quaternion.identity);
        }
    }
}
