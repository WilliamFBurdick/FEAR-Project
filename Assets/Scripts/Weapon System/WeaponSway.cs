using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private PlayerInput Input;

    [Header("Weapon Sway")]
    [SerializeField] private float weaponSwayAmount;
    [SerializeField] private float swaySmooth;

    private void Update()
    {
        Vector2 sway = Input.LookValue * weaponSwayAmount * Time.deltaTime;

        Quaternion rotX = Quaternion.AngleAxis(-sway.y, Vector3.right);
        Quaternion rotY = Quaternion.AngleAxis(sway.x, Vector3.up);

        Quaternion targetRot = rotX * rotY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, swaySmooth * Time.deltaTime);
    }
}
