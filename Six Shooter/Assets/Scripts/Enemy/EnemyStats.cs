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
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private ParticleSystem enemyDeathParticles, drunkParticles, brawlParticles;
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

        // instantiate damage popup with a bit of randomization to make it look more natural
        GameObject popup = Instantiate(damagePopup, damagePopupPosition.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);

        // Calculate the rotation based on the X offset.
        float xOffset = popup.transform.position.x - damagePopupPosition.position.x;
        float zRotation = xOffset * 60;

        // Apply the rotation to the popup.
        popup.transform.rotation = Quaternion.Euler(0, 0, zRotation);

        popup.transform.GetChild(0).GetComponent<DamagePopup>().SetDamage(damage, isCrit);
    }

    private void Died()
    {
        isDead = true;

        if (footstepSound.isPlaying)
            footstepSound.Stop();

        if (drunkParticles.isPlaying)
            drunkParticles.Stop();

        if (brawlParticles.isPlaying)
            brawlParticles.Stop();

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
