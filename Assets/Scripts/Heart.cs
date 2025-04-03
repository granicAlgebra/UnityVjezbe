
using UnityEngine;


/// <summary>
/// Heart predstavlja objekt koji igrač može prikupiti kako bi povećao neki parametar (npr. zdravlje).
/// Kada igrač (ili drugi entitet) dođe u kontakt s ovim objektom, izvršava se interakcija koja:
/// - mijenja definirani parametar entiteta,
/// - reproducira odgovarajući zvučni efekt,
/// - deaktivira Heart objekt kako se ne bi ponovno iskoristio.
/// 
/// Ova skripta implementira sučelje <see cref="Interactable"/>, što omogućuje da se interakcija pokrene i na druge načine (npr. putem tipkovnice).
/// </summary>
public class Heart : MonoBehaviour, Interactable
{
    // Parametar koji se mijenja prilikom interakcije (npr. zdravlje, energija)
    [SerializeField] private ParamType _paramType;
    // Količina kojom se parametar mijenja (npr. +1, +5)
    [SerializeField] private int _amount;
    // Prefab AudioSource koji se koristi za reproduciranje zvučnog efekta prilikom interakcije
    [SerializeField] private AudioSource _SFXprefab;
    // AudioClip koji se reproducira prilikom prikupljanja Heart objekta
    [SerializeField] private AudioClip _SFXclip;

    /// <summary>
    /// Metoda koja se automatski poziva kada drugi Collider uđe u trigger zonu ovog objekta.
    /// Ako objekt koji ulazi ima komponentu Entity, izvršava se interakcija:
    /// - mijenja se parametar entiteta,
    /// - reproducira se zvučni efekt,
    /// - deaktivira se Heart objekt.
    /// </summary>
    /// <param name="other">Collider objekta koji ulazi u trigger zonu</param>
    private void OnTriggerEnter(Collider other)
    {
        // Pokušaj dohvatiti komponentu Entity s objekta koji je ušao u trigger zonu
        var entity = other.GetComponent<Entity>();
        // Ako komponenta postoji, objekt je interaktivan
        if (entity != null)
        {
            // Promijeni parametar entiteta na temelju definiranog tipa i količine
            entity.ChangeParam(_paramType, _amount);
            // Reproduciraj zvučni efekt pomoću SFXManagera (Singleton instanca)
            SFXManager.Instance.PlaySFX(_SFXprefab, transform.position, _SFXclip);
            // Deaktiviraj Heart objekt kako bi se spriječila daljnja interakcija
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Implementacija metode iz sučelja Interactable.
    /// Ova metoda omogućuje eksplicitno pokretanje interakcije, što se može koristiti
    /// za različite načine interakcije (npr. pritiskom tipke).
    /// Kada se pozove, mijenja se parametar entiteta i deaktivira se Heart objekt.
    /// </summary>
    /// <param name="entity">Entitet koji inicira interakciju (npr. igrač)</param>
    public void InvokeInteraction(Entity entity)
    {
        // Promijeni parametar entiteta pomoću definiranog tipa i količine
        entity.ChangeParam(_paramType, _amount);
        // Deaktiviraj Heart objekt nakon izvršene interakcije
        gameObject.SetActive(false);
    }
}
