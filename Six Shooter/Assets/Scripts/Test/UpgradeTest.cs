using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTest : MonoBehaviour
{
    [SerializeField] private UpgradeSlotMachine upgradeSlotMachine;

    private void Start()
    {
        upgradeSlotMachine.StartSlotMachine();
    }
}
