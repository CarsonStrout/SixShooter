using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotMachine : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    private BulletManager BulletManager => BulletManager.Instance;
    [SerializeField] private BulletIcons BulletIcons;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private float spinSpeed = 0.01f;
    [SerializeField] private AudioSource spinSound, confettiSound, confirmUpgradeSound;
    [SerializeField] private ParticleSystem confettiParticles;
    private bool isSpinning = false;
    private int[] selectedUpgrades;
    private bool beginSlowdown = false;
    private float timer = 0f;

    private UpgradeOptions upgradeOption;

    public void StartSlotMachine()
    {
        selectedUpgrades = new int[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            selectedUpgrades[i] = UnityEngine.Random.Range(0, slots[i].transform.childCount);
            slots[i].transform.GetChild(selectedUpgrades[i]).gameObject.SetActive(true);
        }

        StartCoroutine(SpinSlots());
    }

    private void Update()
    {
        if (isSpinning && !beginSlowdown)
        {
            timer += Time.deltaTime;
            // Debug.Log(timer);

            if (!spinSound.isPlaying)
                spinSound.Play();

            if (timer >= 3f)
            {
                beginSlowdown = true;
                Debug.Log("Begin slowdown");
            }
        }
    }

    private IEnumerator SpinSlots()
    {
        isSpinning = true;
        while (isSpinning)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].transform.GetChild(selectedUpgrades[i]).gameObject.SetActive(false);
                selectedUpgrades[i] = UnityEngine.Random.Range(0, slots[i].transform.childCount);
                slots[i].transform.GetChild(selectedUpgrades[i]).gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(spinSpeed);

            if (beginSlowdown)
            {
                spinSpeed += 0.01f;
                spinSound.pitch -= 0.01f;

                if (spinSpeed >= 0.25f)
                {
                    isSpinning = false;
                    spinSound.Stop();
                    confettiSound.Play();
                    confettiParticles.Play();
                    RandomizeUniqueFinalSelections();
                }
            }
        }

        EnableSelection();
    }

    private void RandomizeUniqueFinalSelections()
    {
        HashSet<int> usedSelections = new HashSet<int>();
        for (int i = 0; i < slots.Length; i++)
        {
            int selection;
            do
            {
                selection = UnityEngine.Random.Range(0, slots[i].transform.childCount);
            } while (usedSelections.Contains(selection));

            usedSelections.Add(selection);
            selectedUpgrades[i] = selection;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            foreach (Transform child in slots[i].transform)
            {
                child.gameObject.SetActive(false);
            }
            slots[i].transform.GetChild(selectedUpgrades[i]).gameObject.SetActive(true);

            // Enable text for final selection
            slots[i].transform.GetChild(selectedUpgrades[i]).GetChild(0).gameObject.SetActive(true);
        }
    }

    private void EnableSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        Debug.Log("Upgrade Selection Enabled");
    }

    public void SelectUpgrade(int slotIndex)
    {
        upgradeOption = (UpgradeOptions)selectedUpgrades[slotIndex];

        Debug.Log("Selected Upgrade: " + upgradeOption);

        BulletManager.SetBulletType(Convert.ToInt32(GameManager.activeLevel), (BulletType)upgradeOption + 1);
        BulletIcons.UpdateBulletIcons();
        GameManager.UpdateGameState(GameState.WaveSpawner);
    }
}

public enum UpgradeOptions
{
    BigBullet,
    PokerCard,
    Dynamite,
    Moonshine,
    Lasso
}
