using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (!spawnManager)
            Debug.LogError("spawn manager is null");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, .5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            var asteroidPostion = transform.position;

            Instantiate(explosionPrefab, asteroidPostion, Quaternion.identity);
            Destroy(collision.gameObject);
            spawnManager.StartSpawning();

            Destroy(gameObject, .15f);
        }
    }
}