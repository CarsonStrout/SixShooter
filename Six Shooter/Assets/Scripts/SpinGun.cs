using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SpinGun : MonoBehaviour
{
    private InputData _inputData;
    private ShootWeapon shootWeapon;
    [SerializeField] private GameObject revolver;
    public bool isSpinning = false;
    [SerializeField] private AudioSource spinWhoosh;
    // Threshold for detecting fast upward motion
    float upwardVelocityThreshold = 0.5f;

    // Threshold for detecting fast downward motion
    float downwardVelocityThreshold = -0.5f;
    private enum SpinDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    private SpinDirection spinDirection;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
        shootWeapon = GetComponent<ShootWeapon>();
    }

    private void Update()
    {
        if (!shootWeapon.isReloading)
        {
            // check if right grip button is pressed
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool _gripButtonPressed))
            {
                if (_gripButtonPressed)
                {
                    if (!isSpinning)
                    {
                        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
                        {
                            // Get the y-component of the velocity
                            float upwardVelocity = velocity.y;

                            // Get the x-component of the velocity
                            float horizontalVelocity = velocity.x;

                            // Check if the upward velocity exceeds the threshold
                            if (upwardVelocity > upwardVelocityThreshold)
                            {
                                isSpinning = true;
                                spinDirection = SpinDirection.Up;
                            }

                            // Check if the downward velocity exceeds the threshold
                            if (upwardVelocity < downwardVelocityThreshold)
                            {
                                isSpinning = true;
                                spinDirection = SpinDirection.Down;
                            }

                            // Check if the horizontal velocity exceeds the threshold
                            if (horizontalVelocity > upwardVelocityThreshold)
                            {
                                isSpinning = true;
                                spinDirection = SpinDirection.Right;
                            }

                            // Check if the horizontal velocity exceeds the threshold
                            if (horizontalVelocity < downwardVelocityThreshold)
                            {
                                isSpinning = true;
                                spinDirection = SpinDirection.Left;
                            }
                        }
                    }
                    else
                    {
                        if (!spinWhoosh.isPlaying)
                        {
                            spinWhoosh.pitch = Random.Range(1.2f, 1.5f);
                            spinWhoosh.Play();
                        }

                        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
                        {
                            // Get the y-component of the velocity
                            float upwardVelocity = velocity.y;

                            // Get the x-component of the velocity
                            float horizontalVelocity = velocity.x;

                            // Check if the upward velocity exceeds the threshold
                            if (upwardVelocity > upwardVelocityThreshold)
                            {
                                // revolver.transform.localRotation = Quaternion.identity;
                                spinDirection = SpinDirection.Up;
                            }

                            // Check if the downward velocity exceeds the threshold
                            if (upwardVelocity < downwardVelocityThreshold)
                            {
                                // revolver.transform.localRotation = Quaternion.identity;
                                spinDirection = SpinDirection.Down;
                            }

                            // Check if the horizontal velocity exceeds the threshold
                            if (horizontalVelocity > upwardVelocityThreshold)
                            {
                                // revolver.transform.localRotation = Quaternion.identity;
                                spinDirection = SpinDirection.Right;
                            }

                            // Check if the horizontal velocity exceeds the threshold
                            if (horizontalVelocity < downwardVelocityThreshold)
                            {
                                // revolver.transform.localRotation = Quaternion.identity;
                                spinDirection = SpinDirection.Left;
                            }
                        }

                        if (spinDirection == SpinDirection.Up)
                        {
                            revolver.gameObject.transform.Rotate(-360 * 6f * Time.deltaTime, 0, 0);
                        }
                        else if (spinDirection == SpinDirection.Down)
                        {
                            revolver.gameObject.transform.Rotate(360 * 6f * Time.deltaTime, 0, 0);
                        }
                        else if (spinDirection == SpinDirection.Left)
                        {
                            revolver.gameObject.transform.Rotate(0, -360 * 6f * Time.deltaTime, 0);
                        }
                        else if (spinDirection == SpinDirection.Right)
                        {
                            revolver.gameObject.transform.Rotate(0, 360 * 6f * Time.deltaTime, 0);
                        }
                    }
                }
                else
                {
                    isSpinning = false;
                    revolver.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}
