using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;
    [SerializeField] private GameObject cardVFX;
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private int minDamage = 2;
    [SerializeField] private int maxDamage = 12;
    [SerializeField] private float criticalChance = 0.2f;
    private bool isCrit;

    private void Start()
    {
        Destroy(gameObject, 5f);

        int randomCard = Random.Range(0, cards.Length);
        cards[randomCard].SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int damage = Random.Range(minDamage, maxDamage);
            int totalDamage = damage;
            if (Random.Range(0, 100) < criticalChance * 100)
            {
                isCrit = true;
                totalDamage = damage * 2;
            }
            else
            {
                isCrit = false;
            }

            GameObject blood = Instantiate(bloodVFX, gameObject.transform.position, gameObject.transform.rotation);

            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(totalDamage, isCrit);
        }
        else if (collision.gameObject.tag == "Hat")
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            collision.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }

        GameObject cards = Instantiate(cardVFX, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(cards, 5f);

        Destroy(gameObject);
    }
}
