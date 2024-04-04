using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoController : MonoBehaviour
{
    [SerializeField] private int baseDamage = 5;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(baseDamage, false);
            collision.transform.parent.GetComponent<EnemyAI>().chaseSpeed = 0f;
            collision.transform.parent.GetComponent<EnemyAI>().retreatSpeed = 0f;
        }

        Destroy(gameObject);
    }
}
