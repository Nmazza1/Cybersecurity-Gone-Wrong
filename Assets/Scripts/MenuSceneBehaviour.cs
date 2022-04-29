using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MenuSceneBehaviour : MonoBehaviour
{

    public void LoadGame()
    {
        EditorSceneManager.LoadScene("MainWorld");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {

    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

