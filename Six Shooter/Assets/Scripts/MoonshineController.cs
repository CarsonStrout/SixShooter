using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonshineController : MonoBehaviour
{
    [SerializeField] private float breakRadius = 10f;
    [SerializeField] private int baseDamage = 3;
    [SerializeField] private GameObject breakVFX;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Break();
    }

    private void Break()
    {
        Instantiate(breakVFX, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, breakRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                collider.transform.parent.GetComponent<EnemyStats>().TakeDamage(baseDamage, false);

                if (collider.transform.parent.GetComponent<EnemyStats>().EnemyHealth > 0)
                    collider.transform.parent.GetComponent<EnemyAI>().currentState = EnemyAI.State.Drunk;
            }
        }

        Destroy(gameObject);
    }
}
