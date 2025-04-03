using UnityEngine;

/// <summary>
/// Coin predstavlja objekt koji igrač može prikupiti, obično za povećanje nekog parametra (npr. bodovi, valuta).
/// Kada igrač uđe u trigger zonu ovog objekta, izvršava se interakcija koja:
/// - mijenja parametar entiteta,
/// - reproducira zvučni efekt pomoću SFXManagera,
/// - reproducira vizualni efekt pomoću VFXManagera,
/// - deaktivira Coin objekt kako se ne bi ponovo prikupio.
/// 
/// Ova skripta implementira sučelje <see cref="Interactable"/>, omogućujući i eksplicitno pozivanje interakcije (npr. pritiskom tipke).
/// </summary>
public class Coin : MonoBehaviour, Interactable
{
    // Parametar koji se mijenja prilikom prikupljanja kovanice (npr. valuta, bodovi)
    [SerializeField] private ParamType _paramType;
    // Količina kojom se povećava definirani parametar
    [SerializeField] private int _amount;
    // Prefab AudioSource koji se koristi za reproduciranje zvučnog efekta prilikom prikupljanja
    [SerializeField] private AudioSource _SFXprefab;
    // AudioClip koji se reproducira kad se kovanica prikupi
    [SerializeField] private AudioClip _SFXclip;
    // Prefab ParticleSystem koji se koristi za vizualni efekt prilikom prikupljanja
    [SerializeField] private ParticleSystem _VFXprefab;

    /// <summary>
    /// Metoda koja se automatski poziva kada drugi Collider uđe u trigger zonu ove kovanice.
    /// Ako objekt koji ulazi ima komponentu Entity, izvršava se interakcija:
    /// - mijenja se parametar entiteta,
    /// - reproduciraju se zvučni i vizualni efekti,
    /// - kovanica se deaktivira.
    /// </summary>
    /// <param name="other">Collider objekta koji ulazi u trigger zonu</param>
    private void OnTriggerEnter(Collider other)
    {
        // Pokušaj dohvatiti komponentu Entity s objekta koji je ušao u trigger zonu
        var entity = other.GetComponent<Entity>();
        // Ako komponenta postoji, objekt je interaktivan
        if (entity != null)
        {
            // Promijeni parametar entiteta (npr. dodaj bodove ili valutu)
            entity.ChangeParam(_paramType, _amount);
            // Reproduciraj zvučni efekt pomoću SFXManagera (Singleton instanca)
            SFXManager.Instance.PlaySFX(_SFXprefab, transform.position, _SFXclip);
            // Reproduciraj vizualni efekt pomoću VFXManagera (Singleton instanca)
            VFXManager.Instance.PlayVFX(_VFXprefab, transform.position);
            // Deaktiviraj Coin objekt kako se ne bi mogao ponovo prikupiti
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Implementacija metode iz sučelja Interactable.
    /// Ova metoda omogućuje eksplicitno pokretanje interakcije (npr. putem tipkovnice),
    /// pri čemu se mijenja parametar entiteta, a Coin objekt se deaktivira.
    /// </summary>
    /// <param name="entity">Entitet koji inicira interakciju (npr. igrač)</param>
    public void InvokeInteraction(Entity entity)
    {
        // Promijeni parametar entiteta na temelju definiranog tipa i količine
        entity.ChangeParam(_paramType, _amount);
        // Deaktiviraj Coin objekt nakon izvršene interakcije
        gameObject.SetActive(false);
    }
}
