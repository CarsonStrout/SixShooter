using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State;

    public static event Action<GameState> OnGameStateChange;

    [SerializeField] private GameObject upgradeSlotMachine;
    [SerializeField] private GameObject waveSpawner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TargetPractice")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= UpdateGameState;
    }

    private void Start()
    {
        if (upgradeSlotMachine == null)
            upgradeSlotMachine = GameObject.Find("UpgradeSlotMachine");

        if (waveSpawner == null)
            waveSpawner = GameObject.Find("WaveSpawner");

        UpdateGameState(GameState.UpgradeSlotMachine);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.UpgradeSlotMachine:
                StartCoroutine(StartUpgrade());
                break;
            case GameState.WaveSpawner:
                StartWaves();
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    private IEnumerator StartUpgrade()
    {
        yield return new WaitForSeconds(2);
        upgradeSlotMachine.SetActive(true);
        yield return new WaitForSeconds(1);
        upgradeSlotMachine.GetComponent<UpgradeSlotMachine>().StartSlotMachine();
    }

    public void StartWaves()
    {
        upgradeSlotMachine.SetActive(false);
        waveSpawner.GetComponent<WaveSpawner>().StartSpawning();
    }
}

public enum GameState
{
    UpgradeSlotMachine,
    WaveSpawner
}
