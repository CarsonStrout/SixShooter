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
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
