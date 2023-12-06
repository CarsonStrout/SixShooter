using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private float maxHealthBarLength = 200f;
    private float maxHealth;
    private float currentHealth;
    private float healthPercentage;
    private float currentHealthBarLength;

    [SerializeField] private RectTransform rt;

    private void Start()
    {
        maxHealth = playerStats.PlayerHealth;
    }

    public void UpdateHealthBar(int damage)
    {
        currentHealth = playerStats.PlayerHealth;
        healthPercentage = currentHealth / maxHealth;
        currentHealthBarLength = maxHealthBarLength * healthPercentage;
        rt.sizeDelta = new Vector2(currentHealthBarLength, rt.sizeDelta.y);
    }
}
