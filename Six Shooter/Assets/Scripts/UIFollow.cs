using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private float distanceFromPlayer = 2.0f;
    [SerializeField] private float followSpeed = 5.0f;

    private Vector3 targetPosition;

    void Update()
    {
        // Calculate the target position in front of the player
        targetPosition = playerHead.position + playerHead.forward * distanceFromPlayer;

        // Interpolate position for smooth following
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Ensure the UI faces away from the player to appear correctly oriented
        Vector3 directionToPlayer = transform.position - playerHead.position;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}
