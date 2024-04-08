using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [SerializeField] private GameObject upgradeSlotMachine;

    private BulletType[] bulletType = new BulletType[6];

    [HideInInspector] public int currentBulletSlot = 0;
    [HideInInspector] public bool[] isBulletLoaded = new bool[6];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TargetPractice")
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (upgradeSlotMachine == null)
            upgradeSlotMachine = GameObject.Find("UpgradeSlotMachine");

        ResetBulletTypes();
        for (int i = 0; i < isBulletLoaded.Length; i++)
        {
            isBulletLoaded[i] = true; // Initially, all bullets are loaded
        }
    }

    public bool IsBulletLoaded()
    {
        return isBulletLoaded[currentBulletSlot];
    }

    public void UseBullet()
    {
        if (isBulletLoaded[currentBulletSlot])
        {
            isBulletLoaded[currentBulletSlot] = false;
            currentBulletSlot = (currentBulletSlot + 1) % bulletType.Length;
        }
    }

    public BulletType GetBulletType(int index)
    {
        return bulletType[index];
    }

    public void SetBulletType(int index, BulletType type)
    {
        bulletType[index] = type;
    }

    public void ResetBulletTypes()
    {
        bulletType[0] = BulletType.Regular;
        bulletType[1] = BulletType.Regular;
        bulletType[2] = BulletType.Regular;
        bulletType[3] = BulletType.Regular;
        bulletType[4] = BulletType.Regular;
        bulletType[5] = BulletType.Regular;
    }
}

public enum BulletType
{
    Regular,
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