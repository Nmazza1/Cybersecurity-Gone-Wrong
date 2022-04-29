using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour

{
    // C = is pause menu
    // S = status menu
    [SerializeField]
    Canvas pauseCanvas, statusCanvas;



    public bool isPaused = false;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
             
            }
            else
            {
                Pause();
                  
            }
        }
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        isPaused = true;
        pauseCanvas.enabled = true;
        statusCanvas.enabled = false;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        isPaused = false;
        pauseCanvas.enabled = false;
        statusCanvas.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu_Scene");
        
    }
}
