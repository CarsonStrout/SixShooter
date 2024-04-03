using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float sprintSpeed = 5;
    [SerializeField] private AudioSource playerHitSound;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject waveManager;
    [SerializeField] private GameObject[] otherUIs;
    [SerializeField] private MeshCollider groundCollider;

    public int PlayerHealth = 20;

    private void Update()
    {
        if (PlayerHealth <= 0)
            PlayerDeath();
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;
        healthBar.UpdateHealthBar(damage);
        playerHitSound.Play();
    }

    private void PlayerDeath()
    {
        waveManager.SetActive(false);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
            Destroy(enemy);

        foreach (GameObject ui in otherUIs)
            ui.SetActive(false);

        groundCollider.excludeLayers = LayerMask.GetMask("PlayerBullet");

        GameManager.Instance.UpdateGameState(GameState.LoseGame);

        loseUI.SetActive(true);
    }
}
