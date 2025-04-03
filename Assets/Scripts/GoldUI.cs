using TMPro;
using UnityEngine;

/// <summary>
/// GoldUI je skripta koja ažurira prikaz količine zlata na UI elementu (TextMeshProUGUI).
/// Skripta se veže uz entitet (npr. igrač) koji posjeduje parametar Gold, te sluša promjene vrijednosti tog parametra.
/// Kada se vrijednost promijeni, automatski se ažurira tekst u UI-u.
/// </summary>
public class GoldUI : MonoBehaviour
{
    // Referenca na entitet (npr. igrač) iz kojeg se dohvaća parametar zlata.
    [SerializeField] private Entity _player;
    // Referenca na UI element tipa TextMeshProUGUI koji prikazuje količinu zlata.
    [SerializeField] private TextMeshProUGUI _goldTxt;
    // Varijabla za spremanje ParamData objekta koji sadrži informacije o zlatu.
    private ParamData _paramData;

    /// <summary>
    /// Start metoda se poziva pri inicijalizaciji skripte.
    /// U ovoj metodi se dohvaća parametar zlata iz entiteta te se registrira metoda UpdateGoldText kao listener za event OnValueChange.
    /// Tako će se UI automatski ažurirati kad se vrijednost zlata promijeni.
    /// </summary>
    void Start()
    {
        // Dohvati parametar zlata iz entiteta (igrača) koristeći enumeraciju ParamType.Gold.
        _paramData = _player.GetParam(ParamType.Gold);
        // Dodaj listener metodu UpdateGoldText koja se poziva svaki put kada se promijeni vrijednost parametra.
        _paramData.OnValueChange.AddListener(UpdateGoldText);
    }

    /// <summary>
    /// UpdateGoldText metoda se poziva kada se promijeni vrijednost parametra zlata.
    /// Metoda prima novu vrijednost te ažurira tekst UI elementa.
    /// </summary>
    /// <param name="value">Nova vrijednost zlata koju treba prikazati.</param>
    private void UpdateGoldText(int value)
    {
        // Postavi tekst u TextMeshProUGUI elementu na novu vrijednost zlata.
        _goldTxt.SetText(value.ToString());
    }
}
