using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("");
    }

    public void Settings()
    {

    }

    public void QuiteGame()
    {
        Application.Quit();
    }


}
