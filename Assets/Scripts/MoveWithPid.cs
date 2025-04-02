using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class MoveWithPid : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _character;

    [SerializeField] private float _maxAddForce = 30;
    [SerializeField] private float _maxSpeed = 5f;

    // PID koeficijenti – potrebno ih fino podesiti
    [SerializeField] private float kp = 10f;
    [SerializeField] private float ki = 0.294f;
    [SerializeField] private float kd = 0.05f;

    private float _moveHorizontal;
    private float _moveVertical;
    [SerializeField] private float _speed;
    float maxIntegral = 20f;
    float minIntegral = -20f;

    private float integral = 20f;
    private float lastError;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        Rotate();
    }

    private void FixedUpdate()
    {
        MoveWithPID();
    }

    private void GetInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
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

    private void Rotate()
    {
        Vector3 dir = _rigidBody.velocity.normalized;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            _character.rotation = Quaternion.Slerp(_character.rotation, Quaternion.LookRotation(dir, Vector3.up), 0.1f);
        }

        //_rigidBody.rotation *= Quaternion.Euler(0, _mouseHorizontal * _mouseSensivity * Time.deltaTime, 0);
    }

    private void MoveWithPID()
    {
        // Kreiramo vektor smjera ulaza
        Vector3 inputDirection = new Vector3(_moveHorizontal, 0, _moveVertical);

        float maxSpeed = _maxSpeed;
        // Ako nema značajnog ulaza, nema potrebe za primjenom sile
        if (inputDirection.sqrMagnitude < 0.01f)
            maxSpeed = 0;

        inputDirection.Normalize();

        // Izračunaj trenutnu horizontalnu brzinu u smjeru ulaza
        Vector3 horizontalVelocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        _speed = _rigidBody.velocity.magnitude;

        // PID kontrola
        float error = maxSpeed - _speed;
        float outputForce = PID(error);
        outputForce = Mathf.Clamp(outputForce, 0, _maxAddForce);

        // Primjena sile u smjeru ulaza
        _rigidBody.AddForce(inputDirection * outputForce, ForceMode.Force);
    }

    private float PID(float error)
    {
        integral = Mathf.Clamp(integral + error * Time.fixedDeltaTime, minIntegral, maxIntegral);
        float derivative = (error - lastError) / Time.fixedDeltaTime;
        float output = kp * error + ki * integral + kd * derivative;
        lastError = error;
        return output;
    }
}
