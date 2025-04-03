using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ParamData predstavlja podatkovnu strukturu koja drži informacije o nekom parametru (npr. Gold ili Health).
/// Sadrži trenutnu vrijednost, minimalno i maksimalno ograničenje te događaj koji se poziva kada se vrijednost promijeni.
/// </summary>
[Serializable]
public class ParamData
{
    // Tip parametra, npr. Gold ili Health, definiran u enumeraciji ParamType.
    public ParamType Type;

    // Trenutna vrijednost parametra.
    public int Value;

    // Minimalna i maksimalna dozvoljena vrijednost parametra.
    public Vector2Int MinMax;

    // UnityEvent koji se poziva prilikom promjene vrijednosti, prosljeđuje novu vrijednost kao argument.
    public UnityEvent<int> OnValueChange;

    /// <summary>
    /// Postavlja novu vrijednost parametra.
    /// Vrijednost se ograničava između MinMax.x (minimum) i MinMax.y (maksimum) pomoću Mathf.Clamp.
    /// Nakon postavljanja nove vrijednosti, OnValueChange event se aktivira ako je definiran.
    /// </summary>
    /// <param name="value">Nova vrijednost koja se postavlja</param>
    public void SetValue(int value)
    {
        // Ograniči vrijednost između zadanih minimalnih i maksimalnih vrijednosti.
        Value = Mathf.Clamp(value, MinMax.x, MinMax.y);
        // Pozovi event za promjenu vrijednosti, ako postoje pretplatnici.
        OnValueChange?.Invoke(Value);
    }
}

/// <summary>
/// Enumeracija koja definira tipove parametara.
/// </summary>
public enum ParamType
{
    Gold,   // Predstavlja parametar zlata ili valute.
    Health  // Predstavlja parametar zdravlja.
}
