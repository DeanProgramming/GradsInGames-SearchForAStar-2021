using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyCube : MonoBehaviour
{
    private float yChange; 
    private bool direction;
    public float range = 5;
    public float startingY;
     
    /*
    void Start()
    {
        yChange = Random.Range(-range, range);
        transform.position = new Vector3(transform.position.x, startingY + yChange, transform.position.z);
    }*/

    void Update()
    {
        if (yChange > 0)
        {
            yChange -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x, startingY + yChange, transform.position.z); 
        }

        /*
        if (direction == false)
        {
            yChange += Time.deltaTime;
        }
        else
        {
            yChange -= Time.deltaTime;
        }

        if (yChange >= range && direction == false)
        {
            direction = true;
        }

        if (yChange <= -range && direction == true)
        {
            direction = false;
        }

        transform.position = new Vector3(transform.position.x, startingY + yChange, transform.position.z);
        */
    } 

    public void Pulse()
    {
        transform.position = new Vector3(transform.position.x, startingY + 10, transform.position.z);
        yChange = 10; 
    }
}
