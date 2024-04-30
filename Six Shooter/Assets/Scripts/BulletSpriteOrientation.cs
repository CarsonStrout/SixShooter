using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpriteOrientation : MonoBehaviour
{
    [SerializeField] private Transform slotsTransform;

    private void Update()
    {
        // Calculate the inverse z-axis rotation from the bulletSlots to keep the bullets upright
        Quaternion inverseZRotation = Quaternion.Inverse(Quaternion.Euler(0, 0, slotsTransform.localEulerAngles.z));

        // Apply the inverse z-rotation to the bullet so it cancels out the z-axis rotation of the bulletSlots
        // and preserves the initial orientation relative to the bulletSlots
        transform.localRotation = inverseZRotation;
    }
}
