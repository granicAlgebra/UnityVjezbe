using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;

    [SerializeField] private float _walkSpeed = 20f;
    [SerializeField] private float _jumpVelocity = 7f;

    [SerializeField] private LayerMask _layerMaskJump;

    [SerializeField] private string _rampName = "Ramp";
    [SerializeField] private string _pickupable = "Pickupable";

    private float _moveHorizontal;
    private float _moveVertical;
    private bool _jump;

    private RaycastHit[] _hitResults = new RaycastHit[5];

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        Jump();
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
        _jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void Move()
    {
        if (Physics.RaycastNonAlloc(transform.position, -Vector3.up, _hitResults, 1.15f, _layerMaskJump) > 0)
        {
            Vector3 direction = new Vector3(_moveHorizontal, 0, _moveVertical);
            _rigidBody.AddForce(direction * _walkSpeed, ForceMode.Force);
        }          
    }

    private void Jump()
    {
        if (!_jump)
        {
            return;
        }

        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, 1.15f, _layerMaskJump))
        {
            if (hitInfo.collider != null)
            {
                _rigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
            }
        }
    }
}
