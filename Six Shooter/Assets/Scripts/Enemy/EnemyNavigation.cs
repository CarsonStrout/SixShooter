using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private List<Transform> movePositions = new List<Transform>();
    private NavMeshAgent m_Agent;
    private Transform CurrentDestination;

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        CurrentDestination = RandomDestination();
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, CurrentDestination.position);
        if (dist < 1.2f)
        {
            CurrentDestination = RandomDestination();
        }

        m_Agent.destination = CurrentDestination.position;
    }

    private Transform RandomDestination()
    {
        if (movePositions.Count > 0)
        {
            int rd = Random.Range(0, movePositions.Count);
            return movePositions[rd];
        }

        return null;
    }
}
