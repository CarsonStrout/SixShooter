using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int EnemyHealth = 20;

    private void Update()
    {
        if (EnemyHealth <= 0)
        {
            Died();
        }
    }

    public void TakeDamage(int damage)
    {
        EnemyHealth -= damage;
    }

    private void Died()
    {
        Destroy(gameObject);
    }
}
