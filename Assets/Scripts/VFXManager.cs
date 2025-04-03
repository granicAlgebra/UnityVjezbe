using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// VFXManager nasljeđuje ObjectPool kako bi upravljao poolom za ParticleSystem komponente.
/// Ova izvedena klasa implementira vlastiti Singleton kako bi omogućila globalni pristup putem VFXManager.Instance.
/// Koristi se za reproduciranje vizualnih efekata (VFX) s optimiziranim poolanjem instance.
/// </summary>
public class VFXManager : ObjectPool<ParticleSystem>
{
    /// <summary>
    /// Jedinstvena (statička) instanca VFXManagera.
    /// Omogućava pristup metodama ove klase globalno, npr. VFXManager.Instance.PlayVFX(...).
    /// </summary>
    public static VFXManager Instance { get; private set; }

    /// <summary>
    /// Awake metoda se poziva prilikom inicijalizacije komponente.
    /// Postavlja Singleton instancu te uništava duplicirane instance ako ih ima.
    /// </summary>
    private void Awake()
    {
        // Provjera ako već postoji instanca
        if (Instance != null)
        {
            // Ako postoji druga instanca, uništi ovu komponentu kako bi se osiguralo da postoji samo jedan VFXManager
            Destroy(this);
        }
        else
        {
            // Postavi trenutnu instancu kao Singleton
            Instance = this;
        }
    }

    /// <summary>
    /// Metoda za reproduciranje vizualnog efekta.
    /// Dohvaća ParticleSystem instancu iz poola, postavlja ju na zadanu poziciju i pokreće reprodukciju efekta.
    /// Nakon što efekt završi, pokreće se coroutine za vraćanje instance u pool.
    /// </summary>
    /// <param name="prefab">Prefab ParticleSystema koji se koristi za instanciranje efekta</param>
    /// <param name="position">Pozicija na kojoj će se efekt reproducirati</param>
    public void PlayVFX(ParticleSystem prefab, Vector3 position)
    {
        // Dohvati ParticleSystem iz poola s default rotacijom (Quaternion.identity)
        ParticleSystem ps = GetFromPool(prefab, position, Quaternion.identity);
        // Pokreni reproduciranje efekta
        ps.Play();
        // Pokreni coroutine koji čeka kraj efekta te vraća instancu natrag u pool
        StartCoroutine(ReturnAfterPlay(prefab, ps));
    }

    /// <summary>
    /// Coroutine koja čeka da se vizualni efekt završi, a zatim vraća ParticleSystem natrag u pool.
    /// Vrijeme čekanja je određeno trajanjem glavne animacije i maksimalnim životom čestica (startLifetime).
    /// </summary>
    /// <param name="prefab">Prefab ParticleSystema, potreban za ispravno vraćanje instance u pool</param>
    /// <param name="ps">Instanca ParticleSystema koja reproducira efekt</param>
    /// <returns>IEnumerator potreban za coroutine</returns>
    private IEnumerator ReturnAfterPlay(ParticleSystem prefab, ParticleSystem ps)
    {
        // Čekaj dok efekt ne završi reprodukciju: trajanje ParticleSystema + maksimalno vrijeme života čestica
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        // Nakon isteka vremena, vrati ParticleSystem instancu u pool za ponovno korištenje
        ReturnToPool(prefab, ps);
    }
}