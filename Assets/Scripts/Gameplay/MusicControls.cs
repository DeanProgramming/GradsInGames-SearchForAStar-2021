using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControls : MonoBehaviour
{
    public static MusicControls instance = null;

    [SerializeField] private AudioSource storyTheme;
    [SerializeField] private AudioSource creepyTheme;
    [SerializeField] private AudioSource gameTheme;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        creepyTheme.Play();
        creepyTheme.Pause();
        gameTheme.Play();
        gameTheme.Pause();

        ActivateStoryTheme(true);

        DontDestroyOnLoad(gameObject);
    }

    public void ActivateStoryTheme(bool on)
    {
        if (on)
        {
            storyTheme.UnPause();
        }
        else
        {
            storyTheme.Pause();
        }
    }

    public void ActivateCreepyTheme(bool on)
    {
        if (on)
        {
            creepyTheme.UnPause();
        }
        else
        {
            creepyTheme.Pause();
        }
    }

    public void ActivateGameTheme(bool on)
    {
        if (on)
        {
            gameTheme.UnPause();
        }
        else
        {
            gameTheme.Pause();
        }
    }
}
