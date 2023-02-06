using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CapturePoint[] capturePoints;
    [SerializeField] private WaitPoint[] waitPoints;
    private int lastPoint = -1;

    private float downloadTime = 0;
    [SerializeField] private float downloadLimit;
    [SerializeField] private Slider downloadTimer; 
    [SerializeField] private GameObject gameHolder;
    [SerializeField] private GameObject healthHolder;
    [SerializeField] private AudioListener listener;
    [SerializeField] private TMP_Text textHolder;

    [SerializeField] private bool creepyMusic; 
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownTimer;
    [SerializeField] private bool startCountdown;

    private MusicControls musicController;
    [SerializeField] private AudioClip warpSound;

    [SerializeField] private bool gameEnded;
    [SerializeField] private bool gameWon;
    private bool next;
    [SerializeField] private Image fadeBlackScreen;

    [SerializeField] private GameObject loseHolder;

    [SerializeField] private GameObject winHolder;
    [SerializeField] private TMP_Text uploadingText;
    [SerializeField] private float uploadingTimer;


    void Start()
    { 
        downloadTimer.value = downloadTime;
        countdownTimer = 5;
        musicController = FindObjectOfType<MusicControls>();
    }

    private void Update()
    {
        if (startCountdown)
        {
            Countdown();
        }

        if (gameEnded)
        {
            if (gameWon)
            {
                EndGameWin();
            }
            else
            {
                EndGameLose();
            }
        }
    }

    private void Countdown()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            string timeValue = (Mathf.Round(countdownTimer * 1000.0f) / 1000.0f).ToString();
            while (timeValue.Length < 5)
            {
                timeValue += "0";
            }
            countdownText.text = "SURVIVE - " + timeValue;
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(warpSound, .25f); 
            healthHolder.gameObject.SetActive(false);
            GameOver(true);
        }
    }

    public void ActivateaCapturePoint()
    {
        int randomPoint = Random.Range(0, capturePoints.Length);
        while (randomPoint == lastPoint)
        {
            randomPoint = Random.Range(0, capturePoints.Length);
        }

        capturePoints[randomPoint].ActivatePoint(true);
        lastPoint = randomPoint;
    }

    public void IncreaseDownloadTime()
    {
        downloadTime += .05f;
        downloadTimer.value = Mathf.InverseLerp(0, downloadLimit, downloadTime);

        if(downloadTime >= downloadLimit)
        {
            startCountdown = true;
            downloadTimer.gameObject.SetActive(false);
        }
        else
        {
            ActivateNextTask();
        }
    }

    private void ActivateNextTask()
    {
        if (startCountdown == false)
        {
            if (Random.Range(0, 4) == 3)
            {
                waitPoints[Random.Range(0, waitPoints.Length)].ActivatePoint(true);
            }
            else
            {
                ActivateaCapturePoint();
            }
        }
    }

    public void BeginGame()
    { 
        gameHolder.SetActive(true);
        healthHolder.gameObject.SetActive(true);

        FindObjectOfType<MissileSpawning>().StartMissileReleasing();

        waitPoints = FindObjectsOfType<WaitPoint>();
        foreach (var item in waitPoints)
        {
            item.ActivatePoint(false);
        }

        capturePoints = FindObjectsOfType<CapturePoint>();
        foreach (var item in capturePoints)
        {
            item.ActivatePoint(false);
        }

        musicController.ActivateGameTheme(true);
        musicController.ActivateStoryTheme(false);
        textHolder.rectTransform.localScale = Vector3.zero;
        downloadTimer.gameObject.SetActive(true);
        listener.enabled = false;
        ActivateNextTask();
    }

    private void EndGameLose()
    {
        loseHolder.SetActive(true);
        FadeInScreen(0.3f); 
    }

    private void EndGameWin()
    {
        if (FadeInWin() && !next)
        {
            next = true;
            winHolder.SetActive(false);
            Time.timeScale = 1;
            NextStorySection();
        } 
        else if (!next)
        {
            winHolder.SetActive(true);
        }
    }

    private bool FadeInWin()
    {
        FadeInScreen(.25f);

        uploadingTimer += Time.unscaledDeltaTime * 25; 
        uploadingText.text = "Uploading " + Mathf.Round(Mathf.Clamp(uploadingTimer, 0, 100)).ToString() + " %";  

        if(uploadingTimer >= 120 && !next)
        {
            GetComponent<AudioSource>().PlayOneShot(warpSound, .1f);
            return true;
        }
        return false;
    }

    private void FadeInScreen(float speed)
    {
        fadeBlackScreen.gameObject.SetActive(true);
        Color fadeInColour = fadeBlackScreen.color;
        fadeInColour.a += Time.unscaledDeltaTime * speed;
        fadeBlackScreen.color = fadeInColour;
    }

    private void NextStorySection()
    {
        listener.enabled = true;

        musicController.ActivateGameTheme(false);
        if (creepyMusic)
        {
            musicController.ActivateCreepyTheme(true);
        }
        else
        {
            musicController.ActivateStoryTheme(true);
        }

        textHolder.rectTransform.localScale = Vector3.one;
        gameHolder.SetActive(false);
        downloadTimer.gameObject.SetActive(false);
        FindObjectOfType<Game>().AchievedGoal();
    }

    public void GameOver(bool won)
    {
        downloadTimer.gameObject.SetActive(false);
        startCountdown = false;
        countdownText.text = "";
        Time.timeScale = 0;
        gameWon = won;
        gameEnded = true;
    }
}
