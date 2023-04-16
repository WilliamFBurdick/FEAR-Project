using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public Vector2 LookValue { get; private set; }
    public bool IsPrimaryFiring { get; private set; }
    public bool IsSecondaryFiring { get; private set; }

    public event Action JumpEvent;
    public event Action CrouchEvent;

    public event Action<InputAction.CallbackContext> Fire1Event;
    public event Action<InputAction.CallbackContext> Fire2Event;

    private PlayerInputActions controls;

    private void Start()
    {
        controls = new PlayerInputActions();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        CrouchEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookValue = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        Fire1Event?.Invoke(context);
    }

    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        Fire2Event?.Invoke(context);
    }
}
