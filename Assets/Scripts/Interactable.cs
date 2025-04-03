/// <summary>
/// Sučelje (interface) koje definira metodu za interakciju s objektom.
/// Ovaj interface se može implementirati u raznim klasama kako bi omogućio uniforman način 
/// za rukovanje interakcijama (npr. podizanje predmeta, otvaranje vrata, pokretanje događaja).
/// 
/// Napomena: Od C# 8, interfejsi mogu imati default implementacije metoda.
/// Metoda je deklarirana kao virtual kako bi se omogućilo override-anje u implementirajućim klasama.
/// </summary>
public interface Interactable
{
    /// <summary>
    /// Metoda koja se poziva kada entitet (npr. igrač ili drugi objekt) inicira interakciju.
    /// Default implementacija je prazna, ali se u konkretnim klasama može pružiti specifično ponašanje.
    /// </summary>
    /// <param name="entity">Entitet koji pokreće interakciju (npr. igrač, NPC)</param>
    public virtual void InvokeInteraction(Entity entity) { }
}
