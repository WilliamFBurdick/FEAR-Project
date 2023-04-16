using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool AutoTrigger = true;

    [SerializeField] private bool ApplySpread = true;
    [SerializeField] private Vector3 BulletSpreadBounds = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField] private GunVFX gunVFX;
    [SerializeField] private Transform tracerOrigin;

    [SerializeField] private float FireDelay = 0.5f;
    [SerializeField] private LayerMask Mask;

    float LastShotTime;
    bool triggerPressed = false;

    public void StartShooting()
    {
        if (AutoTrigger)
        {
            triggerPressed = true;
            StartCoroutine(ContinueShooting());
        }
        else
        {
            if (Time.time >= LastShotTime + FireDelay && !triggerPressed)
            {
                triggerPressed = true;
                FireShot();
                LastShotTime = Time.time;
            }
        }
    }
    
    public void StopShooting()
    {
        triggerPressed = false;
        if (AutoTrigger) { StopCoroutine(ContinueShooting()); }
    }

    private IEnumerator ContinueShooting()
    {
        float delay = Mathf.Max(LastShotTime + FireDelay - Time.time, 0f);
        yield return new WaitForSeconds(delay);
        while (triggerPressed)
        {
            FireShot();
            LastShotTime = Time.time;
            delay = Mathf.Max(LastShotTime + FireDelay - Time.time, 0f);
            yield return new WaitForSeconds(delay);
        }
    }

    private void FireShot()
    {
        Vector3 direction = GetDirection();

        if (Physics.Raycast(transform.parent.position, direction, out RaycastHit hit, float.MaxValue, Mask))
        {
            gunVFX?.PlayVFX(tracerOrigin.position, hit.point, hit);
            LastShotTime = Time.time;
        }
        else
        {
            gunVFX.PlayVFX(tracerOrigin.position, tracerOrigin.position + direction * 100f);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.parent.forward;

        if (ApplySpread)
        {
            direction +=
                transform.parent.right * Random.Range(-BulletSpreadBounds.x, BulletSpreadBounds.x) +
                transform.parent.up * Random.Range(-BulletSpreadBounds.y, BulletSpreadBounds.y);

            direction.Normalize();
        }

        return direction;
    }
}
