using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedPosterController : MonoBehaviour
{
    private GameObject enemy;

    private void Start()
    {
        Destroy(gameObject, 10f);

        enemy = FindClosestEnemy();
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            Vector3 direction = enemy.transform.position - position;
            float distance = direction.sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;

                if (enemy.transform.parent.GetComponent<EnemyStats>().EnemyHealth > 0)
                    closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void LateUpdate()
    {
        // move towards enemy if it exists at a speed of 5
        if (enemy != null)
        {
            // remove force impulse from rigidbody of gameObject
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (enemy.transform.Find("Center") != null)
            {
                transform.LookAt(enemy.transform.Find("Center"));
                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.Find("Center").position, 6 * Time.deltaTime);
            }
            else
            {
                transform.LookAt(enemy.transform);
                transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, 6 * Time.deltaTime);
            }

            if (enemy.transform.parent.GetComponent<EnemyStats>().EnemyHealth <= 0)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent.GetComponent<EnemyStats>().TakeDamage(10, false);
        }

        Destroy(gameObject);
    }
}
