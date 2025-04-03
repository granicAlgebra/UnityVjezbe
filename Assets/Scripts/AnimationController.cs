using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AnimationController služi za upravljanje animacijama entiteta temeljem njegovih fizikalnih svojstava (npr. brzine kretanja, skoka, pada).
/// Ova skripta dohvaća referencu na Animator i Rigidbody komponente te na osnovu njihove stanja ažurira animacije.
/// Također, koristi se Dictionary kako bi se optimiziralo korištenje hashiranih parametara animatora.
/// </summary>
public class AnimationController : MonoBehaviour
{
    // Maksimalna brzina kretanja entiteta, koristi se za normalizaciju brzine u blend treeu animacija.
    [SerializeField] private float _maxMovementSpeed;
    // Vrijednost za zaglađivanje prijelaza (smooth damp) prilikom mijenjanja parametara animacije.
    [SerializeField] private float _smooth;

    // Referenca na Animator komponentu koja upravlja animacijama.
    private Animator _animator;
    // Referenca na Rigidbody komponentu koja omogućuje dohvaćanje fizičkih svojstava entiteta.
    private Rigidbody _rigidBody;

    // Dictionary za spremanje hashiranih vrijednosti parametara animatora. Time se smanjuje potreba za ponovnim izračunom hasha.
    private Dictionary<string, int> _paramHashes = new Dictionary<string, int>();

    /// <summary>
    /// Awake se poziva prilikom inicijalizacije komponente.
    /// Ovdje se dohvaćaju reference na Animator (pretražujući djecu) i Rigidbody komponente.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update se poziva jednom po frame-u.
    /// Ovdje se dohvaća trenutna brzina entiteta i prosljeđuje metodi Move za ažuriranje animacije kretanja.
    /// </summary>
    private void Update()
    {
        // Prosljeđuje trenutnu brzinu (magnitude) iz Rigidbody komponente metodi Move.
        Move(_rigidBody.velocity.magnitude);
    }

    /// <summary>
    /// Metoda za pokretanje animacije skoka.
    /// Postavlja "Jump" trigger u animatoru koristeći hash vrijednost parametra.
    /// </summary>
    public void Jump()
    {
        _animator.SetTrigger(GetHash("Jump"));
    }

    /// <summary>
    /// Metoda za ažuriranje animacije kretanja.
    /// Normalizira brzinu entiteta u odnosu na maksimalnu brzinu i postavlja float parametar "Movement" u animatoru.
    /// Koristi zaglađivanje (smooth damp) za glatke prijelaze između animacija.
    /// </summary>
    /// <param name="speed">Trenutna brzina entiteta</param>
    public void Move(float speed)
    {
        // Normalizira brzinu (vrijednost između 0 i 1) i postavlja parametar s glatkim prijelazom
        _animator.SetFloat(GetHash("Movement"), speed / _maxMovementSpeed, _smooth, Time.deltaTime);
    }

    /// <summary>
    /// Metoda za postavljanje stanja pada ili prisutnosti na tlu.
    /// Postavlja bool parametar "IsOnGround" u animatoru, čime se upravlja animacijom pada ili stajanja.
    /// </summary>
    /// <param name="isOnGround">True ako je entitet na tlu, inače false</param>
    public void Falling(bool isOnGround)
    {
        _animator.SetBool(GetHash("IsOnGround"), isOnGround);
    }

    /// <summary>
    /// Metoda koja dohvaća hash vrijednost za dani string parametar animatora.
    /// Ako hash vrijednost već postoji u dictionaryju, vraća je; inače, izračunava hash, sprema ga i vraća.
    /// Ovo poboljšava performanse smanjujući broj poziva Animator.StringToHash.
    /// </summary>
    /// <param name="key">Naziv parametra animatora (npr. "Jump", "Movement", "IsOnGround")</param>
    /// <returns>Hash vrijednost parametra</returns>
    private int GetHash(string key)
    {
        if (!_paramHashes.TryGetValue(key, out var hash))
        {
            hash = Animator.StringToHash(key);
            _paramHashes.Add(key, hash);
        }
        return hash;
    }
}
