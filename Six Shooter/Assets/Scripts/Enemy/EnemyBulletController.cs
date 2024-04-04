using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] private AudioSource ricochetPrefab;
    [SerializeField] private int damage = 2;
    [SerializeField] private int critDamage = 5;
    [SerializeField] private float critChance = 0.2f;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int totalDamage = Random.Range(0, 100) < critChance * 100 ? critDamage : damage;
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(totalDamage);
        }
        else
            Instantiate(ricochetPrefab, gameObject.transform);

        Destroy(gameObject);
    }
}
