using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatShot : MonoBehaviour
{
    private ParticleSystem hatParticle;
    private bool hasIncreased = false;

    private void Start()
    {
        hatParticle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (hatParticle.isPlaying && !hasIncreased)
        {
            GameManager.Instance.hatsKnocked++;

            hasIncreased = true;
        }
    }
}
