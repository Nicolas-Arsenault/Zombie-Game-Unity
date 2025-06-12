using System;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool paused = false;
    public GameObject pannel;

    public void Start()
    {
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauserJeu();
        }
    }

    public void PauserJeu()
    {

        if(!paused)
        {
            Cursor.lockState = CursorLockMode.None;
            paused = true;
            pannel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pannel.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
