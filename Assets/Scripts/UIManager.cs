using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text _score, _bestText;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restart;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _pauseMenu;
    private int _bestScore, _actualScore;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _score.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);

        _bestScore = PlayerPrefs.GetInt("BestScore");
        _bestText.text = "Best: " + _bestScore;
    }

    public void IncreaseScore(int score)
    {
        _actualScore = score;
        _score.text = "Score: " + _actualScore;
    }

    public void CheckForBestScore()
    {
        if (_actualScore > _bestScore)
        {
            _bestScore = _actualScore;
            PlayerPrefs.SetInt("BestScore", _bestScore);
            _bestText.text = "Best: " + _bestScore;
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            _gameManager.GameOver();
            StartCoroutine(DisplayGameOverTextRoutine());
        }
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _pauseMenu.gameObject.SetActive(false);
    }

    IEnumerator DisplayGameOverTextRoutine()
    {
        while (true)
        {
            _restart.gameObject.SetActive(true);
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(.35f);
            _gameOver.gameObject.SetActive(false);
            _restart.gameObject.SetActive(false);
            yield return new WaitForSeconds(.35f);
        }
    }
}
