using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restart;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _score.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);
    }

    public void IncreaseScore(int score)
    {
        _score.text = "Score: " + score;
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
