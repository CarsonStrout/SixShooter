using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject playButtons;
    private MusicManager musicManager;
    // [SerializeField] private 
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private AudioClip targetMusic;
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
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
        SceneManager.LoadScene("Cemetery");
        musicManager.CrossfadeMusic(waveMusic, 2f);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        BulletManager.Instance.ResetBulletTypes();
        GameManager.Instance.UpdateGameState(GameState.Menu);
        musicManager.CrossfadeMusic(menuMusic, 2f);
    }
}
