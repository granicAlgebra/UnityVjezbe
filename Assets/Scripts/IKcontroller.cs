using UnityEngine.Animations.Rigging;
using UnityEngine;

public class IKcontroller : MonoBehaviour
{
    // Transform cilja za pomicanje glave (gdje lik gleda)
    [SerializeField] private Transform _headTarget;
    // Transform cilja za pomicanje ruke (gdje ruka treba biti)
    [SerializeField] private Transform _handTarget;

    // MultiAimConstraint za kontrolu usmjeravanja glave prema cilju
    [SerializeField] private MultiAimConstraint _headLook;
    // TwoBoneIKConstraint za inverznu kinematiku desne ruke
    [SerializeField] private TwoBoneIKConstraint _handIK;

    // Faktor glatkoće kretanja glave pri prelasku između pozicija
    [SerializeField] private float _smoothHead;

    // Početna lokalna pozicija cilja glave (spremamo kako bismo se mogli "resetirati")
    private Vector3 _heatTargetStart;
    // Trenutni cilj na koji lik treba gledati (ako postoji)
    private Transform _stareTarget;

    // Metoda Awake se poziva odmah nakon inicijalizacije objekta
    private void Awake()
    {
        // Spremamo početnu lokalnu poziciju cilja glave, kako bismo mogli vratiti glavu na početnu poziciju
        _heatTargetStart = _headTarget.localPosition;
        // Postavljamo početnu težinu (weight) IK kontrola ruke na 0, čime se inverzna kinematika ne primjenjuje
        _handIK.weight = 0;
    }

    // Metoda za upravljanje pogledom prema određenom cilju
    // target: Transform objekta na koji treba gledati
    // start: true ako želimo započeti gledanje, false ako želimo zaustaviti
    public void StareAt(Transform target, bool start)
    {
        if (start)
        {
            // Ako je start true, postavljamo _stareTarget na željeni cilj
            _stareTarget = target;
        }
        // Ako se traži prekid gledanja, a trenutni cilj odgovara proslijeđenom targetu, poništavamo _stareTarget
        else if (_stareTarget != null && _stareTarget.Equals(target))
        {
            _stareTarget = null;
        }
    }

    // Metoda za pomicanje desne ruke prema određenoj poziciji s određenom težinom IK kontrola
    public void MoveRightHand(Vector3 position, float weight)
    {
        // Postavljamo ciljnu poziciju za ruku
        _handTarget.position = position;
        // Postavljamo težinu primjene inverzne kinematike na ruci
        _handIK.weight = weight;
    }

    // Update metoda se poziva u svakom frame-u, omogućavajući dinamično ažuriranje pozicija
    private void Update()
    {
        // Ako nema aktivnog cilja (_stareTarget je null) za gledanje...
        if (_stareTarget == null)
        {
            // ...postupno vraćamo cilj glave na početnu lokalnu poziciju koristeći linearnu interpolaciju (Lerp)
            _headTarget.localPosition = Vector3.Lerp(_headTarget.localPosition, _heatTargetStart, _smoothHead * Time.deltaTime);
        }
        else
        {
            // Ako postoji aktivni cilj, glavu postupno usmjeravamo prema poziciji cilja koristeći Lerp
            _headTarget.position = Vector3.Lerp(_headTarget.position, _stareTarget.position, _smoothHead * Time.deltaTime);
        }
    }
}
