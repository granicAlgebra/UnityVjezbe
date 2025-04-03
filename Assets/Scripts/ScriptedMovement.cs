using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // DOTween biblioteka za tweening animacije

/// <summary>
/// ScriptedMovement kontrolira pomicanje objekta (npr. vrata) prema zadanim pozicijama uz pomoć tweeninga ili coroutina.
/// Ova skripta demonstrira nekoliko načina za animiranje kretanja, kao što su Lerp, SmoothDamp i tweening s DOTweenom.
/// </summary>
public class ScriptedMovement : MonoBehaviour
{
    // Transform cilj koji se koristi za alternativno pomicanje (npr. slijedi neki drugi objekt)
    [SerializeField] Transform _targert;
    // Krajnja pozicija do koje će se objekt pomaknuti
    [SerializeField] Vector3 _endPosition;
    // Vrijeme potrebno za pomicanje objekta od početne do krajnje pozicije
    [SerializeField] private float _timeToMove = 2f;

    // Brzina zaglađivanja za alternativno pomicanje (koristi se u Update metodi, zakomentirano)
    [SerializeField] private float _smoothSpeed = 2f;

    // AnimationCurve koja se može koristiti za prilagođeno animiranje (zakomentirano primjer)
    [SerializeField] private AnimationCurve _animationCurve;

    // Početna pozicija objekta, postavljena u Start metodi
    private Vector3 _startPosition;
    // Varijabla za pohranu brzine (koristi se u SmoothDamp metodi)
    private Vector3 _velocity;
    // Varijabla koja označava trenutno stanje (npr. otvorena ili zatvorena vrata)
    private bool _isOpen;

    // Referenca na pokrenuti coroutine, kako bismo mogli zaustaviti prethodni ako je potrebno
    private Coroutine _moveCoroutine;

    // Start se poziva prije prvog frame-a
    void Start()
    {
        // Spremi trenutnu poziciju objekta kao početnu poziciju
        _startPosition = transform.position;
        // Inicijalno stanje postavimo na zatvoreno (false)
        _isOpen = false;

        // Primjeri korištenja DOTween tweeninga (zakomentirano):
        // transform.DOMove(_endPosition, _timeToMove).SetEase(Ease.OutQuint);
        // Ovim se objekt pomakne do _endPosition u _timeToMove sekundi s Ease.OutQuint krivuljom

        // Primjer pokretanja coroutine MoveRoutine (zakomentirano):
        // StartCoroutine(MoveRoutine());
    }

    /// <summary>
    /// Metoda koja pokreće proces otvaranja (ili zatvaranja) vrata.
    /// Ako je već pokrenuta coroutine, ona se zaustavlja i ponovno pokreće kako bi se osiguralo pravilno ponašanje.
    /// </summary>
    public void OpenDoor()
    {
        if (_moveCoroutine == null)
        {
            // Ako nema pokrenutog coroutinea, pokreni ga
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }
        else
        {
            // Ako već postoji pokrenuti coroutine, zaustavi ga i pokreni ponovno
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }
    }

    /// <summary>
    /// Coroutine koja postepeno pomiče objekt između početne i krajnje pozicije.
    /// Metoda koristi različite primjere interpolacije (Lerp, Lerp s cubic funkcijom ili AnimationCurve)
    /// - Zakomentirani primjeri pokazuju različite načine interpolacije.
    /// </summary>
    /// <returns>IEnumerator potreban za coroutine</returns>
    private IEnumerator MoveRoutine()
    {
        // Prebacuje stanje: ako je otvoreno, postaje zatvoreno, i obrnuto
        _isOpen = !_isOpen;
        float time = 0;
        while (time < _timeToMove)
        {
            // Povećaj vrijeme proteklo od početka animacije
            time += Time.deltaTime;

            // Primjeri interpolacije (zakomentirani):
            // 1. Klasični Lerp između _startPosition i _endPosition:
            // transform.position = Vector3.Lerp(_startPosition, _endPosition, time / _timeToMove);

            // 2. Lerp s primjenom cubic funkcije za easing efekt:
            // transform.position = Vector3.Lerp(_startPosition, _endPosition, CubicIn(time / _timeToMove));

            // 3. Lerp s AnimationCurve, gdje krivulja određuje progres:
            // transform.position = Vector3.Lerp(_startPosition, _endPosition, _animationCurve.Evaluate(time / _timeToMove));

            // U ovom primjeru, ovisno o stanju (_isOpen), objekt se pomiče prema početnoj ili krajnjoj poziciji.
            if (_isOpen)
            {
                // Ako je stanje otvoreno, približi se početnoj poziciji
                transform.position = Vector3.Lerp(transform.position, _startPosition, CubicIn(time / _timeToMove));
            }
            else
            {
                // Inače, približi se krajnjoj poziciji
                transform.position = Vector3.Lerp(transform.position, _endPosition, CubicIn(time / _timeToMove));
            }

            // Čekaj sljedeći frame
            yield return null;
        }
    }

    // Primjer alternativnog Update metoda za kontinuirano praćenje cilja (zakomentirano):
    /*
    private void Update()
    {
        // 1. Koristeći Lerp za postepeno pomicanje prema target poziciji:
        // transform.position = Vector3.Lerp(transform.position, _targert.position, Time.deltaTime * _smoothSpeed); 
        
        // 2. Koristeći MoveTowards za linearan prijelaz prema target poziciji:
        // transform.position = Vector3.MoveTowards(transform.position, _targert.position, Time.deltaTime * _smoothSpeed); 
        
        // 3. Koristeći SmoothDamp za glatko kretanje s momentalnim ubrzanjem i usporavanjem:
        transform.position = Vector3.SmoothDamp(transform.position, _targert.position, ref _velocity, Time.deltaTime * _smoothSpeed); 
    }
    */

    // Metoda za vizualizaciju krajnje pozicije u Editoru.
    // Ova metoda se izvršava samo kada je objekt odabran u Editoru (unutar UNITY_EDITOR bloka).
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Postavi boju gizmo crvenu
        Gizmos.color = Color.red;
        // Nacrtaj žičani kockasti okvir na _endPosition s veličinom jednakom lokalnoj skali objekta
        Gizmos.DrawWireCube(_endPosition, transform.localScale);
    }
#endif

    /// <summary>
    /// Metoda koja vraća vrijednost cubic easing funkcije (CubicIn).
    /// Ova funkcija ubrzava animaciju na početku.
    /// </summary>
    /// <param name="t">Vrijednost između 0 i 1 koja predstavlja normalizirani vrijeme</param>
    /// <returns>t^3, što daje cubic easing efekt</returns>
    private float CubicIn(float t)
    {
        return t * t * t;
    }
}
