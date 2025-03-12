using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;
    public Transform LookTarget;

    public float Smoothnes = 0.5f;
    private Vector3 _velocity;

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Smoothnes * Time.deltaTime);
        Vector3 direction = LookTarget.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
