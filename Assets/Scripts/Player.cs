using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool _isPlayerOne = false;
    public bool _isPlayerTwo = false;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private GameObject _tripleShotPreFab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f; 
    [SerializeField]
    private int _lives = 3;
    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    private Transform _shieldProtection;
    private Transform _leftEngineDamage;
    private Transform _rightEngineDamage;
    private Transform _turnLeft;
    private Transform _turnRight;
    private int _score;
    [SerializeField]
    private AudioClip laserShotClip;
    [SerializeField]
    private AudioSource audioSource;

    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private UIManager _uIManager;

    private GameObject playerOne = null;
    private GameObject playerTwo = null;

    private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _shieldProtection = gameObject.transform.Find("Shield_protection");
        _rightEngineDamage = gameObject.transform.Find("Right_Engine_Fire");
        _leftEngineDamage = gameObject.transform.Find("Left_Engine_Fire");
        _turnLeft = gameObject.transform.Find("Turn_Left");
        _turnRight = gameObject.transform.Find("Turn_Right");

        _sprite = gameObject.GetComponent<SpriteRenderer>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            
        audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL.");
        }

        if (_uIManager == null)
        {
            Debug.LogError("_uIManager is NULL.");
        }

        if (_gameManager == null)
        {
            Debug.LogError("game manager is NULL.");
        }

        if (audioSource == null)
        {
            Debug.LogError("audio source for player is NULL.");
        }
        else
        {
            audioSource.clip = laserShotClip;
        }

        if (!_gameManager.isCoopMode)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePlayerOneMovement();
        CalculatePlayerTwoMovement();
    }
    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Vector3 offset = new Vector3(0, 1.2f, 0);
        GameObject laser = _isTripleShotActive ? _tripleShotPreFab : _laserPreFab;
        Instantiate(laser, transform.position + offset, Quaternion.identity);
        audioSource.Play(0);
    }

    #region Movement
    public void CalculatePlayerOneMovement()
    {
        if (_isPlayerOne)
        {
            HandlePlayerOneMovement();
            HandleBounds();

            HandleFire(KeyCode.KeypadEnter);
        }
    }

    public void CalculatePlayerTwoMovement()
    {
        if (_isPlayerTwo)
        {
            HandlePlayerTwoMovement();
            HandleBounds();

            HandleFire(KeyCode.Space);
        }
    }

    public void HandleFire(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void HandleBounds()
    {
        float _bottomBoundY = -3.5f;
        float _rightBoundX = 11.3f;
        float _leftBoundX = -11.3f;

        float yBounds = Mathf.Clamp(transform.position.y, _bottomBoundY, 0);
        transform.position = new Vector3(transform.position.x, yBounds, 0);

        if (transform.position.x >= _rightBoundX)
        {
            transform.position = new Vector3(_leftBoundX, transform.position.y, 0);
        }
        else if (transform.position.x <= _leftBoundX)
        {
            transform.position = new Vector3(_rightBoundX, transform.position.y, 0);
        }
    }

    public void HandlePlayerTwoMovement()
    {
        PlayerMoveAnimation(KeyCode.A, _turnLeft);
        PlayerMoveAnimation(KeyCode.D, _turnRight);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            float horizontalInput = Input.GetAxis("P2_Horizontal");
            float verticalInput = Input.GetAxis("P2_Vertical");

            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            
            this.transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    void HandlePlayerOneMovement()
    {
        PlayerMoveAnimation(KeyCode.LeftArrow, _turnLeft);
        PlayerMoveAnimation(KeyCode.RightArrow, _turnRight);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
                
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            ////deltaTime - realtime
            this.transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    public void PlayerMoveAnimation(KeyCode moveDirection, Transform animation)
    {
        if (Input.GetKeyDown(moveDirection))
        {
            _sprite.enabled = false;
            animation.gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(moveDirection))
        {
            animation.gameObject.SetActive(false);
            _sprite.enabled = true;
        }
    }
    #endregion

    #region Damage
    public void Damage()
    {
        if (_isShieldActive == false)
        {
            _lives--;
            EngineDamageAnimation();

            if (_lives < 1)
            {
                OnDead();
            }
        }
        else
        {
            _isShieldActive = false;
            ToggleShieldProtection(false);
        }
    }

    private void EngineDamageAnimation()
    {
        if (_lives == 2)
        {
            _leftEngineDamage.gameObject.SetActive(true);
        } 
        else if (_lives == 1)
        {
            _rightEngineDamage.gameObject.SetActive(true);
        }
    }

    private void OnDead()
    {
        _spawnManager.StopSpawnEnemy();
        _spawnManager.StopSpawnPowerupShot();
        Destroy(this.gameObject);
    }
    #endregion

    #region Powerups
    public void SetTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(ShutDownTripleShot());
    }

    IEnumerator ShutDownTripleShot()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SetSpeed(int speedMultiplier)
    {
        _speed = _speed * speedMultiplier;
        StartCoroutine(ShutDownSpeed(speedMultiplier));
    }

    IEnumerator ShutDownSpeed(int speedNoun)
    {
        yield return new WaitForSeconds(5);
        _speed = _speed / speedNoun;
    }

    public void SetShield()
    {
        _isShieldActive = true;
        ToggleShieldProtection(true);
        StartCoroutine(ShutDownShield());
    }

    IEnumerator ShutDownShield()
    {
        yield return new WaitForSeconds(5);
        ToggleShieldProtection(false);
        _isShieldActive = false;
    }

    private void ToggleShieldProtection(bool setActive)
    {
        if (_shieldProtection != null)
        {
            _shieldProtection.gameObject.SetActive(setActive);
        }
    }
    #endregion

    public void IncreaseScore()
    {
        _score += 10;
        _uIManager.IncreaseScore(_score);
    }

    public void UpdateLives()
    {
        _uIManager.UpdateLives(_lives);
    }
}
