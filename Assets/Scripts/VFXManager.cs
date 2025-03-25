using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : ObjectPool<ParticleSystem>
{
    public static VFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayVFX(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem ps = GetFromPool(prefab, position, Quaternion.identity);
        ps.Play();
        StartCoroutine(ReturnAfterPlay(prefab, ps));
    }

    private IEnumerator ReturnAfterPlay(ParticleSystem prefab, ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        ReturnToPool(prefab, ps);
    }
}
