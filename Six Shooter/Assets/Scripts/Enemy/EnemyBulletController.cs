using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int randDamage = Random.Range(2, 5);
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(randDamage);
            Destroy(gameObject);
        }
    }
}
