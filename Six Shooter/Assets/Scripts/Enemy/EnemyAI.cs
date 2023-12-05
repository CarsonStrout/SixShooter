using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Spawn,
        Chase,
        Attack,
        Retreat
    }

    public State currentState;
    private Transform playerTransform;
    public float chaseSpeed = 3f;
    public float retreatSpeed = 5f;
    public float attackRange = 10f;
    public float minimumDistance = 5f; // Minimum distance to trigger retreat

    private NavMeshAgent agent;
    private float fieldOfViewAngle = 90f;

    // Shooting
    [SerializeField] private GameObject launchPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject revolver;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private ParticleSystem gunParticles;

    [SerializeField] private int maxAmmo = 6;
    private int ammoCount;
    private bool isReloading = false;
    private float reloadTime = 1f;
    private float timer;
    private bool shootingPaused = false;
    [SerializeField] private ParticleSystem spawnParticles;
    private bool canSpawn = true;

    [Space(5)]
    [SerializeField] private GameObject[] components;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Spawn;
        ammoCount = maxAmmo;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player is tagged correctly.");
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Spawn:
                SpawnUpdate();
                break;
            case State.Chase:
                ChaseUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
            case State.Retreat:
                RetreatUpdate();
                break;
        }
    }

    void SpawnUpdate()
    {
        if (canSpawn)
        {
            spawnParticles.Play();
        }

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        canSpawn = false;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < components.Length; i++)
        {
            components[i].SetActive(true);
        }

        currentState = State.Chase;
    }

    void ChaseUpdate()
    {
        agent.isStopped = false;
        agent.destination = playerTransform.position;
        agent.speed = chaseSpeed;

        if ((Vector3.Distance(transform.position, playerTransform.position) < attackRange) || !CanSeePlayer())
        {
            currentState = State.Attack;
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) < minimumDistance)
        {
            currentState = State.Retreat;
        }
    }

    void AttackUpdate()
    {
        if (CanSeePlayer())
        {
            agent.isStopped = true;
            transform.LookAt(playerTransform.position); // Enemy looks at the player

            // Rotate the gun towards the player
            Vector3 directionToPlayer = playerTransform.position - revolver.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            revolver.transform.rotation = Quaternion.Slerp(revolver.transform.rotation, lookRotation, Time.deltaTime * 5f);

            if (!isReloading)
            {
                if (ammoCount > 0)
                {
                    if (!shootingPaused)
                    {
                        shootingPaused = true;
                        Fire();
                        ammoCount--;
                        shootSound.Play();
                        gunParticles.Play();
                        StartCoroutine(Pause());
                    }
                }
                else
                {
                    Reload();
                }
            }
            else
            {
                revolver.gameObject.transform.Rotate(-360 * 6f * Time.deltaTime, 0, 0);
                timer += Time.deltaTime;

                if (timer > reloadTime)
                {
                    isReloading = false;
                    ammoCount = maxAmmo;
                    timer = 0;
                    revolver.transform.localRotation = Quaternion.identity;
                }
            }
        }

        if (Vector3.Distance(transform.position, playerTransform.position) < minimumDistance)
        {
            currentState = State.Retreat;
        }
        else if ((Vector3.Distance(transform.position, playerTransform.position) > attackRange) || !CanSeePlayer())
        {
            currentState = State.Chase;
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.SetActive(true);
        bullet.transform.position = launchPosition.transform.position;

        bullet.transform.rotation = launchPosition.transform.rotation;
        bullet.GetComponent<Rigidbody>().AddForce(launchPosition.transform.forward * 10f, ForceMode.Impulse);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(1.5f);
        shootingPaused = false;
    }

    private void Reload()
    {
        isReloading = true;
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        if (angle < fieldOfViewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, directionToPlayer.normalized, out hit))
            {
                if (hit.transform == playerTransform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void RetreatUpdate()
    {
        agent.isStopped = false;
        Vector3 retreatDirection = transform.position - playerTransform.position;
        Vector3 retreatPosition = transform.position + retreatDirection.normalized * retreatSpeed * Time.deltaTime;

        agent.SetDestination(retreatPosition);
        agent.speed = retreatSpeed;

        if (Vector3.Distance(transform.position, playerTransform.position) > minimumDistance &&
            Vector3.Distance(transform.position, playerTransform.position) < attackRange)
        {
            currentState = State.Attack;
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) > attackRange)
        {
            currentState = State.Chase;
        }
    }
}
