using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;

    [SerializeField] private float _walkSpeed = 20f;
    [SerializeField] private float _jumpVelocity = 7f;

    private float _moveHorizontal;
    private float _moveVertical;
    private bool _jump;


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

    private void GetInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
        _jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_moveHorizontal, 0, _moveVertical);
        _rigidBody.AddForce(direction * _walkSpeed, ForceMode.Force);
    }

    private void Jump()
    {
        if (!_jump)
        {
            return;
        }

        _rigidBody.AddForce(Vector3.up * _jumpVelocity, ForceMode.Impulse);
    }

}
