using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float runningSpeed = 5;
    public int lives = 3;
    public Color startingColour;
    public Color endingColour;
    public bool inTutorial = false;
    public Image[] heartsArray;

    private GameManager gameManager;
     
    private float cooldown; 

    private void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        Color setColour = Color.Lerp(startingColour, endingColour, Mathf.PingPong(Time.time, 1));
        GetComponent<MeshRenderer>().material.color = setColour;

        if (Input.GetMouseButtonDown(1) && cooldown <= 0)
        {
            StartCoroutine(StopTime());
        } 

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        float horzontialInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        GetComponent<Rigidbody>().velocity = new Vector3(horzontialInput * runningSpeed, 0, verticalInput * runningSpeed);  
    }


    IEnumerator StopTime()
    {
        cooldown = 1;
        runningSpeed = 30;
        Time.timeScale = .5f;
        yield return new WaitForSeconds(.5f);

        runningSpeed = 15;
        Time.timeScale = 1;
    }

    public void TakeDamage()
    {
        lives--;

        if (lives <= 0)
        {
            gameManager.GameOver(false);
            heartsArray[0].color = new Color(0, 0, 0, heartsArray[0].color.a);
        }
        else
        {
            heartsArray[lives].color = new Color(0, 0, 0, heartsArray[lives].color.a);
        }
    }


    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.GetComponentInParent<WaitPoint>())
        {
            other.gameObject.GetComponentInParent<WaitPoint>().StartCapture(true); 
        }

        if (other.gameObject.GetComponent<CapturePoint>())
        {
            other.gameObject.GetComponent<CapturePoint>().ActivatePoint(false);
            other.gameObject.GetComponent<CapturePoint>().PlaySound();

            if (inTutorial == false)
            {
                gameManager.IncreaseDownloadTime();
            }
            else
            {
                FindObjectOfType<TutorialManager>().ActivateNextTask();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.GetComponentInParent<WaitPoint>())
        {
            other.gameObject.GetComponentInParent<WaitPoint>().StartCapture(false);
        }
    }
}
