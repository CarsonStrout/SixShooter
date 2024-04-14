using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndgameStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textMeshProUGUIs;

    private void OnEnable()
    {
        textMeshProUGUIs[0].text = "Damage Given: " + GameManager.Instance.damageGiven;
        textMeshProUGUIs[1].text = "Damage Taken: " + GameManager.Instance.damageTaken;
        textMeshProUGUIs[2].text = "Total Shots: " + GameManager.Instance.totalShots;
        textMeshProUGUIs[3].text = "Hats Knocked: " + GameManager.Instance.hatsKnocked;
        textMeshProUGUIs[4].text = "Time Spinning: " + (int)GameManager.Instance.timeGunSpun + " seconds";
        textMeshProUGUIs[5].text = "Yee-Haws: " + GameManager.Instance.yeeHaws;
    }
}
