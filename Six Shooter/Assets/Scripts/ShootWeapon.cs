using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;
using Unity.Mathematics;
using UnityEngine.XR.Interaction.Toolkit;

public class ShootWeapon : MonoBehaviour
{
    private BulletManager bulletManager => BulletManager.Instance;
    [SerializeField] private BulletIcons bulletIcons;

    [Header("References")]
    [SerializeField] private GameObject _launchPosition;
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private GameObject revolver;
    [SerializeField] private AudioSource shootSound, frontierShootSound, revolverSpinSound, dryFireSound;
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
    [SerializeField]
    private float regularBulletSpeed = 25f, sheriffsBadgeSpeed = 25f, pokerCardSpeed = 25f, dynamiteSpeed = 10f, moonshineSpeed = 10f,
        lassoSpeed = 15f, saloonBrawlSpeed = 15f, cactusSpeed = 25f, shotgunSpeed = 25f, wantedPosterSpeed = 15f, frontierJusticeSpeed = 35f;

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

                                if (GameManager.Instance.useAmmo)
                                    ammoCount--;

                                Fire();

                                if (shotVibIntensity > 0)
                                    controller.SendHapticImpulse(shotVibIntensity, shotVibDuration);

                                numShots++;

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
                // trigger button detection again
                if (_inputData._rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool _triggerButtonPressed))
                {
                    if (_triggerButtonPressed)
                    {
                        if (!triggerDown)
                        {
                            triggerDown = true;
                            dryFireSound.Play();
                            StartCoroutine(Pause());
                        }
                    }
                    else
                        triggerDown = false;
                }

                // Threshold for detecting fast upward motion
                float upwardVelocityThreshold = 1.5f;

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

            // Light up ammo from last to first based on currentAmmo
            for (int i = bulletManager.isBulletLoaded.Length - 1; i >= 0; i--)
            {
                if (bulletManager.isBulletLoaded.Length - i <= currentAmmo)
                    bulletIcons.LightUpIcon(i);
                else
                    bulletIcons.BlackOutIcon(i);
            }

            if (timer > reloadTime)
            {
                isReloading = false;
                ammoCount = maxAmmo;
                timer = 0;
                revolver.transform.localRotation = Quaternion.identity;

                for (int i = 0; i < bulletManager.isBulletLoaded.Length; i++)
                {
                    bulletManager.isBulletLoaded[i] = true;
                }
            }
        }
    }

    private void Fire()
    {
        if (GameManager.Instance.useAmmo && !GameManager.Instance.State.Equals(GameState.Menu) && !GameManager.Instance.State.Equals(GameState.TargetPractice))
        {
            if (!bulletManager.IsBulletLoaded())
                return;

            BulletType currentBulletType = bulletManager.GetBulletType(bulletManager.currentBulletSlot);
            GameObject bulletPrefab = bulletPrefabs[(int)currentBulletType];

            GameObject bullet = Instantiate(bulletPrefab) as GameObject;
            bullet.SetActive(true);
            bullet.transform.position = _launchPosition.transform.position;
            bullet.transform.rotation = _launchPosition.transform.rotation;

            float bulletSpeed = 0;
            switch (currentBulletType)
            {
                case BulletType.Regular:
                    bulletSpeed = regularBulletSpeed;
                    break;
                case BulletType.SheriffsBadge:
                    bulletSpeed = sheriffsBadgeSpeed;
                    break;
                case BulletType.PokerCard:
                    bulletSpeed = pokerCardSpeed;
                    break;
                case BulletType.Dynamite:
                    bulletSpeed = dynamiteSpeed;
                    break;
                case BulletType.Moonshine:
                    bulletSpeed = moonshineSpeed;
                    break;
                case BulletType.Lasso:
                    bulletSpeed = lassoSpeed;
                    break;
                case BulletType.SaloonBrawl:
                    bulletSpeed = saloonBrawlSpeed;
                    break;
                case BulletType.Cactus:
                    bulletSpeed = cactusSpeed;
                    break;
                case BulletType.Shotgun:
                    bulletSpeed = shotgunSpeed;
                    break;
                case BulletType.WantedPoster:
                    bulletSpeed = wantedPosterSpeed;
                    break;
                case BulletType.FrontierJustice:
                    bulletSpeed = frontierJusticeSpeed;
                    break;
            }

            if (bulletManager.GetBulletType(bulletManager.currentBulletSlot) == BulletType.FrontierJustice)
                frontierShootSound.Play();
            else
                shootSound.Play();

            if (bulletManager.GetBulletType(bulletManager.currentBulletSlot) == BulletType.Shotgun)
            {
                Destroy(bullet);

                // Define the spread angle (e.g., 5 degrees)
                float spreadAngle = 5f;

                // Number of additional bullets per side
                int additionalBulletsPerSide = 2;

                for (int i = -additionalBulletsPerSide; i <= additionalBulletsPerSide; i++)
                {
                    GameObject extraBullet = Instantiate(bulletPrefabs[(int)currentBulletType]) as GameObject;
                    extraBullet.SetActive(true);
                    extraBullet.transform.position = _launchPosition.transform.position;
                    extraBullet.transform.rotation = _launchPosition.transform.rotation;

                    // Calculate the rotation for each bullet in the spread
                    float rotationAngle = i * spreadAngle;
                    Quaternion spreadRotation = Quaternion.Euler(0, rotationAngle, 0);

                    // Apply the rotation to the bullet's direction
                    Vector3 spreadDirection = spreadRotation * _launchPosition.transform.forward;

                    extraBullet.GetComponent<Rigidbody>().AddForce(spreadDirection * shotgunSpeed, ForceMode.Impulse);
                }
            }

            bullet.GetComponent<Rigidbody>().AddForce(_launchPosition.transform.forward * bulletSpeed, ForceMode.Impulse);

            int bulletIndex = bulletManager.currentBulletSlot;
            bulletManager.UseBullet();

            bulletIcons.BlackOutIcon(bulletIndex);
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefabs[0]) as GameObject;
            bullet.SetActive(true);
            bullet.transform.position = _launchPosition.transform.position;
            bullet.transform.rotation = _launchPosition.transform.rotation;

            shootSound.Play();

            if (ammoCount < maxAmmo)
            {
                int iconIndex = maxAmmo - 1 - ammoCount;

                // Check if the iconIndex is within the valid range
                if (iconIndex >= 0 && iconIndex < bulletIcons.bulletIcons.Length)
                {
                    bulletIcons.BlackOutIcon(iconIndex);
                }
            }

            bullet.GetComponent<Rigidbody>().AddForce(_launchPosition.transform.forward * 25, ForceMode.Impulse);
        }
    }

    private void Reload()
    {
        isReloading = true;

        revolverSpinSound.Play();

        if (reloadVibIntensity > 0)
            controller.SendHapticImpulse(reloadVibIntensity, reloadVibDuration);
    }


    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.5f);
        __shootingPaused = false;
    }
}
