using UnityEngine;

/// <summary>
/// Switch predstavlja interaktivan prekidač koji, kada se aktivira, pokreće interakciju s objektom koji ima ScriptedMovement komponentu,
/// primjerice vrata. Ova skripta implementira sučelje <see cref="Interactable"/>, što omogućuje da se interakcija pokrene
/// od strane entiteta (npr. igrača) putem InvokeInteraction metode.
/// </summary>
public class Switch : MonoBehaviour, Interactable
{
    // Referenca na objekt s ScriptedMovement komponentom (npr. vrata) koji će se otvoriti ili zatvoriti prilikom interakcije.
    [SerializeField] private ScriptedMovement _door;

    /// <summary>
    /// Implementacija metode iz sučelja Interactable.
    /// Kada entitet (npr. igrač) inicira interakciju s ovim prekidačem, poziva se metoda OpenDoor na objektu _door,
    /// čime se pokreće animacija ili logika otvaranja/zatvaranja.
    /// </summary>
    /// <param name="entity">Entitet koji pokreće interakciju (npr. igrač).</param>
    public void InvokeInteraction(Entity entity)
    {
        // Pozovi metodu OpenDoor na objektu _door kako bi se pokrenuo proces otvaranja ili zatvaranja.
        _door.OpenDoor();
    }
}
