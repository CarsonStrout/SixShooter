using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{

    [SerializeField] private int upgradeIndex;
    [SerializeField] private UpgradeSlotMachine upgradeSlotMachine;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            upgradeSlotMachine.SelectUpgrade(upgradeIndex);
        }
    }
}
