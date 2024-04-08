using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : MonoBehaviour
{
    [SerializeField] private GameObject bloodVFX;
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
            collision.transform.parent.GetComponent<EnemyPricked>().isPricked = true;

            GameObject blood = Instantiate(bloodVFX, gameObject.transform.position, gameObject.transform.rotation);
        }
        else if (collision.gameObject.tag == "Hat")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }

        Destroy(gameObject);
    }
}
