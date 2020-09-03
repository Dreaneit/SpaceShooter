using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            DestroyLaserObjects();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8f)
        {
            DestroyLaserObjects();
        }
    }

    private void DestroyLaserObjects()
    {
        DestroyParent(transform.parent);
        Destroy(this.gameObject);
    }

    private void DestroyParent (Transform parent)
    {
        if (parent != null)
        {
            Destroy(parent.gameObject);
        }
    }

    public void AssignLaserToEnemy()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerLaserCollide(other);
    }

    private void PlayerLaserCollide(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.transform.GetComponent<Player>();

            if (player)
            {
                player.Damage();
                player.UpdateLives();
            }
        }
    }
}
