using UnityEngine;

/// <summary>
/// MoveWithPid kontrolira kretanje objekta pomoću PID kontrolera, omogućujući glatko ubrzanje i usporavanje prema željenoj brzini.
/// Na temelju ulaza (Horizontal, Vertical) računa se željena brzina, a PID algoritam izračunava silu koja se primjenjuje na Rigidbody.
/// Također, skripta omogućuje promjenu maksimalne brzine pritiskom tipki 1, 2 ili 3.
/// </summary>
public class MoveWithPid : MonoBehaviour
{
    // Referenca na Rigidbody komponentu koja se koristi za fiziku i primjenu sila
    [SerializeField] private Rigidbody _rigidBody;
    // Referenca na transformu karaktera, koja se koristi za rotaciju prema smjeru kretanja
    [SerializeField] private Transform _character;

    // Maksimalna sila koja se može dodati pri kretanju (ograničenje PID izlaza)
    [SerializeField] private float _maxAddForce = 30;
    // Maksimalna brzina kretanja koju želimo postići
    [SerializeField] private float _maxSpeed = 5f;

    // PID koeficijenti – potrebno ih fino podesiti za željeno ponašanje kretanja
    [Space, Header("P I D parametri")]
    [SerializeField] private float kp = 10f;   // Proporcionalni koeficijent
    [SerializeField] private float ki = 0.294f;  // Integralni koeficijent
    [SerializeField] private float kd = 0.05f;   // Derivativni koeficijent

    // Varijable za pohranu ulaznih vrijednosti (horizontalni i vertikalni input)
    private float _moveHorizontal;
    private float _moveVertical;
    // Trenutna brzina objekta (prikazana u Inspectoru kao read-only)
    [SerializeField, ReadOnly] private float _speed;

    // Ograničenja za integralnu komponentu PID kontrolera (sprječava integrator windup)
    float maxIntegral = 20f;
    float minIntegral = -20f;

    // Trenutna integralna vrijednost i posljednja pogreška, korištene u PID algoritmu
    private float integral = 20f;
    private float lastError;

    /// <summary>
    /// Awake metoda se poziva prije početka izvođenja skripte.
    /// Ovdje se dohvaća referenca na Rigidbody komponentu.
    /// </summary>
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Update metoda se poziva jednom po frame-u.
    /// U ovoj metodi se dohvaćaju ulazi od igrača i vrši se rotacija karaktera prema smjeru kretanja.
    /// </summary>
    private void Update()
    {
        GetInput(); // Dohvati horizontalni i vertikalni input
        Rotate();   // Rotiraj karakter prema smjeru kretanja
    }

    /// <summary>
    /// FixedUpdate se poziva u fiksnim vremenskim intervalima, idealno za fiziku.
    /// Ovdje se poziva metoda MoveWithPID koja računa i primjenjuje silu pomoću PID kontrolera.
    /// </summary>
    private void FixedUpdate()
    {
        MoveWithPID();
    }

    /// <summary>
    /// Dohvaća ulaze od igrača i omogućuje promjenu maksimalne brzine pomoću tipki 1, 2 i 3.
    /// </summary>
    private void GetInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");

        // Promijeni maksimalnu brzinu na temelju pritisnutih tipki (1 = 2, 2 = 4, 3 = 6)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _maxSpeed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _maxSpeed = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _maxSpeed = 6;
        }
    }

    /// <summary>
    /// Rotira karakter tako da se gleda u smjeru kretanja (na osnovu horizontalne komponente brzine).
    /// Koristi se Quaternion.Slerp za glatku interpolaciju između trenutne i željene rotacije.
    /// </summary>
    private void Rotate()
    {
        // Dobij trenutni smjer kretanja, zanemarujući vertikalnu komponentu
        Vector3 dir = _rigidBody.velocity.normalized;
        dir.y = 0;

        // Ako je brzina dovoljno velika, postavi novu rotaciju karaktera
        if (dir.sqrMagnitude > 0.01f)
        {
            _character.rotation = Quaternion.Slerp(_character.rotation, Quaternion.LookRotation(dir, Vector3.up), 0.1f);
        }
    }

    /// <summary>
    /// Računa i primjenjuje silu na Rigidbody pomoću PID kontrolera.
    /// Na osnovu ulaznih vrijednosti i trenutne brzine izračunava se pogreška, a zatim PID algoritam generira odgovarajući izlaz (silu).
    /// Ta sila se primjenjuje u smjeru ulaza.
    /// </summary>
    private void MoveWithPID()
    {
        // Kreiramo vektor smjera ulaza (kombinacija horizontalnog i vertikalnog inputa)
        Vector3 inputDirection = new Vector3(_moveHorizontal, 0, _moveVertical);

        // Početna željena brzina je definirana s _maxSpeed
        float maxSpeed = _maxSpeed;
        // Ako nema značajnog ulaza (inputDirection je gotovo nula), postavi željenu brzinu na 0
        if (inputDirection.sqrMagnitude < 0.01f)
            maxSpeed = 0;

        // Normaliziramo vektor ulaza kako bismo dobili samo smjer
        inputDirection.Normalize();

        // Izračunavamo trenutnu horizontalnu brzinu (bez vertikalne komponente)
        Vector3 horizontalVelocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        // Spremljiva brzina objekta (uključujući sve komponente) - koristi se za PID kalkulaciju
        _speed = _rigidBody.velocity.magnitude;

        // PID kontrola: Izračunavamo pogrešku kao razliku između željene brzine i trenutne brzine
        float error = maxSpeed - _speed;
        // Izračunaj izlaznu silu koristeći PID funkciju
        float outputForce = PID(error);
        // Ograniči izlaznu silu tako da ne prelazi maksimalnu dozvoljenu vrijednost
        outputForce = Mathf.Clamp(outputForce, 0, _maxAddForce);

        // Primjeni silu na Rigidbody u smjeru ulaza
        _rigidBody.AddForce(inputDirection * outputForce, ForceMode.Force);
    }

    /// <summary>
    /// PID funkcija koja izračunava izlaz na temelju pogreške.
    /// Integralna komponenta se akumulira i ograničava kako bi se spriječilo prekomjerno nakupljanje (integral windup).
    /// Derivativna komponenta se računa kao promjena pogreške kroz vrijeme.
    /// Kombinacijom proporcionalne, integralne i derivativne komponente dobiva se izlaz.
    /// </summary>
    /// <param name="error">Razlika između željene brzine i trenutne brzine</param>
    /// <returns>Izlazna vrijednost (sila) koju treba primijeniti</returns>
    private float PID(float error)
    {
        // Akumuliraj integral pogreške, ograničavajući je između minIntegral i maxIntegral
        integral = Mathf.Clamp(integral + error * Time.fixedDeltaTime, minIntegral, maxIntegral);
        // Izračunaj derivativnu komponentu (brzina promjene pogreške)
        float derivative = (error - lastError) / Time.fixedDeltaTime;
        // Kombiniraj PID komponente
        float output = kp * error + ki * integral + kd * derivative;
        // Spremi trenutnu pogrešku za izračun derivata u sljedećem ciklusu
        lastError = error;
        return output;
    }
}

