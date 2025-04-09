using System.Collections;
using UnityEngine;

/// <summary>
/// InteractBehaviour omogućava interakciju s objektima unutar određenog volumena (box) u sceni.
/// Kada igrač pritisne tipku "E", provjerava se postoje li objekti unutar definirane kutije koji implementiraju sučelje <see cref="Interactable"/>.
/// Ako se pronađe takav objekt, poziva se metoda InvokeInteraction na njemu, prosljeđujući entitet (_entity) koji inicira interakciju.
/// </summary>
public class InteractBehaviour : MonoBehaviour
{
    // Referenca na entitet (npr. igrač) koji pokreće interakciju.
    [SerializeField] private Entity _entity;
    // Veličina kutije (box) koja se koristi za provjeru kolizije/interakcije.
    [SerializeField] private Vector3 _boxSize = Vector3.one;
    // Pomak kutije prema naprijed od trenutne pozicije objekta.
    [SerializeField] private float _boxForwardOffset = 1f;
    // Vertikalni pomak kutije od trenutne pozicije objekta (npr. malo niže).
    [SerializeField] private float _boxVerticalOffset = -1f;

    [SerializeField] private IKcontroller _IKcontroller;
    [SerializeField] private PlayerMovement _playerMovement;

    /// <summary>
    /// OnDrawGizmos se koristi za vizualizaciju kutije u Editoru.
    /// Ovo omogućava developerima da vide točnu poziciju i veličinu područja za interakciju u sceni.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Izračunaj poziciju kutije koristeći trenutnu poziciju objekta, te dodajući pomake u smjeru forward i up.
        Vector3 boxPosition = transform.position + (transform.forward * _boxForwardOffset) + (transform.up * _boxVerticalOffset);

        // Postavi boju gizmo na zelenu.
        Gizmos.color = Color.green;
        // Nacrtaj žičani kockasti okvir (wire cube) na izračunatoj poziciji s veličinom _boxSize.
        Gizmos.DrawWireCube(boxPosition, _boxSize);
    }

    /// <summary>
    /// Update se poziva jednom po frame-u.
    /// Ovdje se svakog frame-a poziva metoda Interact koja provjerava input i izvršava interakciju ako je tipka pritisnuta.
    /// </summary>
    void Update()
    {
        Interact();
    }

    /// <summary>
    /// Interact metoda provjerava da li je igrač pritisnuo tipku "E" te, ako jest, provodi pretragu kolizijskih objekata unutar definiranog volumena.
    /// Za svaki pronađeni collider, provjerava se ima li komponentu koja implementira sučelje <see cref="Interactable"/>.
    /// Ako se pronađe, poziva se metoda InvokeInteraction te se petlja prekida.
    /// </summary>
    private void Interact()
    {
        // Provjeri da li je tipka "E" pritisnuta. Ako nije, izlazi iz metode.
        if (!Input.GetKeyDown(KeyCode.E))
        {
            return;
        }

        // Izračunaj poziciju kutije (box) za pretragu koristeći trenutnu poziciju i definirane offset-eve.
        Vector3 boxPosition = transform.position + (transform.forward * _boxForwardOffset) + (transform.up * _boxVerticalOffset);

        // Pretraži sve collidere unutar kutije. Metoda OverlapBox prima poziciju, pola veličine kutije (jer se radi o radijusu),
        // te rotaciju objekta, i vraća sve collidere unutar tog volumena.
        Collider[] hits = Physics.OverlapBox(boxPosition, _boxSize / 2, transform.rotation);

        // Iteriraj kroz sve pronađene collidere.
        for (int i = 0; i < hits.Length; i++)
        {
            // Pokušaj dohvatiti komponentu koja implementira sučelje Interactable s trenutnog collidera.
            Interactable interactable = hits[i].GetComponent<Interactable>();

            // Ako komponenta postoji (objekt je interaktivan), pozovi metodu InvokeInteraction, prosljeđujući entitet koji inicira interakciju.
            if (interactable != null)
            {
                StartCoroutine(InteractCoroutine(hits[i].transform.position, interactable));
                // Nakon prve uspješne interakcije, prekini daljnju pretragu.
                break;
            }
        }
    }

    private IEnumerator InteractCoroutine(Vector3 position, Interactable interactable)
    {
        float timeToInteract = 0;
        _playerMovement.enabled = false;
        while (timeToInteract < 1)
        {
            _IKcontroller.MoveRightHand(position, timeToInteract);
            timeToInteract += Time.deltaTime;
            yield return null;
        }

        interactable.InvokeInteraction(_entity);
        yield return new WaitForSeconds(0.5f);
        timeToInteract = 1;

        while (timeToInteract > 0)
        {
            _IKcontroller.MoveRightHand(position, timeToInteract);
            timeToInteract -= Time.deltaTime;
            yield return null;
        }
        _playerMovement.enabled = true;
    }
}
