using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity predstavlja entitet u igri (npr. igrač, neprijatelj) koji ima definirane parametre (npr. zdravlje, zlato).
/// Ova klasa omogućuje dohvaćanje i promjenu vrijednosti parametara putem metoda GetParam i ChangeParam.
/// </summary>
public class Entity : MonoBehaviour
{
    // Lista parametara ovog entiteta, gdje svaki element predstavlja pojedini parametar (Tip, Trenutna vrijednost, itd.).
    public List<ParamData> Params;
    public RagdollController Ragdoll;

    /// <summary>
    /// Metoda za dohvaćanje parametra određenog tipa.
    /// Iterira kroz sve elemente u listi Params i vraća onaj čiji tip odgovara zadanom.
    /// Ako parametar ne postoji, ispisuje se upozorenje u konzolu.
    /// </summary>
    /// <param name="type">Tip parametra koji se traži (npr. Gold, Health)</param>
    /// <returns>ParamData objekt ako je pronađen, inače null</returns>
    public ParamData GetParam(ParamType type)
    {
        // Iteriraj kroz svaki parametar u listi
        foreach (ParamData param in Params)
        {
            // Ako tip parametra odgovara zadanom tipu, vrati taj parametar
            if (param.Type == type)
            {
                return param;
            }
        }

        // Ako parametar nije pronađen, ispiši upozorenje i vrati null
        Debug.LogWarning($"{name} does not have param {type}");
        return null;
    }

    /// <summary>
    /// Metoda za promjenu vrijednosti parametra određenog tipa.
    /// Ako entitet ima zadani parametar, metoda ažurira njegovu vrijednost dodavanjem proslijeđene vrijednosti.
    /// Vrijednost se postavlja pomoću metode SetValue, koja osigurava da vrijednost ostane unutar zadanih granica.
    /// </summary>
    /// <param name="type">Tip parametra koji se mijenja (npr. Gold, Health)</param>
    /// <param name="value">Vrijednost koja se dodaje ili oduzima (ako je negativna) trenutnoj vrijednosti parametra</param>
    /// <returns>true ako je parametar uspješno promijenjen, inače false</returns>
    public bool ChangeParam(ParamType type, int value)
    {
        // Dohvati parametar određenog tipa
        var paramData = GetParam(type);
        if (paramData != null)
        {
            // Ažuriraj vrijednost parametra dodavanjem proslijeđene vrijednosti
            paramData.SetValue(paramData.Value + value);

            // Ako se mijenja Health parametar i vrijednost postane 0, pokušaj aktivirati ragdoll
            if (paramData.Type.Equals(ParamType.Health) && paramData.Value == 0)
            {
                Ragdoll.ActivateRagdoll();
            }
            return true;
        }
        return false;
    }

    public bool ChangeParam(ParamType type, int value, Vector3 forcePosition, float force, float radius)
    {
        // Dohvati parametar određenog tipa
        var paramData = GetParam(type);
        if (paramData != null)
        {
            // Ažuriraj vrijednost parametra dodavanjem proslijeđene vrijednosti
            paramData.SetValue(paramData.Value + value);

            // Ako se mijenja Health parametar i vrijednost postane 0, pokušaj aktivirati ragdoll
            if (paramData.Type.Equals(ParamType.Health) && paramData.Value == 0)
            {
                // Provjera da li referenca Ragdoll nije null prije poziva metode Die
                Ragdoll.Die(forcePosition, force, radius);
            }
            return true;
        }
        return false;
    }
}
