using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
// Ovaj prilagođeni editor omogućava dodavanje dodatnih funkcionalnosti u Inspector za RagdollController komponentu.
[CustomEditor(typeof(RagdollController))]
public class RagdollEditor : Editor
{
    // Prepisivanje standardne metode za prikazivanje Inspector sučelja.
    public override void OnInspectorGUI()
    {
        // Iscrtavanje zadanih kontrola u Inspectoru
        DrawDefaultInspector();

        // Dobivanje reference na instancu RagdollController-a koja je trenutno selektirana.
        RagdollController ragdoll = (RagdollController)target;

        // Ako korisnik klikne gumb, spremi (cache) rigidbody komponente ragdolla.
        if (GUILayout.Button("Cache ragdoll rigidbodies"))
        {
            ragdoll.CacheRagdollColliders();
        }
        // Ako korisnik klikne gumb, aktivira ragdoll simulaciju.
        if (GUILayout.Button("Activate Ragdoll"))
        {
            ragdoll.ActivateRagdoll();
        }
    }
}
#endif

// Glavna klasa koja upravlja ragdoll efektom na objektu.
public class RagdollController : MonoBehaviour
{
    // Reference na glavnu rigidbody komponentu ovog objekta.
    [SerializeField] private Rigidbody _myRigidbody;
    // Reference na kontroler animacija koji upravlja animacijama.
    [SerializeField] private AnimationController _animationController;
    // Reference na glavni collider (sudarnik) ovog objekta.
    [SerializeField] private Collider _mainCollider;
    // Korijenski GameObject koji sadrži sve dijelove ragdoll modela.
    [SerializeField] private GameObject _ragdollRootObject;
    // Lista rigidbody komponenti koje čine dijelove ragdolla.
    [SerializeField] private List<Rigidbody> _ragdollBodies;
    [SerializeField] private Transform _modelRoot;
   

    // Metoda koja se poziva pri inicijalizaciji objekta prije početka igre.
    private void Awake()
    {
        // Postavljanje svih ragdoll rigidbody komponenata na ne-kinematički način rada (omogućava simulaciju fizike).
        SetRagdollKinematic(false);
    }

    // Metoda kojom se dohvaćaju sve rigidbody komponente unutar ragdoll objekta i spremaju u listu.
    public void CacheRagdollColliders()
    {
        // Dohvaćanje svih rigidbody komponenata (uključujući i neaktivne) unutar _ragdollRootObject i spremanje u _ragdollBodies listu.
        _ragdollRootObject.GetComponentsInChildren(true, _ragdollBodies);
    }

    // Privatna metoda koja postavlja svojstvo isKinematic za sve rigidbody komponente ragdolla.
    private void SetRagdollKinematic(bool value)
    {
        // Prolazak kroz sve komponente u listi i postavljanje isKinematic svojstva.
        for (int i = 0; i < _ragdollBodies.Count; i++)
        {
            _ragdollBodies[i].isKinematic = value;
        }
    }

    // Metoda koja aktivira ragdoll efekte, isključujući kontrolu animacije i kolizije glavnog objekta.
    public void ActivateRagdoll()
    {
        // Onemogućavanje fizike glavnog objekta (postavljanje rigidbody na kinematički način rada).
        _myRigidbody.isKinematic = true;

        // Onemogućavanje Animator-a kako bi se isključile animacije.
        _animationController.Animator.enabled = false;

        // Onemogućavanje glavnog collider-a, tako da sudari ne ometaju ragdoll simulaciju.
        _mainCollider.enabled = false;

        _modelRoot.parent = null;

        // Postavljanje svih ragdoll rigidbody komponenata na ne-kinematički način rada, omogućujući simulaciju fizike.
        SetRagdollKinematic(false);
    }

    // Metoda koja simulira smrt objektom primjenom sile na dijelove ragdolla.
    // forcePosition - pozicija u kojoj se primjenjuje sila
    // force - jačina sile
    // radius - radijus unutar kojeg se sila primjenjuje
    public void Die(Vector3 forcePosition, float force, float radius)
    {
        // Prvo aktiviraj ragdoll simulaciju.
        ActivateRagdoll();

        Debug.Log(force);
        // Prolaz kroz sve rigidbody komponente u ragdoll listi.
        for (int i = 0; i < _ragdollBodies.Count; i++)
        {
            // Izračunaj udaljenost između trenutne komponente i pozicije primjene sile.
            Vector3 dist = _ragdollBodies[i].position - forcePosition;
            float magnitude = dist.magnitude;
            // Ako je objekt unutar zadanog radijusa...
            if (magnitude < radius)
            {
                // Primijeni impulsnu silu proporcionalnu udaljenosti (manja udaljenost = veći udar) na taj rigidbody.
                _ragdollBodies[i].AddForce((1 - (magnitude / radius)) * force * dist, ForceMode.Impulse);
            }
        }
    }
}
