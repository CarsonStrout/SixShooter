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
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject completeLevelUI;
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private MeshCollider groundCollider;

    private int enemyAmount;

    private int waveNumber = 0;
    private float searchCountdown = 1f;
    private int lastSpawnIndex = -1;
    private bool isWaveSpawning = true;

    private bool levelDone = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        playerUI.SetActive(false);
        waveUI.SetActive(false);

        yield return new WaitForSeconds(2);

        waveText.text = "Wave " + (waveNumber + 1);
        waveUI.SetActive(true);

        yield return new WaitForSeconds(5);

        waveUI.SetActive(false);

        yield return new WaitForSeconds(1);

        playerUI.SetActive(true);

        Wave wave = waves[waveNumber];
        enemyAmount = wave.count;
        enemiesRemainingText.text = "Enemies Remaining: " + enemyAmount.ToString();

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        waveNumber++;

        isWaveSpawning = false;
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

    public void EnemyKilled()
    {
        enemyAmount--;
        enemiesRemainingText.text = "Enemies Remaining: " + enemyAmount.ToString();
    }

    private void Update()
    {
        if (!isWaveSpawning && EnemiesAreAllDead())
        {
            if (waveNumber < waves.Length)
            {
                isWaveSpawning = true;
                StartCoroutine(SpawnWave());
            }
            else
            {
                playerUI.SetActive(false);
                waveUI.SetActive(false);

                groundCollider.excludeLayers = LayerMask.GetMask("PlayerBullet");


                if (GameManager.Instance.activeLevel != ActiveLevel.Level6)
                {
                    if (levelDone)
                        return;

                    levelDone = true;
                    completeLevelUI.SetActive(true);
                    GameManager.Instance.UpdateGameState(GameState.CompleteLevel);
                }
                else
                {
                    winUI.SetActive(true);
                    GameManager.Instance.UpdateGameState(GameState.WinGame);
                }
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
