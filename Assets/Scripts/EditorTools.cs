using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// ReadOnlyDrawer je Custom Property Drawer koji omogućuje prikaz varijabli označenih s [ReadOnly] u Inspectoru,
/// ali bez mogućnosti izmjene njihovih vrijednosti. Ova skripta je korisna za debug ili za prikaz vrijednosti koje
/// se ne smiju mijenjati ručno u Editoru.
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// <summary>
    /// OnGUI metoda se poziva za iscrtavanje property-ja u Inspectoru.
    /// Ovdje se privremeno onemogućuje GUI kako bi se spriječile izmjene, zatim se iscrtava property,
    /// te se na kraju GUI ponovno omogućuje.
    /// </summary>
    /// <param name="position">Pozicija i veličina property-ja u Inspectoru.</param>
    /// <param name="property">SerializedProperty koji se crta.</param>
    /// <param name="label">Labela koja se prikazuje pored property-ja.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Onemogući unos tako da property postane read-only
        GUI.enabled = false;
        // Iscrtaj property field s uključenim prikazom svih child elemenata (ako ih ima)
        EditorGUI.PropertyField(position, property, label, true);
        // Ponovno omogući unos nakon crtanja property-ja
        GUI.enabled = true;
    }
}
#endif

/// <summary>
/// ReadOnlyAttribute se koristi za označavanje varijabli koje želite prikazati u Inspectoru, ali koje ne želite
/// da budu izmijenjene tijekom rada u Editoru. Ovaj atribut se primjenjuje izravno na polja u skriptama.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }
