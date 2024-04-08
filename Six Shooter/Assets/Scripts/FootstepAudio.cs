using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private PlayerMovement playerMovement;
    private Vector3 lastPosition;
    [SerializeField] private ParticleSystem sprintParticles;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position != lastPosition)
        {
            audioSource.pitch = Random.Range(1.2f, 1.5f);

            if (playerMovement.isSprinting)
            {
                if (!sprintParticles.isPlaying)
                    sprintParticles.Play();
                audioSource.pitch = Random.Range(1.8f, 2.2f);
            }
            else
            {
                if (sprintParticles.isPlaying)
                    sprintParticles.Stop();
            }

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            if (sprintParticles.isPlaying)
                sprintParticles.Stop();
        }

        lastPosition = transform.position;
    }
}
