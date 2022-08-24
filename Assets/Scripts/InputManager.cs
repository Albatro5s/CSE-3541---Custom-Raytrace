using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CameraRotator rotator;

    private CSE3541Inputs inputScheme;
    private CSE3541Inputs.PlayerActions input;

    private void Awake()
    {
        inputScheme = new CSE3541Inputs();

        inputScheme.Player.Jump.performed += _ => rotator.OnRotatorTogglePressed();
    }

    private void OnEnable()
    {
        inputScheme.Enable();
    }

    private void OnDestroy()
    {
        inputScheme.Disable();
    }
}
