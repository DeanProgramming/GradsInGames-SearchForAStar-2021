using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public enum State { Initialising, Idle, Busy }

    private TMP_Text _displayText;
    private string _displayString;
    public WaitForSeconds _shortWait = new WaitForSeconds(0.1f);
    public WaitForSeconds _longWait = new WaitForSeconds(0.1f);
    private State _state = State.Initialising; 
    private AudioSource typingAudioSource;

    public bool IsIdle { get { return _state == State.Idle; } }
    public bool IsBusy { get { return _state != State.Idle; } }

    private void Awake()
    {
        _displayText = GetComponent<TMP_Text>();
        typingAudioSource = GetComponent<AudioSource>();
        typingAudioSource.volume = 0.01f;
        _displayText.text = string.Empty;
        _state = State.Idle; 
    }

    private IEnumerator DoShowText(string text)
    {
        int currentLetter = 0;
        char[] charArray = text.ToCharArray(); 

        while (currentLetter < charArray.Length)
        { 
            if (charArray[currentLetter] == '[')
            {
                _shortWait = new WaitForSeconds(.25f); 
                currentLetter++;
            }
            else if (charArray[currentLetter] == ']')
            { 
                FindObjectOfType<Game>().ResetTime();
                currentLetter++;
            }
            else if (charArray[currentLetter] == '~')
            {
                _displayText.text = "";
                currentLetter++;
            }
            else if (charArray[currentLetter] == '>')
            {
                char[] pcName = System.Environment.UserName.ToCharArray();
                for (int i = 0; i < pcName.Length; i++)
                {
                    _displayText.text += pcName[i];
                    yield return _shortWait;
                }
                currentLetter++;
            }
            else if (charArray[currentLetter] == '<')
            {
                char[] deactivatedRed = "<color=#FF0000> Deactivated </color>".ToCharArray();
                for (int i = 0; i < deactivatedRed.Length; i++)
                {
                    _displayText.text += deactivatedRed[i];
                }
                currentLetter++;
            }
            else if (charArray[currentLetter] == '+')
            {
                char[] deactivatedRed = "<color=#FF0000>ERROR CODE - 98S5F6221A 00001 </color>".ToCharArray();
                for (int i = 0; i < deactivatedRed.Length; i++)
                {
                    _displayText.text += deactivatedRed[i];
                }
                currentLetter++;
            }
            else
            {
                _displayText.text += charArray[currentLetter++];
                typingAudioSource.Play();
            }
            yield return _shortWait;
        }

        _displayText.text += "\n";
        _displayString = _displayText.text;

        typingAudioSource.Stop();
        _state = State.Idle;
    }

    private IEnumerator DoAwaitingInput()
    {
        bool on = true;

        while (enabled)
        {
            _displayText.text = string.Format( "{0}> {1}", _displayString, ( on ? "|" : " " ));
            on = !on;
            yield return _longWait;
        }
    }

    private IEnumerator DoClearText()
    {
        int currentLetter = 0;
        char[] charArray = _displayText.text.ToCharArray();

        while (currentLetter < charArray.Length)
        {
            if (currentLetter > 0 && charArray[currentLetter - 1] != '\n')
            {
                charArray[currentLetter - 1] = ' ';
            }

            if (charArray[currentLetter] != '\n')
            {
                charArray[currentLetter] = '_';
            }

            _displayText.text = charArray.ArrayToString();
            ++currentLetter;
            yield return null;
        }

        _displayString = string.Empty;
        _displayText.text = _displayString;
        _state = State.Idle;
    }

    public void Display(string text)
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            StartCoroutine(DoShowText(text));
        }
    }

    public void ShowWaitingForInput()
    {
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            StartCoroutine(DoAwaitingInput());
        }
    }

    public void Clear(BeatData data)
    { 
        if (_state == State.Idle)
        {
            StopAllCoroutines();
            _state = State.Busy;
            if (data.InstantWipe)
            {
                _displayText.text = "";
                _displayString = _displayText.text;
                _state = State.Idle;
            }
            else
            {
                StartCoroutine(DoClearText());
            }
        }
    }  
}
