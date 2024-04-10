using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SnowCollision : MonoBehaviour
{
    private ParticleSystem snowParticles;
    private ParticleSystem.NoiseModule noiseModule;
    private ParticleSystem.RotationOverLifetimeModule rotationModule;

    private void Start()
    {
        snowParticles = GetComponent<ParticleSystem>();
        noiseModule = snowParticles.noise;
        rotationModule = snowParticles.rotationOverLifetime;

        var collision = snowParticles.collision;
        collision.sendCollisionMessages = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        noiseModule.enabled = false;
        rotationModule.enabled = false;
    }
}
