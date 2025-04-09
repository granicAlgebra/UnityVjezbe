using UnityEngine;

public class LookFinder : MonoBehaviour
{
    // Reference na IKcontroller komponentu koja kontrolira gdje lik gleda.
    [SerializeField] private IKcontroller _ikController;

    // Metoda se automatski poziva kada drugi collider uđe u trigger zonu ovog objekta.
    private void OnTriggerEnter(Collider other)
    {
        // Pokušaj dohvaćanja komponente "Interactable" s objekta s kojim je došlo do sudara.
        var interactable = other.GetComponent<Interactable>();

        // Ako objekt nema komponentu Interactable, izlazimo iz metode.
        if (interactable == null)
        {
            return;
        }
        // Provjera je li ime ulaznog objekta "Switch".
        if (other.name.Equals("Switch"))
        {
            // Ako je uvjet zadovoljen, aktivira se pogled na objektu "Switch".
            // Drugi parametar (true) označava da se pokreće aktivacija pogleda.
            _ikController.StareAt(other.transform, true);
        }
    }

    // Metoda se automatski poziva kada drugi collider napusti trigger zonu ovog objekta.
    private void OnTriggerExit(Collider other)
    {
        // Pokušaj dohvaćanja komponente "Interactable" s objekta koji napušta zone sudara.
        var interactable = other.GetComponent<Interactable>();

        // Ako objekt nema komponentu Interactable, izlazimo iz metode.
        if (interactable == null)
        {
            return;
        }
        // Provjera je li ime izlaznog objekta "Switch".
        if (other.name.Equals("Switch"))
        {
            // Ako je uvjet zadovoljen, deaktivira se pogled na objektu "Switch".
            // Drugi parametar (false) označava isključivanje gledanja.
            _ikController.StareAt(other.transform, false);
        }
    }
}
