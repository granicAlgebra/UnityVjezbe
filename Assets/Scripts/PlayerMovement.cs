using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private float _movementForce = 50f;
    [SerializeField] private float _dragOnGround = 5;
    [SerializeField] private float _sprintSpeed = 6;
    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _walkSpeed = 1.5f;
    [SerializeField] private float _jumpVelocity = 7f;
    [SerializeField] private float _mouseSensivity = 300;
    [SerializeField] private Transform _character;

    [SerializeField] private LayerMask _layerMaskJump;

    [SerializeField] private string _rampName = "Ramp";
    [SerializeField] private string _pickupable = "Pickupable";
    

    private float _moveHorizontal;
    private float _moveVertical;
    private float _mouseHorizontal;
    private bool _jump;
    private bool _wallk;
    private bool _sprint;

    private RaycastHit[] _hitResults = new RaycastHit[5];

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Update()
    {
        GetInput();
        Jump();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(_rampName))
        {
            Vector3 direction = collision.contacts[0].normal;
            _rigidBody.AddForce(direction * _jumpVelocity * 2, ForceMode.Impulse);
        }
    }

    private void GetInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
        _mouseHorizontal = Input.GetAxis("Mouse X");
        _jump = Input.GetKeyDown(KeyCode.Space);
        _wallk = Input.GetKey(KeyCode.LeftAlt);
        _sprint = Input.GetKey(KeyCode.LeftShift);
    }

    private void Rotate()
    {
        Vector3 dir = _rigidBody.velocity.normalized;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            _character.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        _rigidBody.rotation *= Quaternion.Euler(0, _mouseHorizontal * _mouseSensivity * Time.deltaTime, 0);
    }

    private void Move()
    {
        Vector3 direction = transform.right * _moveHorizontal + transform.forward * _moveVertical;

        float maxSpeed = _runSpeed;
        if (_wallk)
        {
            maxSpeed = _walkSpeed;
        }
        if (_sprint)
        {
            maxSpeed = _sprintSpeed;
        }

        if (Physics.RaycastNonAlloc(transform.position, -Vector3.up, _hitResults, 1.2f, _layerMaskJump) > 0)
        {            
            _animationController.Falling(true);
            _rigidBody.drag = _dragOnGround;
        }
        else
        {
            _animationController.Falling(false);
            _rigidBody.drag = 0;
        }

        if (_rigidBody.velocity.magnitude < maxSpeed)
        {
            _rigidBody.AddForce(direction * _movementForce, ForceMode.Force);
        }
    }

    private void Jump()
    {
        if (!_jump)
        {
            return;
        }

        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, 1.2f, _layerMaskJump))
        {
            if (hitInfo.collider != null)
            {
                _rigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
                _animationController.Jump();
            }
        }
    }
}
