using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject ricochetPrefab;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int randDamage = Random.Range(5, 8);
            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(randDamage);
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
