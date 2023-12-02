using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class LaserSight : MonoBehaviour
{
    [SerializeField] private LineRenderer laser;
    private InputData _inputData;
    private bool isLaserOn;
    private bool togglePaused = false;


    private void Start()
    {
        _inputData = GetComponent<InputData>();
        isLaserOn = false;
        laser.enabled = false;
    }

    private void Update()
    {
        LaserToggle();
    }

    private void LaserToggle()
    {
        if (!isLaserOn)
            laser.enabled = false;
        else
            laser.enabled = true;

        if (!togglePaused)
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool _primaryPressed))
            {
                if (_primaryPressed)
                {
                    togglePaused = true;
                    isLaserOn = !isLaserOn;
                    StartCoroutine(Pause());
                }
            }
        }

    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.5f);
        togglePaused = false;
    }
}
