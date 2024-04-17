using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeSlotMachine : MonoBehaviour
{
    private GameManager GameManager => GameManager.Instance;
    private BulletManager BulletManager => BulletManager.Instance;

    [Header("References")]
    [SerializeField] private BulletIcons BulletIcons;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private AudioSource spinSound, confettiSound, confirmUpgradeSound;
    [SerializeField] private ParticleSystem confettiParticles;

    [Space(10)]
    [Header("Settings")]
    [SerializeField] private float spinSpeed = 0.01f;

    private bool isSpinning = false;
    private int[] selectedUpgrades;
    private bool beginSlowdown = false;
    private float timer = 0f;

    private UpgradeOptions upgradeOption;

    /// <summary>
    /// Starts the slot machine and spins the slots
    /// </summary>
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

            if (!spinSound.isPlaying)
                spinSound.Play();

            if (timer >= 3f)
            {
                beginSlowdown = true;
                Debug.Log("Begin slowdown");
            }
        }
    }

    /// <summary>
    /// Spins the slots and selects the initial final upgrade options
    /// </summary>
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

    /// <summary>
    /// Randomizes the final upgrade selections to ensure that each upgrade is unique
    /// </summary>
    private void RandomizeUniqueFinalSelections()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        HashSet<int> globalSelections = new HashSet<int>();

        int[] weights = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 5 }; // the lower the weight, the rarer the upgrade

        for (int i = 0; i < slots.Length; i++)
        {
            // weighted list of available selections
            List<int> weightedSelections = new List<int>();
            for (int j = 0; j < weights.Length; j++)
            {
                if (!globalSelections.Contains(j))
                {
                    weightedSelections.AddRange(Enumerable.Repeat(j, weights[j]));
                }
            }

            // shuffles the weighted list
            for (int j = weightedSelections.Count - 1; j > 0; j--)
            {
                int swapIndex = UnityEngine.Random.Range(0, j + 1);
                int temp = weightedSelections[j];
                weightedSelections[j] = weightedSelections[swapIndex];
                weightedSelections[swapIndex] = temp;
            }

            // select the first element from the shuffled list and add it to the global selections
            int selected = weightedSelections[0];
            selectedUpgrades[i] = selected;
            globalSelections.Add(selected);
        }

        // activate the selected upgrades
        for (int i = 0; i < slots.Length; i++)
        {
            foreach (Transform child in slots[i].transform)
            {
                child.gameObject.SetActive(false);
            }
            slots[i].transform.GetChild(selectedUpgrades[i]).gameObject.SetActive(true);
            slots[i].transform.GetChild(selectedUpgrades[i]).GetChild(0).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Enables the selection of the final upgrade options
    /// </summary>
    private void EnableSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.gameObject.GetComponent<BoxCollider>().enabled = true;

            // extra effects for Frontier Justice upgrade
            if (slots[i].transform.GetChild(selectedUpgrades[i]).name == "Frontier Justice Upgrade")
            {
                slots[i].transform.GetChild(selectedUpgrades[i]).GetChild(1).gameObject.SetActive(true);
                slots[i].transform.GetChild(selectedUpgrades[i]).GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            }
        }

        Debug.Log("Upgrade Selection Enabled");
    }

    /// <summary>
    /// Selects the upgrade option and updates the bullet type
    /// </summary>
    /// <param name="slotIndex"></param>
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
    SheriffsBadge,
    PokerCard,
    Dynamite,
    Moonshine,
    Lasso,
    SaloonBrawl,
    Cactus,
    Shotgun,
    WantedPoster,
    FrontierJustice
}
