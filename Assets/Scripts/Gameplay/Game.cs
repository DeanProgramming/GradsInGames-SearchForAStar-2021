using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private StoryData _data;

    private TextDisplay _output;
    private BeatData _currentBeat;
    private WaitForSeconds _wait;
    private float shortWait = 0.04f;
    private float longWait = 2;
     
    [SerializeField] private TMP_Text textSetting;

    public GameObject laptopHolder;
    public GameObject gameCamera;
    public Canvas gameCanvas;
    public Camera screenCamera; 


    private void Awake()
    {
        _output = GetComponentInChildren<TextDisplay>();
        _output._shortWait = new WaitForSeconds(shortWait);
        _output._longWait = new WaitForSeconds(longWait); 
        _currentBeat = null;
        _wait = new WaitForSeconds(0.5f);
    }

    private void Update()
    {
        if(_output.IsIdle)
        {
            if (_currentBeat == null)
            {
                DisplayBeat(1);
            }
            else
            {
                UpdateInput();
            }
        }
    }

    public void ResetTime()
    {
        UpdateTime(shortWait, longWait);
    }

    public void UpdateTime(float newShortWait, float newLongWait)
    {
        _output._shortWait = new WaitForSeconds(newShortWait);
        _output._longWait = new WaitForSeconds(newLongWait);
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_currentBeat != null)
            {
                if (_currentBeat.ID == 1)
                {
                    Application.Quit();
                }
                else
                {
                    DisplayBeat(1);
                }
            }
        }
        else
        {
            KeyCode alpha = KeyCode.Alpha1;
            KeyCode keypad = KeyCode.Keypad1;

            for (int count = 0; count < _currentBeat.Decision.Count; ++count)
            {
                if (alpha <= KeyCode.Alpha9 && keypad <= KeyCode.Keypad9)
                {
                    if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                    {
                        ChoiceData choice = _currentBeat.Decision[count];

                        if (choice.RequiresGoal == false)
                        { 
                            //Changes - Gave the choice data the ability to control if the game should start, end or carry on with text display.
                            if (choice.BeginGame)
                            { 
                                FindObjectOfType<GameManager>().BeginGame();
                            } 
                            
                            if (choice.ChangesLevel)
                            {
                                SceneManager.LoadScene(choice.LevelName, LoadSceneMode.Single);
                            }
                            else if (choice.EndGame)
                            {
                                SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
                            }
                            else
                            {
                                DisplayBeat(choice.NextID);

                                if (choice.AdjustTextAlignment && choice.OnLaptop == false)
                                { 
                                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, -100, Camera.main.transform.position.z);
                                    textSetting.alignment = TextAlignmentOptions.Center; 
                                }
                                else if (choice.OnLaptop == false)
                                {
                                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21, Camera.main.transform.position.z);
                                    textSetting.alignment = TextAlignmentOptions.TopLeft;
                                } 

                                if (choice.OnLaptop == true && choice.BeginGame == false)
                                {
                                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21, Camera.main.transform.position.z);
                                    laptopHolder.SetActive(true);
                                    gameCamera.SetActive(false);
                                    gameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                                    gameCanvas.worldCamera = screenCamera;
                                    textSetting.alignment = TextAlignmentOptions.BottomLeft;
                                }
                                else
                                {
                                    laptopHolder.SetActive(false);
                                    gameCamera.SetActive(true);
                                    gameCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                                }
                            }
                            break;
                        }
                    }
                }

                ++alpha;
                ++keypad;
            }
        }
    }

    private void DisplayBeat(int id)
    {
        BeatData data = _data.GetBeatById(id);
        _currentBeat = data;

        if (_currentBeat.EditTime)
        {
            UpdateTime(_currentBeat.ShortTime, _currentBeat.LongTime);
        }
        else
        {
            ResetTime();
        }

        StartCoroutine(DoDisplay(data));
    }

    private IEnumerator DoDisplay(BeatData data)
    {
        _output.Clear(data);

        while (_output.IsBusy)
        {
            yield return null;
        }

        _output.Display(data.DisplayText);

        while(_output.IsBusy)
        {
            yield return null;
        }
        
        for (int count = 0; count < data.Decision.Count; ++count)
        {
            ChoiceData choice = data.Decision[count];
            _output.Display(string.Format("{0}: {1}", (count + 1), choice.DisplayText));

            while (_output.IsBusy)
            {
                yield return null;
            }
        }

        if(data.Decision.Count > 0)
        {
            _output.ShowWaitingForInput();
        }
    }

    public void AchievedGoal()
    {
        ChoiceData choice = _currentBeat.Decision[0];

        //Changes - Gave the choice data the ability to control if the game should start, end or carry on with text display.
        if (choice.ChangesLevel)
        {
            SceneManager.LoadScene(choice.LevelName, LoadSceneMode.Single);
        }
        else if (choice.EndGame)
        {
            Application.Quit();
        }
        else
        {
            DisplayBeat(choice.NextID);

            if (choice.AdjustTextAlignment && choice.OnLaptop == false)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, -100, Camera.main.transform.position.z);
                textSetting.alignment = TextAlignmentOptions.Center;
            }
            else if (choice.OnLaptop == false)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21, Camera.main.transform.position.z);
                textSetting.alignment = TextAlignmentOptions.TopLeft;
            }

            if (choice.OnLaptop == true)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 21, Camera.main.transform.position.z);
                laptopHolder.SetActive(true);
                gameCamera.SetActive(false);
                gameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                gameCanvas.worldCamera = screenCamera;
                textSetting.alignment = TextAlignmentOptions.BottomLeft;
                textSetting.fontSize = 85.94f;
            }
            else
            {
                laptopHolder.SetActive(false);
                gameCamera.SetActive(true);
                gameCanvas.renderMode = RenderMode.ScreenSpaceOverlay; 
            }
        }
    }

    public bool OutputIsIdle()
    { 
        return _output.IsIdle;
    }
}
