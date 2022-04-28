using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour

{
    // C = is pause menu
    // S = status menu
    [SerializeField]
    Canvas c, s;



    public bool isPaused = false;
    // Start is called before the first frame update
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
        Time.timeScale = 0;
        isPaused = true;
        c.enabled = true;
        s.enabled = false;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
        c.enabled = false;
        s.enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
