using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class YeeHaw : MonoBehaviour
{
    private InputData inputData;
    [SerializeField] private AudioSource yeeHaw;
    [SerializeField] private AudioClip yeeHaw1, loudYeeHaw;

    private void Start()
    {
        inputData = GetComponent<InputData>();
    }

    private void Update()
    {
        if (inputData._leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool _buttonPressed))
        {
            if (_buttonPressed)
            {
                if (!yeeHaw.isPlaying)
                {
                    // Randomly play one of the two yee haw sounds
                    if (Random.Range(0, 2) == 0)
                        yeeHaw.clip = yeeHaw1;
                    else
                        yeeHaw.clip = loudYeeHaw;

                    yeeHaw.Play();

                    if (GameManager.Instance.State == GameState.UpgradeSlotMachine || GameManager.Instance.State == GameState.WaveSpawner || GameManager.Instance.State == GameState.CompleteLevel)
                        GameManager.Instance.yeeHaws++;
                }
            }
        }
    }
}
