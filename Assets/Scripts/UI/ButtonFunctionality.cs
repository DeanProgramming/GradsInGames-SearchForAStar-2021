using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{
    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void HideItem(GameObject item)
    {
        item.SetActive(false);
    }
    public void ShowItem(GameObject item)
    {
        item.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
