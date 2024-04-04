using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

public class EnemyStats : MonoBehaviour
{
    public int EnemyHealth = 20;
    [SerializeField] private DamageFlash damageFlash;
    [SerializeField] private AudioSource enemyHitSound;
    [SerializeField] private AudioSource enemyDeathSound;
    [SerializeField] private ParticleSystem enemyDeathParticles, drunkParticles;
    [SerializeField] private GameObject[] visualComponents;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private GameObject damagePopup;
    [SerializeField] private Transform damagePopupPosition, healthPrefabPosition;
    [SerializeField] private GameObject healthPrefab;
    private bool isDead = false;
    private WaveSpawner waveSpawner;

    private void Start()
    {
        waveSpawner = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveSpawner>();

        int activeLevel = (int)GameManager.Instance.activeLevel;

        switch (activeLevel)
        {
            case 0:
                EnemyHealth = 15;
                break;
            case 1:
                EnemyHealth = 18;
                break;
            case 2:
                EnemyHealth = 21;
                break;
            case 3:
                EnemyHealth = 24;
                break;
            case 4:
                EnemyHealth = 27;
                break;
            case 5:
                EnemyHealth = 30;
                break;
        }
    }

    private void Update()
    {
        if (EnemyHealth <= 0 && !isDead)
        {
            Died();
        }
    }

    public void TakeDamage(int damage, bool isCrit)
    {
        EnemyHealth -= damage;
        enemyHitSound.Play();

        damageFlash.Flash();

        GameObject popup = Instantiate(damagePopup, damagePopupPosition.position, Quaternion.identity);

        popup.transform.GetChild(0).GetComponent<DamagePopup>().SetDamage(damage, isCrit);
    }

    private void Died()
    {
        isDead = true;

        if (drunkParticles.isPlaying)
            drunkParticles.Stop();

        enemyAI.enabled = false;

        for (int i = 0; i < visualComponents.Length; i++)
            visualComponents[i].SetActive(false);

        waveSpawner.EnemyKilled();

        enemyDeathSound.Play();
        enemyDeathParticles.Play();

        GameObject health = Instantiate(healthPrefab, healthPrefabPosition.position, gameObject.transform.rotation);

        Destroy(gameObject, 5f);
    }
}
