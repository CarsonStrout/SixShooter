using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class ButtonController : MonoBehaviour
{
    public static ButtonController Instance { get; private set; }

    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject playButtons;
    private MusicManager musicManager;
    [SerializeField] private AudioClip[] waveMusic;
    private Dictionary<string, AudioClip> levelMusicMap = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioClip targetMusic;
    [SerializeField] private AudioClip menuMusic;

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

        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    // private void Start()
    // {
    //     InitializeLevelMusicMap();
    // }

    private void InitializeLevelMusicMap()
    {
        foreach (var levelScene in GameManager.Instance.waveSceneNames)
        {
            // Assuming each scene name in waveScenes has a corresponding AudioClip in waveMusic
            // int index = GameManager.Instance.waveSceneNames.IndexOf(levelScene);

            int index = Array.IndexOf(GameManager.Instance.waveSceneNames, levelScene);

            if (index < waveMusic.Length)
            {
                levelMusicMap[levelScene] = waveMusic[index];
                Debug.Log("Added " + levelScene + " to levelMusicMap with music " + waveMusic[index].name);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void Play()
    {
        mainButtons.SetActive(false);
        playButtons.SetActive(true);
    }

    public void WaveMode()
    {
        InitializeLevelMusicMap();

        GameManager.Instance.RandomizeLevels();

        // print the new order to the console
        foreach (var scene in GameManager.Instance.waveScenes)
        {
            Debug.Log(scene);
        }

        string firstLevel = GameManager.Instance.waveScenes[0];
        GameManager.Instance.waveScenes.RemoveAt(0);
        SceneManager.LoadScene(firstLevel);
        GameManager.Instance.UpdateActiveLevel(ActiveLevel.Level1);

        if (levelMusicMap.TryGetValue(firstLevel, out AudioClip music))
        {
            musicManager.CrossfadeMusic(music, 2f);
            Debug.Log("Crossfading to " + music.name + " for " + firstLevel);
        }
    }

    public void NextLevel(string level)
    {
        // Crossfade music to the next level's music
        if (levelMusicMap.TryGetValue(level, out AudioClip music))
        {
            Debug.Log("Crossfading to " + music.name + " for " + level);
            musicManager.CrossfadeMusic(music, 2f);
        }
    }

    public void TargetMode()
    {
        SceneManager.LoadScene("TargetPractice");
        GameManager.Instance.UpdateGameState(GameState.TargetPractice);
        musicManager.CrossfadeMusic(targetMusic, 2f);
    }

    public void Back()
    {
        playButtons.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void Restart()
    {
        if (SceneManager.GetActiveScene().name == "TargetPractice")
        {
            TargetMode();
            return;
        }
        else
        {
            BulletManager.Instance.ResetBulletTypes();

            WaveMode();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        BulletManager.Instance.ResetBulletTypes();
        GameManager.Instance.UpdateGameState(GameState.Menu);
        musicManager.CrossfadeMusic(menuMusic, 2f);
    }
}
