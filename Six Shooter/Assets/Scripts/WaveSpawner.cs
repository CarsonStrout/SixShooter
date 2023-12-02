using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int count;
        public float spawnInterval;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;

    private int waveNumber = 0;
    private float searchCountdown = 1f;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveNumber];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        waveNumber++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    private void Update()
    {
        if (EnemiesAreAllDead())
        {
            if (waveNumber < waves.Length)
            {
                StartCoroutine(SpawnWave());
            }
            else
            {
                Debug.Log("All waves complete!");
                // You can implement what happens when all waves are complete.
            }
        }
    }

    bool EnemiesAreAllDead()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                return true;
        }

        return false;
    }
}
