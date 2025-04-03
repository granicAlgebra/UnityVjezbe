using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generička (abstraktna) klasa za pooling objekata.
/// Sadrži implementaciju Singleton patterna kako bi se omogućio globalni pristup instanci.
/// Svaka izvedena klasa (npr. SFXManager ili VFXManager) imat će svoju vlastitu Singleton instancu.
/// </summary>
public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    // Dictionary koji mapira prefab na red (Queue) njegovih instanci.
    private Dictionary<T, Queue<T>> _poolDict = new Dictionary<T, Queue<T>>();

    /// <summary>
    /// Vraća instancu objekta iz poola.
    /// Ako pool ne sadrži dostupnu instancu, stvara novu instancu na temelju danog prefaba.
    /// </summary>
    /// <param name="prefab">Prefab koji se koristi za instanciranje objekta</param>
    /// <param name="position">Pozicija na kojoj se objekt postavlja</param>
    /// <param name="rotation">Rotacija objekta</param>
    /// <returns>Instancu objekta tipa T</returns>
    public T GetFromPool(T prefab, Vector3 position, Quaternion rotation)
    {
        // Ako dictionary ne sadrži ključ za dani prefab, kreiraj novi Queue
        if (!_poolDict.ContainsKey(prefab))
            _poolDict[prefab] = new Queue<T>();

        // Dohvati Queue za dati prefab
        Queue<T> pool = _poolDict[prefab];
        T instance;

        // Ako pool sadrži barem jednu instancu, izvadi je iz reda
        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
        }
        else
        {
            // Inače, instanciraj novu instancu prefaba i dodaj je u hijerarhiju ovog objekta
            instance = Instantiate(prefab, transform);
        }

        // Postavi poziciju i rotaciju instance te je aktiviraj
        instance.transform.SetPositionAndRotation(position, rotation);
        instance.gameObject.SetActive(true);
        return instance;
    }

    /// <summary>
    /// Vraća instancu objekta natrag u pool.
    /// Deaktivira instancu i dodaje je u odgovarajući Queue, čime postaje dostupna za ponovno korištenje.
    /// </summary>
    /// <param name="prefab">Prefab s kojim je instanca povezana</param>
    /// <param name="instance">Instanca objekta koja se vraća u pool</param>
    public void ReturnToPool(T prefab, T instance)
    {
        // Deaktiviraj objekt prije vraćanja u pool
        instance.gameObject.SetActive(false);
        // Dodaj instancu natrag u Queue za dani prefab
        _poolDict[prefab].Enqueue(instance);
    }
}