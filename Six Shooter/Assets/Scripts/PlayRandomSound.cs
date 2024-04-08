using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    private AudioSource[] audioSources;

    private void Start()
    {
        audioSources = new AudioSource[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            audioSources[i] = transform.GetChild(i).GetComponent<AudioSource>();

        InvokeRepeating(nameof(PlayRandom), Random.Range(5, 15), Random.Range(30, 45));
    }

    public void PlayRandom()
    {
        int randomIndex = Random.Range(0, audioSources.Length);
        audioSources[randomIndex].Play();
    }
}
