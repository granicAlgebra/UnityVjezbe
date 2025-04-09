using UnityEngine;

/// <summary>
/// LandMine predstavlja minu koja se aktivira kada entitet (npr. igrač, neprijatelj) uđe u njezin trigger.
/// Kada se aktivira, mina mijenja određeni parametar entiteta (npr. smanjuje zdravlje),
/// reproducira zvučne i vizualne efekte eksplozije, primjenjuje impulsnu silu na entitet,
/// te se deaktivira kako se ne bi ponovno aktivirala.
/// Ova skripta implementira sučelje <see cref="Interactable"/> i omogućuje i eksplicitnu interakciju.
/// </summary>
public class LandMine : MonoBehaviour, Interactable
{
    // Parametar koji se mijenja prilikom aktivacije mine (npr. Health)
    [SerializeField] private ParamType _paramType;
    // Količina kojom se mijenja navedeni parametar (pozitivno ili negativno)
    [SerializeField] private int _amount;
    // Sila koja se primjenjuje na entitet prilikom eksplozije
    [SerializeField] private float _force;
    // Prefab AudioSource koji se koristi za reproduciranje zvučnog efekta eksplozije
    [SerializeField] private AudioSource _SFXprefab;
    // AudioClip koji sadrži zvuk eksplozije
    [SerializeField] private AudioClip _SFXclip;
    // Prefab ParticleSystem koji se koristi za prikaz vizualnog efekta eksplozije
    [SerializeField] private ParticleSystem _VFXprefab;

    /// <summary>
    /// OnTriggerEnter se automatski poziva kada drugi collider uđe u trigger zonu ove mine.
    /// Ako objekat koji ulazi ima komponentu Entity, mina se aktivira.
    /// </summary>
    /// <param name="other">Collider objekta koji ulazi u trigger zonu</param>
    private void OnTriggerEnter(Collider other)
    {
        // Pokušaj dohvatiti komponentu Entity s objekta koji je ušao u trigger zonu
        var entity = other.GetComponent<Entity>();
        // Ako entitet postoji, mina se aktivira
        if (entity != null)
        {
            // Promijeni parametar entiteta (npr. smanji zdravlje)
            entity.ChangeParam(_paramType, _amount, transform.position, _force, 1);
            // Reproduciraj zvučni efekt eksplozije pomoću SFXManagera (Singleton instanca)
            SFXManager.Instance.PlaySFX(_SFXprefab, transform.position, _SFXclip);
            // Reproduciraj vizualni efekt eksplozije pomoću VFXManagera (Singleton instanca)
            VFXManager.Instance.PlayVFX(_VFXprefab, transform.position);
            // Izračunaj smjer impulsa: od mine prema poziciji entiteta
            var dir = other.transform.position - transform.position;
            // Primijeni impulsnu silu entitetu na temelju izračunatog smjera i definirane sile _force
            other.GetComponent<Rigidbody>().AddForce(dir.normalized * _force, ForceMode.Impulse);
            // Deaktiviraj minu kako bi se spriječila ponovna aktivacija
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Implementacija metode InvokeInteraction iz sučelja Interactable.
    /// Ova metoda omogućuje eksplicitno pokretanje interakcije s minom (npr. putem tipkovnice).
    /// Kada se pozove, mijenja se parametar entiteta i mina se deaktivira.
    /// </summary>
    /// <param name="entity">Entitet koji inicira interakciju</param>
    public void InvokeInteraction(Entity entity)
    {
        // Promijeni parametar entiteta (npr. smanji zdravlje)
        entity.ChangeParam(_paramType, _amount);
        // Deaktiviraj minu nakon interakcije
        gameObject.SetActive(false);
    }
}
