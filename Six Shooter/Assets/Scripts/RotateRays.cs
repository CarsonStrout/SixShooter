using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRays : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
