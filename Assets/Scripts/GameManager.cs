using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool _isGameOver;
    public bool isCoopMode = false;
    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            if (isCoopMode)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        SetTimeScale(0f);
        _uiManager.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        SetTimeScale(1f);
        _uiManager.HidePauseMenu();
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
