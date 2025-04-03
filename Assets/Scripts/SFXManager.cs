using System.Collections;
using UnityEngine;

/// <summary>
/// SFXManager nasljeđuje ObjectPool kako bi upravljao poolom za AudioSource komponente.
/// Ova izvedena klasa implementira vlastiti Singleton za globalni pristup reprodukciji zvukova.
/// </summary>
public class SFXManager : ObjectPool<AudioSource>
{
    /// <summary>
    /// Jedinstvena (statička) instanca SFXManagera.
    /// Omogućuje pozivanje: SFXManager.Instance.PlaySFX(...)
    /// </summary>
    public static SFXManager Instance { get; private set; }

    /// <summary>
    /// Ova metoda se poziva čim se komponenta instancira (ili kad se scena učita).
    /// Postavljamo Singleton: ako već postoji instanca koja nije ova, uništimo duplikat.
    /// Inače, postavimo Instance = this.
    /// </summary>
    protected virtual void Awake()
    {
        // Pozovi i bazni Awake, ako želiš (ako ga imaš definiranog u ObjectPool<T>)
        // base.Awake(); 

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Reproducira zvučni efekt dohvaćanjem AudioSource instance iz poola.
    /// </summary>
    /// <param name="prefab">Prefab AudioSourcea koji želimo iskoristiti</param>
    /// <param name="position">Pozicija na kojoj će se reproducirati zvuk</param>
    /// <param name="clip">AudioClip koji sviramo</param>
    /// <param name="volume">Glasnoća (default je 1f)</param>
    public void PlaySFX(AudioSource prefab, Vector3 position, AudioClip clip, float volume = 1f)
    {
        // Dohvati AudioSource iz poola
        AudioSource source = GetFromPool(prefab, position, Quaternion.identity);

        // Postavi clip i volume, pa pusti zvuk
        source.clip = clip;
        source.volume = volume;
        source.Play();

        // Pokreni coroutine koji čeka kraj reprodukcije pa vraća AudioSource u pool
        StartCoroutine(ReturnAfterPlay(source, prefab));
    }

    /// <summary>
    /// Coroutine koja čeka da završi reprodukcija zvuka i onda vraća AudioSource u pool.
    /// </summary>
    private IEnumerator ReturnAfterPlay(AudioSource source, AudioSource prefab)
    {
        yield return new WaitForSeconds(source.clip.length);
        ReturnToPool(prefab, source);
    }
}