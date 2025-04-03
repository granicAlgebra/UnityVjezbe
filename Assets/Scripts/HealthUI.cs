using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HealthUI služi za prikaz zdravstvenog statusa entiteta (npr. igrača) na UI elementu.
/// Ova skripta ažurira Image komponentu (health bar) na temelju promjena u parametru zdravlja (Health) entiteta.
/// </summary>
public class HealthUI : MonoBehaviour
{
    // Referenca na UI Image komponentu koja prikazuje health bar.
    [SerializeField] private Image _healthBar;
    // Referenca na entitet (npr. igrač) čiji zdravstveni parametar želimo pratiti.
    [SerializeField] private Entity _entity;
    // Varijabla koja sprema ParamData za zdravstveni parametar entiteta.
    private ParamData _paramData;

    /// <summary>
    /// Start metoda se poziva pri inicijalizaciji skripte.
    /// Dohvaća se zdravstveni parametar entiteta te se dodaje listener koji će ažurirati health bar kad se vrijednost promijeni.
    /// Također se odmah postavlja početno stanje health bara.
    /// </summary>
    void Start()
    {
        // Dohvati parametar zdravlja iz entiteta koristeći enumeraciju ParamType.Health.
        _paramData = _entity.GetParam(ParamType.Health);
        // Dodaj listener metodu UpdateHealthBar koji se poziva kada se promijeni vrijednost parametra.
        _paramData.OnValueChange.AddListener(UpdateHealthBar);
        // Inicijalno ažuriraj health bar na temelju trenutne vrijednosti zdravlja.
        UpdateHealthBar(_paramData.Value);
    }

    /// <summary>
    /// UpdateHealthBar metoda ažurira UI health bar na temelju nove vrijednosti zdravlja.
    /// Postavlja fillAmount health bara kao omjer trenutne vrijednosti i maksimalne vrijednosti zdravlja.
    /// </summary>
    /// <param name="arg0">Nova vrijednost zdravlja (Health) entiteta</param>
    private void UpdateHealthBar(int arg0)
    {
        // Računa se omjer trenutne vrijednosti zdravlja u odnosu na maksimalnu vrijednost (MinMax.y)
        // fillAmount je vrijednost između 0 i 1, gdje 1 predstavlja puno zdravlje.
        _healthBar.fillAmount = arg0 / (float)_paramData.MinMax.y;
    }
}