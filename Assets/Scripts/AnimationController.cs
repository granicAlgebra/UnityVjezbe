using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AnimationController služi za upravljanje animacijama entiteta temeljem njegovih fizikalnih svojstava (npr. brzine kretanja, skoka, pada).
/// Ova skripta dohvaća referencu na Animator i Rigidbody komponente te na osnovu njihovih stanja ažurira animacije.
/// Također, koristi se Dictionary kako bi se optimiziralo korištenje hashiranih parametara animatora.
/// </summary>
public class AnimationController : MonoBehaviour
{
    // Maksimalna brzina kretanja entiteta, koristi se za normalizaciju brzine u blend treeu animacija.
    [SerializeField] private float _maxMovementSpeed;
    // Vrijednost za zaglađivanje prijelaza (smooth damp) prilikom mijenjanja parametara animacije.
    [SerializeField] private float _smooth;
    // Trajanje prelaska (blend) layer weight-a prilikom početka i završetka napada.
    [SerializeField] private float _blendDuration = 0.2f;

    // Referenca na Animator komponentu koja upravlja animacijama.
    private Animator _animator;
    // Referenca na Rigidbody komponentu koja omogućuje dohvaćanje fizičkih svojstava entiteta.
    private Rigidbody _rigidBody;

    // Varijabla koja označava je li entitet trenutno u napadu.
    private bool _isAttacking = false;

    // Dictionary za spremanje hashiranih vrijednosti parametara animatora. Time se smanjuje potreba za ponovnim izračunom hasha.
    private Dictionary<string, int> _paramHashes = new Dictionary<string, int>();

    // Javna svojstva za pristup animatoru, ali samo za čitanje izvana.
    public Animator Animator { get => _animator; private set { _animator = value; } }

    /// <summary>
    /// Awake se poziva prilikom inicijalizacije komponente.
    /// Ovdje se dohvaćaju reference na Animator (pretražujući djecu) i Rigidbody komponente.
    /// </summary>
    private void Awake()
    {
        // Pokušaj dohvaćanja Animator komponente iz djece objekta.
        _animator = GetComponentInChildren<Animator>();
        if (_animator == null)
        {
            // Ako Animator nije pronađen, ispisati upozorenje.
            Debug.LogWarning($"{name} nema Animator komponentu u hijerarhiji.");
        }

        // Dohvati Rigidbody komponentu s istog objekta.
        _rigidBody = GetComponent<Rigidbody>();
        if (_rigidBody == null)
        {
            // Ako Rigidbody nije pronađen, ispisati upozorenje.
            Debug.LogWarning($"{name} nema Rigidbody komponentu.");
        }
    }

    /// <summary>
    /// Update se poziva jednom po frame-u.
    /// Ovdje se dohvaća trenutna brzina entiteta i prosljeđuje metodi Move za ažuriranje animacije kretanja.
    /// </summary>
    private void Update()
    {
        // Ako _rigidBody postoji, proslijedi njegovu trenutnu brzinu metodi Move.
        if (_rigidBody != null)
        {
            Move(_rigidBody.velocity.magnitude);
        }
    }

    /// <summary>
    /// Metoda za pokretanje animacije skoka.
    /// Postavlja "Jump" trigger u animatoru koristeći hash vrijednost parametra.
    /// </summary>
    public void Jump()
    {
        // Postavi trigger "Jump" pomoću hash vrijednosti.
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
        // Provjeri da _maxMovementSpeed nije 0 kako se ne bi dogodilo dijeljenje s nulom.
        if (_maxMovementSpeed <= 0)
        {
            Debug.LogWarning("MaxMovementSpeed je 0 ili manja. Nemoguće normalizirati brzinu.");
            return;
        }
        // Normaliziraj brzinu (vrijednost između 0 i 1) i postavi float parametar "Movement" s glatkim prijelazom.
        _animator.SetFloat(GetHash("Movement"), speed / _maxMovementSpeed, _smooth, Time.deltaTime);
    }

    /// <summary>
    /// Metoda za postavljanje stanja pada ili prisutnosti na tlu.
    /// Postavlja bool parametar "IsOnGround" u animatoru, čime se upravlja animacijom pada ili stajanja.
    /// </summary>
    /// <param name="isOnGround">True ako je entitet na tlu, inače false</param>
    public void Falling(bool isOnGround)
    {
        // Postavi bool parametar "IsOnGround" u animatoru.
        _animator.SetBool(GetHash("IsOnGround"), isOnGround);
    }

    /// <summary>
    /// Metoda za pokretanje animacije napada.
    /// Ako entitet već napada, metoda se ne izvršava ponovo.
    /// Inače, pokreće se coroutine HandleAttack.
    /// </summary>
    public void PlayAttack()
    {
        // Ako je entitet već u napadu, izađi iz metode.
        if (_isAttacking) return;
        // Pokreni coroutine za upravljanje napadom.
        StartCoroutine(HandleAttack());
    }

    /// <summary>
    /// Coroutine koja postupa s animacijom napada.
    /// Uključuje blendanje layer weight-a, postavljanje triggera napada, te čeka da animacija napada prođe određeni prag prije vraćanja.
    /// </summary>
    private IEnumerator HandleAttack()
    {
        _isAttacking = true;

        // Postupno blendaj weight layera 1 na vrijednost 1 tijekom _blendDuration.
        yield return StartCoroutine(BlendLayerWeight(1, 1f, _blendDuration));

        // Postavi trigger "Attack" za pokretanje animacije napada.
        _animator.SetTrigger(GetHash("Attack"));

        AnimatorStateInfo stateInfo;

        // Čekaj dok animacija na layeru 1 ne prođe 90% (normalizedTime < 0.9)
        do
        {
            yield return null;
            stateInfo = _animator.GetCurrentAnimatorStateInfo(1);
        } while (stateInfo.normalizedTime < 0.9f);

        // Postupno vrati weight layera 1 na 0 tijekom _blendDuration.
        yield return StartCoroutine(BlendLayerWeight(1, 0, _blendDuration));

        _isAttacking = false;
    }

    /// <summary>
    /// Coroutine za postupno miješanje (blendanje) layer weight-a animatora.
    /// Prelazi s početne vrijednosti weight-a (startWeight) na ciljanu vrijednost (targetWeight) tijekom zadanog trajanja (duration).
    /// </summary>
    /// <param name="layerIndex">Indeks layera čiji se weight mijenja</param>
    /// <param name="targetWeight">Ciljana vrijednost weight-a</param>
    /// <param name="duration">Trajanje blendanja</param>
    private IEnumerator BlendLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        // Početna vrijednost weight-a na zadanom layeru.
        float startWeight = _animator.GetLayerWeight(layerIndex);
        float time = 0;

        // Postupno mijenjaj weight vrijednost koristeći linearnu interpolaciju.
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, t);
            _animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }
        // Osiguraj da je na kraju weight postavljen točno na ciljanu vrijednost.
        _animator.SetLayerWeight(layerIndex, targetWeight);
    }

    /// <summary>
    /// Metoda koja dohvaća hash vrijednost za dani string parametar animatora.
    /// Ako hash vrijednost već postoji u dictionary-ju, vraća je; inače, izračunava hash, sprema ga i vraća.
    /// Ovo poboljšava performanse smanjujući broj poziva Animator.StringToHash.
    /// </summary>
    /// <param name="key">Naziv parametra animatora (npr. "Jump", "Movement", "IsOnGround")</param>
    /// <returns>Hash vrijednost parametra</returns>
    private int GetHash(string key)
    {
        // Ako hash vrijednost za zadani ključ nije spremljena, izračunaj je i spremi u dictionary.
        if (!_paramHashes.TryGetValue(key, out var hash))
        {
            hash = Animator.StringToHash(key);
            _paramHashes.Add(key, hash);
        }
        return hash;
    }
}
