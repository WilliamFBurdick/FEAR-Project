using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerStateMachine : StateMachine
{
    [field: Header("Variables")]
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float JumpHeight { get; private set; }
    [field: SerializeField][field: Range(0, 1)] public float AirControl { get; private set; }
    [field: SerializeField] public float LookSensitivity { get; private set; }
    [field: SerializeField] public float DefaultCameraXBounds { get; private set; }

    [field: Header("Physics")]
    [field: SerializeField] public float GravityStrength { get; private set; }

    [field: Header("Components")]
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Transform Eyes { get; private set; }
    [HideInInspector] public float EyesXRot;
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public PlayerInput Input { get; private set; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EyesXRot = Eyes.localEulerAngles.x;
        ChangeState(new PlayerWalkState(this));
    }
}
