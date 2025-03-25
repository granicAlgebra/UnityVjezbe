using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SFXManager : ObjectPool<AudioSource>
{
    public static SFXManager Instance { get; private set; }

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

    public void PlaySFX(AudioSource prefab, Vector3 position, AudioClip clip, float volume = 1f)
    {
        AudioSource source = GetFromPool(prefab, position, Quaternion.identity);

        source.clip = clip;
        source.volume = volume;
        source.Play();

        StartCoroutine(ReturnAfterPlay(source, prefab));
    }

    private IEnumerator ReturnAfterPlay(AudioSource source, AudioSource prefab)
    {
        yield return new WaitForSeconds(source.clip.length);
        ReturnToPool(prefab, source);  
    }
}
