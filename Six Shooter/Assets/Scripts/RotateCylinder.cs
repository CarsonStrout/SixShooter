using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCylinder : MonoBehaviour
{
    [SerializeField] private GameObject revolverCylinder;

    public IEnumerator RotateCylinderCoroutine()
    {
        // Rotate 60 degrees on y-axis in 0.25 seconds
        float elapsedTime = 0;
        float duration = 0.25f;
        Quaternion startRotation = revolverCylinder.transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 60, 0); // Local axis rotation

        while (elapsedTime < duration)
        {
            revolverCylinder.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it finishes exactly at the desired rotation
        revolverCylinder.transform.localRotation = endRotation;
    }
}