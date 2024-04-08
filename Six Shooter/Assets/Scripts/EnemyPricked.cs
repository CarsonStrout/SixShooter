using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPricked : MonoBehaviour
{
    public bool isPricked = false;
    [SerializeField] private float prickedDuration = 5f;
    private float damageInterval = 0.5f;
    private float nextDamageTime = 0f;
    [SerializeField] private Transform bloodSpawn;
    [SerializeField] private GameObject bloodVFX;

    private void Update()
    {
        if (isPricked && GetComponent<EnemyStats>().EnemyHealth > 0)
        {
            prickedDuration -= Time.deltaTime;

            if (Time.time >= nextDamageTime)
            {
                nextDamageTime = Time.time + damageInterval;
                GetComponent<EnemyStats>().TakeDamage(1, false);
                Instantiate(bloodVFX, bloodSpawn.position, Quaternion.identity);
            }

            if (prickedDuration <= 0)
            {
                isPricked = false;
                prickedDuration = 5f;
                nextDamageTime = 0f;
            }
        }
    }
}

