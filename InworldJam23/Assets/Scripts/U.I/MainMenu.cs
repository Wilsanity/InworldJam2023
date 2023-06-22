using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    public string creditsName;

    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }

  

    public void QuiteGame()
    {
        Application.Quit();
    }

    public void CreditScene()
    {
        SceneManager.LoadScene(creditsName);

    }

}
