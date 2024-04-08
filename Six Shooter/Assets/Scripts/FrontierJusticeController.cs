using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontierJusticeController : MonoBehaviour
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
            int enemyHealth = collision.transform.parent.GetComponent<EnemyStats>().EnemyHealth;

            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(enemyHealth, true);
        }
        else if (collision.gameObject.tag == "Hat")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Instantiate(ricochetPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }

        Destroy(gameObject);
    }
}
