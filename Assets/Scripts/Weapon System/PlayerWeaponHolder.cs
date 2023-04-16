using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHolder : MonoBehaviour
{
    [SerializeField] private PlayerInput Input;

    Gun currentGun;
    Gun[] guns;

    private void Start()
    {
        guns = GetComponentsInChildren<Gun>(true);
        Input.Fire1Event += OnFire1;
        EquipGun(0);
    }

    private void EquipGun(int slot)
    {
        foreach (Gun gun in guns)
        {
            gun.gameObject.SetActive(false);
        }
        guns[slot].gameObject.SetActive(true);
        currentGun = guns[slot];
    }

    private void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentGun.StartShooting();
        }
        else if (context.canceled)
        {
            currentGun.StopShooting();
        }
    }
}
