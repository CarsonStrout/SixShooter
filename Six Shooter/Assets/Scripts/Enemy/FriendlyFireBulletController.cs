using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFireBulletController : MonoBehaviour
{
    [SerializeField] private AudioSource ricochetPrefab;
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private int damage = 2;
    [SerializeField] private int critDamage = 5;
    [SerializeField] private float critChance = 0.05f;

    private void Start()
    {
        Destroy(gameObject, 5f);

        int activeLevel = (int)GameManager.Instance.activeLevel;

        switch (activeLevel)
        {
            case 0:
                damage = 2;
                critDamage = 5;
                break;
            case 1:
                damage = 3;
                critDamage = 6;
                break;
            case 2:
                damage = 4;
                critDamage = 8;
                break;
            case 3:
                damage = 5;
                critDamage = 10;
                break;
            case 4:
                damage = 6;
                critDamage = 12;
                break;
            case 5:
                damage = 7;
                critDamage = 14;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int totalDamage = Random.Range(0, 100) < critChance * 100 ? critDamage : damage;
            collision.gameObject.transform.parent.GetComponent<EnemyStats>().TakeDamage(totalDamage, false);

            GameObject blood = Instantiate(bloodVFX, gameObject.transform.position, gameObject.transform.rotation);
        }
        else
            Instantiate(ricochetPrefab, gameObject.transform);

        Destroy(gameObject);
    }
}
