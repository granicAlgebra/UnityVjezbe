using UnityEngine;

/// <summary>
/// CameraMovement kontrolira poziciju i rotaciju kamere u sceni.
/// Ova skripta omogućava glatko kretanje kamere prema ciljanom objektu (Target) te opcionalno gledanje prema drugom objektu (LookTarget).
/// U ovom primjeru, zakomentirani su alternativni načini postizanja glatkog kretanja i usmjeravanja kamere.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    // Transform cilja kojem kamera treba slijediti (npr. igrač)
    public Transform Target;
    // Transform cilja prema kojem kamera gleda, ako se koristi drugačiji fokus od pozicije cilja
    public Transform LookTarget;

    // Faktor zaglađivanja kretanja kamere
    public float Smoothnes = 0.5f;
    // Varijabla koja se koristi u SmoothDamp metodi za glatko kretanje
    private Vector3 _velocity;

    /// <summary>
    /// LateUpdate se poziva nakon Update metoda svih ostalih skripti, što osigurava da se kamera pomakne nakon što su svi objekti ažurirani.
    /// </summary>
    void LateUpdate()
    {
        // Primjer 1: Korištenje SmoothDamp za glatko pomicanje kamere prema Target poziciji
        // transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Smoothnes * Time.deltaTime);
        //
        // Postavljanje rotacije kamere tako da gleda prema LookTarget objektu
        // Vector3 direction = LookTarget.position - transform.position;
        // transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        // Trenutni način: Postavljanje pozicije i rotacije kamere direktno na Targetovu poziciju i rotaciju.
        // Ovaj pristup ne koristi glatko prijelazanje, već odmah postavlja kameru na cilj.
        transform.SetPositionAndRotation(Target.position, Target.rotation);
    }
}
