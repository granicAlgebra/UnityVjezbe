using UnityEngine;

public class LookFinder : MonoBehaviour
{
    [SerializeField] private IKcontroller _ikController;

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable == null)
        {
            return;
        }
        if (other.name.Equals("Switch"))
        {
            _ikController.StareAt(other.transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        if (interactable == null)
        {
            return;
        }
        if (other.name.Equals("Switch"))
        {
            _ikController.StareAt(other.transform, false);
        }
    }
}
