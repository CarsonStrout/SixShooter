using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [SerializeField] private GameObject waveUI;
    [SerializeField] private TextMeshProUGUI waveText;

    private int waveNumber = 0;
    private float searchCountdown = 1f;
    private int lastSpawnIndex = -1;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // Display the wave text and activate the UI
        waveText.text = "Wave " + (waveNumber + 1);
        waveUI.SetActive(true);

        // Wait for 3 seconds before starting the wave
        yield return new WaitForSeconds(5);

        // Turn off the wave UI after 3 seconds
        waveUI.SetActive(false);

        yield return new WaitForSeconds(1);

        // Retrieve the current wave details
        Wave wave = waves[waveNumber];

        // Spawn enemies based on wave configuration
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        // Increment the wave number for the next wave
        waveNumber++;
    }
    void SpawnEnemy(GameObject enemy)
    {
        int randIndex;
        do
        {
            randIndex = Random.Range(0, spawnPoints.Length);
        } while (randIndex == lastSpawnIndex);

        Instantiate(enemy, spawnPoints[randIndex].position, spawnPoints[randIndex].rotation);

        lastSpawnIndex = randIndex;
    }

    private void Update()
    {
        if (EnemiesAreAllDead())
        {
            if (waveNumber < waves.Length)
            {
                // showUI = true;
                StartCoroutine(SpawnWave());
            }
            else
            {
                Debug.Log("All waves complete!");

                // End UI
                // Options to replay, go to menu, and exit game
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
