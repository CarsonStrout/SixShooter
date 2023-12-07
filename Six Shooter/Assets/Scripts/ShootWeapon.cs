using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using Unity.Mathematics;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _launchPosition;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject revolver;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private ParticleSystem gunParticles;
    [SerializeField] private XRBaseController controller;

    [Space(5)]
    [Header("Stats")]
    [SerializeField] private int maxAmmo = 6;
    private int ammoCount;
    private float timer;
    public float reloadTime = 1f;
    private bool isReloading = false;

    private bool __shootingPaused = false;
    private bool triggerDown;
    private InputData _inputData;
    [SerializeField] private float bulletSpeed = 25f;

    [Space(5)]
    [Header("Controller Vibration")]
    [Range(0, 1)]
    [SerializeField] private float shotVibIntensity;
    [SerializeField] private float shotVibDuration;
    [Range(0, 1)]
    [SerializeField] private float reloadVibIntensity;
    [SerializeField] private float reloadVibDuration;

    [HideInInspector] public int numShots = 0;

    private void Start()
    {
        _inputData = GetComponent<InputData>();
        ammoCount = maxAmmo;
    }

    private void Update()
    {
        ammoText.text = ammoCount + " / " + maxAmmo;

        if (!isReloading)
        {
            if (ammoCount > 0)
            {
                if (!__shootingPaused)
                {
                    if (_inputData._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool _triggerButtonPressed))
                    {
                        if (_triggerButtonPressed)
                        {
                            if (!triggerDown)
                            {
                                triggerDown = true;
                                __shootingPaused = true;
                                Fire();
                                if (shotVibIntensity > 0)
                                    controller.SendHapticImpulse(shotVibIntensity, shotVibDuration);
                                ammoCount--;
                                numShots++;
                                shootSound.Play();
                                gunParticles.Play();
                                StartCoroutine(Pause());
                            }
                        }
                        else
                            triggerDown = false;
                    }
                }
            }
            else
            {
                // Threshold for detecting fast upward motion
                float upwardVelocityThreshold = 2.0f;

                if (_inputData._rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
                {
                    // Get the y-component of the velocity
                    float upwardVelocity = velocity.y;

                    // Check if the upward velocity exceeds the threshold
                    if (upwardVelocity > upwardVelocityThreshold)
                    {
                        Reload();
                    }
                }
            }
        }
        else
        {
            revolver.gameObject.transform.Rotate(-360 * 6f * Time.deltaTime, 0, 0);
            timer += Time.deltaTime;

            // Calculate reload percentage
            float reloadPercentage = Mathf.Clamp01(timer / reloadTime);

            // Calculate current ammo based on the reload percentage
            int currentAmmo = Mathf.RoundToInt(reloadPercentage * maxAmmo);
            ammoText.text = currentAmmo + " / " + maxAmmo;

            if (timer > reloadTime)
            {
                isReloading = false;
                ammoCount = maxAmmo;
                timer = 0;
                revolver.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(_bulletPrefab) as GameObject;
        bullet.SetActive(true);
        bullet.transform.position = _launchPosition.transform.position;
        bullet.transform.rotation = _launchPosition.transform.rotation;

        bullet.GetComponent<Rigidbody>().AddForce(_launchPosition.transform.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void Reload()
    {
        isReloading = true;
        if (reloadVibIntensity > 0)
            controller.SendHapticImpulse(reloadVibIntensity, reloadVibDuration);
    }


    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.5f);
        __shootingPaused = false;
    }
}
