using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteController : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 15f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private GameObject explosionVFX;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            float damage = CalculateDamage(distance);

            if (collider.tag == "Enemy")
            {
                collider.transform.parent.GetComponent<EnemyStats>().TakeDamage((int)damage, false);
            }
        }

        Destroy(gameObject);
    }

    private float CalculateDamage(float distance)
    {
        return Mathf.Max(0, baseDamage * (1 - distance / explosionRadius));
    }
}
