using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float sprintSpeed = 5;

    [Space(5)]
    [SerializeField] private DynamicMoveProvider dynamicMoveProvider;
    private InputData inputData;

    private void Start()
    {
        inputData = GetComponent<InputData>();
    }

    private void Update()
    {
        if (inputData._leftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool _triggerButtonPressed))
        {
            if (_triggerButtonPressed)
            {
                dynamicMoveProvider.moveSpeed = sprintSpeed;
            }
            else
                dynamicMoveProvider.moveSpeed = movementSpeed;
        }
    }
}