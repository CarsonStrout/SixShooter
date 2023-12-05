using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    private AudioSource secondaryAudioSource;
    [SerializeField] private float fadeTime = 1.0f;
    private bool isCrossfading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            secondaryAudioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            secondaryAudioSource.loop = true;
            audioSource.playOnAwake = false;
            secondaryAudioSource.playOnAwake = false;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(audioSource.clip, true);
    }

    public void PlayMusic(AudioClip clip, bool fadeIn = false)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn(clip, fadeTime));
        }
        else
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopMusic(bool fadeOut = false)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(fadeTime));
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    IEnumerator FadeIn(AudioClip newClip, float duration)
    {
        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.volume = 0f;

        float currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.2f, currentTime / duration);
            yield return null;
        }
    }

    IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void CrossfadeMusic(AudioClip newClip, float duration)
    {
        if (!isCrossfading)
        {
            StartCoroutine(CrossfadeCoroutine(newClip, duration));
        }
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip, float duration)
    {
        isCrossfading = true;

        secondaryAudioSource.clip = newClip;
        secondaryAudioSource.volume = 0;
        secondaryAudioSource.Play();

        float currentTime = 0f;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            secondaryAudioSource.volume = Mathf.Lerp(0f, startVolume, currentTime / duration);
            yield return null;
        }

        AudioSource temp = audioSource;
        audioSource = secondaryAudioSource;
        secondaryAudioSource = temp;
        secondaryAudioSource.Stop();

        isCrossfading = false;
    }
}
