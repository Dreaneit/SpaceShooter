using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void LoadSinglePlayer()
    {
        _gameManager.SetTimeScale(1f);
        SceneManager.LoadScene(1);
    }
    public void LoadCoOp()
    {
        _gameManager.SetTimeScale(1f);
        SceneManager.LoadScene(2);
    }
}
