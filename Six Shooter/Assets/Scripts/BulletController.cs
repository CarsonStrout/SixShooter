using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject ricochetPrefab;
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private int baseDamage = 5;
    [SerializeField] private int criticalDamage = 10;
    [SerializeField] private float criticalChance = 0.05f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (Random.Range(0, 100) < criticalChance * 100)
            {
                collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(criticalDamage, true);
            }
            else
            {
                collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(baseDamage, false);
            }

            GameObject blood = Instantiate(bloodVFX, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (collision.gameObject.tag == "Hat")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }
        else if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<TargetManager>().TargetHit();
            collision.gameObject.GetComponent<TargetManager>().SpawnTarget();
        }
        else
        {
            Instantiate(ricochetPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }

        Destroy(gameObject);
    }
}
