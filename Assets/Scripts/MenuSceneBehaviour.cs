using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneBehaviour : MonoBehaviour
{

    public void LoadGame()
    {
        SceneManager.LoadScene("MainWorld");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {

    }

}

