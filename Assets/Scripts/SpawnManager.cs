using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawnTimeEnemy = 5f;
    private bool _stopSpawnEnemy = false;
    private bool _stopSpawnPowerup = false;
    [SerializeField]
    private GameObject[] powerups;

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutineEnemy(_spawnTimeEnemy));
        StartCoroutine(SpawnRoutinePowerup());
    }

    IEnumerator SpawnRoutinePowerup()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawnPowerup == false)
        {
            int powerUpRandomIndex = Random.Range(0,3);
            Instantiate(powerups[powerUpRandomIndex], new Vector3(Random.Range(-9,9), 5, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15, 20));
        }
    }

    IEnumerator SpawnRoutineEnemy(float spawnTime)
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawnEnemy == false)
        {
            Enemy enemy = _enemyPrefab.GetComponent<Enemy>();
            if (enemy)
            {
                GameObject newEnemy =  Instantiate(_enemyPrefab, enemy.GetRandomStartPosition(), Quaternion.identity);
                newEnemy.transform.SetParent(_enemyContainer.transform);
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void StopSpawnEnemy()
    {
        _stopSpawnEnemy = true;
    }

    public void StopSpawnPowerupShot()
    {
        _stopSpawnPowerup = true;
    }
}
