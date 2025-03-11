using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform Target;
    public Transform LookTarget;

    public float Smoothnes = 0.5f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, Smoothnes * Time.deltaTime);
        Vector3 direction = LookTarget.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
