using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _fireRate = 1.0f;
    private float _canFire = -1f;
    private Player _player;

    [SerializeField]
    private GameObject _laserPreFab;

    private float deadSpeed = 1f;
    private float destroyAnimDuration = 2.4f;

    private Animator _animator;
    private BoxCollider2D _collider;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _collider = gameObject.GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        if (_player == null)
            Debug.LogError("_player is null");

        if (_animator == null)
            Debug.LogError("Animator is null");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.5f)
        {
            Vector3 randomPosition = GetRandomStartPosition();
            transform.position = randomPosition;
        }
    }

    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPreFab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (var laser in lasers)
            {
                laser.AssignLaserToEnemy();
            }
        }
    }

    public Vector3 GetRandomStartPosition()
    {
        float xAxis = Random.Range(-9f, 9f);
        return new Vector3(xAxis, 6, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LaserCollide(other);
        PlayerCollide(other);
    }

    private void PlayerCollide(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player)
            {
                player.Damage();
                player.UpdateLives();
            }

            OnEnemyDeath();

            audioSource.Play();
            Destroy(this.gameObject, destroyAnimDuration);
        }
    }

    private void LaserCollide(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            OnEnemyDeath();

            audioSource.Play();
            Destroy(this.gameObject, destroyAnimDuration);

            _player.IncreaseScore();
        }
    }

    private void OnEnemyDeath()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _speed = deadSpeed;
        _collider.enabled = false;
        _canFire = -1f;
    }
}
