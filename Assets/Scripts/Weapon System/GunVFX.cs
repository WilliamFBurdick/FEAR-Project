using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GunVFX : MonoBehaviour
{
    [Header("Muzzle VFX")]
    [SerializeField] private GameObject muzzleVFX_planar;
    [SerializeField] private float muzzleVFX_duration;
    [SerializeField] private ParticleSystem muzzleVFX_particle;

    [Header("Impact VFX")]
    [SerializeField] private ParticleSystem impactVFX;

    [Header("Tracer Rounds")]
    [SerializeField] private TrailRenderer tracerTrail;
    [SerializeField] private float tracerMissDistance = 100f;
    [SerializeField] private float tracerSpeed = 100f;
    [SerializeField] private float tracerDuration = 1f;
    private ObjectPool<TrailRenderer> tracerPool;

    private void Start()
    {
        muzzleVFX_planar?.SetActive(false);
        tracerPool = new ObjectPool<TrailRenderer>(SpawnTracer);
    }

    public void PlayVFX(Vector3 startPoint, Vector3 endPoint, RaycastHit hit = new RaycastHit())
    {
        PlayMuzzleVFX();
        StartCoroutine(PlayTrailVFX(startPoint, endPoint, hit));
    }

    void PlayMuzzleVFX()
    {
        if (muzzleVFX_planar != null)
        {
            muzzleVFX_planar.SetActive(true);
            Invoke("DeactivateMuzzleVFX", muzzleVFX_duration);
        }
        
        if (muzzleVFX_particle != null)
        {
            muzzleVFX_particle.Play();
        }
    }

    void DeactivateMuzzleVFX()
    {
        muzzleVFX_planar.SetActive(false);
    }

    IEnumerator PlayTrailVFX(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = tracerPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;
        yield return null;

        instance.emitting = true;

        float distance = Vector2.Distance(startPoint, endPoint);
        float remainingDistance = distance;
        while(remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= tracerSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            PlayImpactVFX(hit);
        }

        yield return new WaitForSeconds(tracerDuration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        tracerPool.Release(instance);
    }

    private TrailRenderer SpawnTracer()
    {
        TrailRenderer instance = Instantiate(tracerTrail);

        instance.emitting = false;

        return instance;
    }

    void PlayImpactVFX(RaycastHit Hit)
    {
        if (impactVFX != null)
        {
            Instantiate(impactVFX, Hit.point, Quaternion.LookRotation(Hit.normal));
        }
    }
}
