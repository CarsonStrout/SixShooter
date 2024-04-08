using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 15f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float criticalChance = 0.1f;
    [SerializeField] private GameObject explosionVFX;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
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
                if (Random.Range(0, 100) < criticalChance * 100)
                    collider.transform.parent.GetComponent<EnemyStats>().TakeDamage((int)damage * 2, true);
                else
                    collider.transform.parent.GetComponent<EnemyStats>().TakeDamage((int)damage, false);
            }

            if (collider.tag == "Hat")
            {
                collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                collider.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            }
        }

        Destroy(gameObject);
    }

    private float CalculateDamage(float distance)
    {
        return Mathf.Max(0, baseDamage * (1 - distance / explosionRadius));
    }
}
