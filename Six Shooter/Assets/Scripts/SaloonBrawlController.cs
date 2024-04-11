using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaloonBrawlController : MonoBehaviour
{
    [SerializeField] private int damage = 3;
    [SerializeField] private GameObject breakVFX;
    [SerializeField] private GameObject bloodVFX;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(damage, false);

            GameObject blood = Instantiate(bloodVFX, gameObject.transform.position, gameObject.transform.rotation);

            if (collision.transform.parent.GetComponent<EnemyStats>().EnemyHealth > 0)
                collision.transform.parent.GetComponent<EnemyAI>().currentState = EnemyAI.State.Brawl;
        }
        else if (collision.gameObject.tag == "Hat")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }

        Instantiate(breakVFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
