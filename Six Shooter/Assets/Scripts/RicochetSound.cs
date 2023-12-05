using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetSound : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.8f, 1.1f);
        audioSource.Play();

        Destroy(gameObject, 3f);
    }
}
