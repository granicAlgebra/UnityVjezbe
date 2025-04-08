using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Referenca na Rigidbody komponentu za fiziku
    [SerializeField] private Rigidbody _rigidBody;
    // Referenca na AnimationController za upravljanje animacijama
    [SerializeField] private AnimationController _animationController;
    // Sila koja se primjenjuje pri kretanju igrača
    [SerializeField] private float _movementForce = 50f;
    // Vrijednost drag-a (otpora) kada je igrač na tlu
    [SerializeField] private float _dragOnGround = 5;
    // Maksimalna brzina sprintanja
    [SerializeField] private float _sprintSpeed = 6;
    // Maksimalna brzina trčanja
    [SerializeField] private float _runSpeed = 4f;
    // Maksimalna brzina hodanja
    [SerializeField] private float _walkSpeed = 1.5f;
    // Impulzna sila za skok
    [SerializeField] private float _jumpVelocity = 7f;
    // Osjetljivost miša za rotaciju
    [SerializeField] private float _mouseSensivity = 300;
    // Transform referenca na lik (karakter) koji se rotira
    [SerializeField] private Transform _character;

    // Layer maska koja se koristi za provjeru kontakta s tlom (skakanje)
    [SerializeField] private LayerMask _layerMaskJump;

    // Naziv objekta "Ramp" za specifične kolizijske efekte (npr. impuls na rampi)
    [SerializeField] private string _rampName = "Ramp";

    // Varijable za spremanje inputa igrača
    private float _moveHorizontal;
    private float _moveVertical;
    private float _mouseHorizontal;
    private bool _jump;
    private bool _wallk;
    private bool _sprint;
    private bool _attack;

    // Polje za spremanje rezultata raycast operacija (optimizacija – smanjuje stvaranje novih objekata)
    private RaycastHit[] _hitResults = new RaycastHit[5];

    // Awake se poziva prije bilo koje druge metode, inicijalizira se Rigidbody i postavlja kursor
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        // Zaključaj i sakrij kursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update se poziva jednom po frame-u, ovdje se prikupljaju inputi i upravlja rotacijom te skokom
    private void Update()
    {
        GetInput(); // Dohvati korisničke ulaze (tipkovnica, miš)
        Jump();     // Provjeri i izvrši skok ako je potrebno
        Rotate();   // Rotiraj igrača na osnovu brzine i pokreta miša
        Attack();
    }

    // FixedUpdate se koristi za fizičke operacije (primjena sile, kretanje)
    private void FixedUpdate()
    {
        Move(); // Primijeni silu za kretanje igrača
    }

    // OnCollisionEnter se poziva kada dođe do sudara s nekim objektom
    private void OnCollisionEnter(Collision collision)
    {
        // Ako se sudar dogodio s objektom koji ima tag _rampName (npr. "Ramp")
        if (collision.collider.CompareTag(_rampName))
        {
            // Koristi normalu kontakta kako bi odredio smjer dodatnog impulsa
            Vector3 direction = collision.contacts[0].normal;
            // Primijeni impulsni skok u tom smjeru (pomnožen s _jumpVelocity i faktorom 2)
            _rigidBody.AddForce(direction * _jumpVelocity * 2, ForceMode.Impulse);
        }
    }

    // Metoda za dohvaćanje inputa s tipkovnice i miša
    private void GetInput()
    {
        // Horizontalni i vertikalni input (W, A, S, D ili strelice)
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
        // Input pokreta miša po horizontalnoj osi
        _mouseHorizontal = Input.GetAxis("Mouse X");
        // Detekcija skoka (Space tipka)
        _jump = Input.GetKeyDown(KeyCode.Space);
        // Hodanje (npr. pritiskom tipke Left Alt)
        _wallk = Input.GetKey(KeyCode.LeftAlt);
        // Sprintanje (npr. pritiskom tipke Left Shift)
        _sprint = Input.GetKey(KeyCode.LeftShift);

        _attack = Input.GetMouseButtonDown(0);
    }

    // Metoda za rotaciju igrača
    private void Rotate()
    {
        // Izračunaj smjer kretanja na temelju trenutne brzine (normalizirano)
        Vector3 dir = _rigidBody.velocity.normalized;
        dir.y = 0; // Isključi vertikalnu komponentu kako bi se rotacija dogodila samo u horizontalnoj ravnini
        if (dir.sqrMagnitude > 0.01f)
        {
            // Glatka rotacija karaktera prema smjeru kretanja koristeći Slerp
            _character.rotation = Quaternion.Slerp(_character.rotation, Quaternion.LookRotation(dir, Vector3.up), 0.1f);
        }

        // Rotiraj Rigidbody na osnovu inputa miša
        _rigidBody.rotation *= Quaternion.Euler(0, _mouseHorizontal * _mouseSensivity * Time.deltaTime, 0);
    }

    // Metoda za kretanje igrača
    private void Move()
    {
        // Izračunaj željeni smjer kretanja kombinirajući horizontalni i vertikalni input
        Vector3 direction = transform.right * _moveHorizontal + transform.forward * _moveVertical;

        // Odredi maksimalnu brzinu ovisno o načinu kretanja (hodanje, trčanje ili sprint)
        float maxSpeed = _runSpeed;
        if (_wallk)
        {
            maxSpeed = _walkSpeed;
        }
        if (_sprint)
        {
            maxSpeed = _sprintSpeed;
        }

        // Provjeri je li igrač na tlu koristeći RaycastNonAlloc (optimizirana verzija raycasta)
        if (Physics.RaycastNonAlloc(transform.position, -Vector3.up, _hitResults, 1.2f, _layerMaskJump) > 0)
        {
            // Ako je igrač na tlu, aktiviraj animaciju pada i postavi drag
            _animationController.Falling(true);
            _rigidBody.drag = _dragOnGround;
        }
        else
        {
            // Ako igrač nije na tlu, deaktiviraj animaciju pada i ukloni drag
            _animationController.Falling(false);
            _rigidBody.drag = 0;
        }

        // Ako je trenutna brzina manja od maksimalne, primijeni dodatnu silu u smjeru kretanja
        if (_rigidBody.velocity.magnitude < maxSpeed)
        {
            _rigidBody.AddForce(direction * _movementForce, ForceMode.Force);
        }
    }

    private void Attack()
    {
        if (_attack)
            _animationController.PlayAttack();
    }

    // Metoda za izvođenje skoka
    private void Jump()
    {
        // Ako nije detektiran input za skok, izađi iz metode
        if (!_jump)
        {
            return;
        }

        // Provjeri je li igrač na tlu pomoću raycasta
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, 1.2f, _layerMaskJump))
        {
            // Ako raycast detektira kolajder, izvrši skok
            if (hitInfo.collider != null)
            {
                // Primijeni impuls za skok
                _rigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
                // Aktiviraj animaciju skoka
                _animationController.Jump();
            }
        }
    }
}