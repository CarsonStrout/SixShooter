using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State;
    public ActiveLevel activeLevel;

    public static event Action<GameState> OnGameStateChange;

    public bool useAmmo;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (upgradeSlotMachine == null)
            upgradeSlotMachine = GameObject.Find("UpgradeSlotMachine");

        if (waveSpawner == null)
            waveSpawner = GameObject.Find("WaveSpawner");
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= UpdateGameState;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (upgradeSlotMachine != null)
            UpdateGameState(GameState.UpgradeSlotMachine);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
            UpdateGameState(GameState.Menu);
        else if (scene.name == "TargetPractice")
            UpdateGameState(GameState.TargetPractice);

        if (upgradeSlotMachine == null)
            upgradeSlotMachine = GameObject.Find("UpgradeSlotMachine");

        if (waveSpawner == null)
            waveSpawner = GameObject.Find("WaveManager");

        if (upgradeSlotMachine != null)
            UpdateGameState(GameState.UpgradeSlotMachine);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Menu:
                useAmmo = true;
                break;
            case GameState.TargetPractice:
                useAmmo = true;
                break;
            case GameState.UpgradeSlotMachine:
                useAmmo = false;
                StartCoroutine(StartUpgrade());
                break;
            case GameState.WaveSpawner:
                useAmmo = true;
                StartWaves();
                break;
            case GameState.WinGame:
                useAmmo = false;
                break;
            case GameState.LoseGame:
                useAmmo = false;
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    public void UpdateActiveLevel(ActiveLevel level)
    {
        switch (level)
        {
            case ActiveLevel.Level1:
                activeLevel = ActiveLevel.Level1;
                break;
            case ActiveLevel.Level2:
                activeLevel = ActiveLevel.Level2;
                break;
            case ActiveLevel.Level3:
                activeLevel = ActiveLevel.Level3;
                break;
            case ActiveLevel.Level4:
                activeLevel = ActiveLevel.Level4;
                break;
            case ActiveLevel.Level5:
                activeLevel = ActiveLevel.Level5;
                break;
            case ActiveLevel.Level6:
                activeLevel = ActiveLevel.Level6;
                break;
        }
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
    Menu,
    TargetPractice,
    UpgradeSlotMachine,
    WaveSpawner,
    WinGame,
    LoseGame
}

public enum ActiveLevel
{
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6
}