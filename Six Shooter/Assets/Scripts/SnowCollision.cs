using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SnowCollision : MonoBehaviour
{
    private ParticleSystem snowParticles;
    private ParticleSystem.NoiseModule noiseModule;
    private ParticleSystem.RotationOverLifetimeModule rotationModule;
    private ParticleSystem.MainModule mainModule;


    private void Start()
    {
        snowParticles = GetComponent<ParticleSystem>();
        noiseModule = snowParticles.noise;
        rotationModule = snowParticles.rotationOverLifetime;
        mainModule = snowParticles.main;

        var collision = snowParticles.collision;
        collision.sendCollisionMessages = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        noiseModule.enabled = false;
        rotationModule.enabled = false;
        mainModule.startSpeed = 0;
    }
}
