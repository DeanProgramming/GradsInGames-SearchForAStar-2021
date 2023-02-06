using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private int positionOnTutorial = 0;
    public GameObject capturePoint;
    public GameObject waitPoint;
    private Game _game; 

    private void Start()
    {
        capturePoint.GetComponent<CapturePoint>().ActivatePoint(false);
        waitPoint.GetComponent<WaitPoint>().ActivatePoint(false);
        _game = FindObjectOfType<Game>();
    }

    void Update()
    {
        if (_game.OutputIsIdle())
        {
            if (positionOnTutorial == 0 && Input.GetKeyDown(KeyCode.A))
            {
                positionOnTutorial += 1;
                FindObjectOfType<Game>().AchievedGoal();
            }
            else if (positionOnTutorial == 1)
            {
                capturePoint.GetComponent<CapturePoint>().ActivatePoint(true);
            }
            else if (positionOnTutorial == 2)
            {
                waitPoint.GetComponent<WaitPoint>().ActivatePoint(true);
            }
        }
    }

    public void ActivateNextTask()
    {
        positionOnTutorial += 1;
        FindObjectOfType<Game>().AchievedGoal();
    }
}
