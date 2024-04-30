using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCylinder : MonoBehaviour
{
    [SerializeField] private GameObject revolverCylinder;
    [SerializeField] private GameObject bulletSlots;
    private bool hasReset = false;

    private void Update()
    {
        if (GameManager.Instance.useAmmo && !hasReset)
        {
            bulletSlots.transform.localRotation = Quaternion.Euler(0, 0, 0);
            hasReset = true;
        }
        else if (!GameManager.Instance.useAmmo && hasReset)
        {
            hasReset = false;
        }
    }

    public IEnumerator RotateCylinderCoroutine()
    {
        // Rotate -60 degrees on y-axis in 0.25 seconds
        float elapsedTime = 0;
        float duration = 0.15f;
        Quaternion startRotation = revolverCylinder.transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -60, 0); // Local axis rotation

        Quaternion endCanvasRotation = bulletSlots.transform.localRotation * Quaternion.Euler(0, 0, -60);

        while (elapsedTime < duration)
        {
            revolverCylinder.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            bulletSlots.transform.localRotation = Quaternion.Slerp(bulletSlots.transform.localRotation, endCanvasRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it finishes exactly at the desired rotation
        revolverCylinder.transform.localRotation = endRotation;
    }
}