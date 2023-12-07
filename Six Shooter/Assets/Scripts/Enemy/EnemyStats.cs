using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

public class EnemyStats : MonoBehaviour
{
    public int EnemyHealth = 20;
    [SerializeField] private AudioSource enemyHitSound;
    [SerializeField] private AudioSource enemyDeathSound;
    [SerializeField] private ParticleSystem enemyDeathParticles;
    [SerializeField] private GameObject[] visualComponents;
    [SerializeField] private EnemyAI enemyAI;
    private bool isDead = false;
    private WaveSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveSpawner>();
    }

    private void Update()
    {
        if (EnemyHealth <= 0 && !isDead)
        {
            Died();
        }
    }

    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
        enemyHitSound.Play();
    }

    private void Died()
    {
        isDead = true;

        enemyAI.enabled = false;

        for (int i = 0; i < visualComponents.Length; i++)
            visualComponents[i].SetActive(false);

        waveSpawner.EnemyKilled();

        enemyDeathSound.Play();
        enemyDeathParticles.Play();

        Destroy(gameObject, 5f);
    }
}
