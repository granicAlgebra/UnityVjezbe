using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScriptedMovement : MonoBehaviour
{
    [SerializeField] Transform _targert;
    [SerializeField] Vector3 _endPosition;
    [SerializeField] private float _timeToMove = 2f;

    [SerializeField] private float _smoothSpeed = 2f;

    [SerializeField] private AnimationCurve _animationCurve;

    private Vector3 _startPosition;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;

        //transform.DOMove(_endPosition, _timeToMove).SetEase(Ease.OutQuint);
        //StartCoroutine(MoveRoutine());
    }

    public void Hodor()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float time = 0;
        while (time < _timeToMove)
        {
            time += Time.deltaTime;

            //transform.position = Vector3.Lerp(_startPosition, _endPosition, time / _timeToMove);
            //transform.position = Vector3.Lerp(_startPosition, _endPosition, CubicIn(time / _timeToMove));
            transform.position = Vector3.Lerp(_startPosition, _endPosition, _animationCurve.Evaluate(time / _timeToMove));

            yield return null;
        }
    }

    //private void Update()
    //{
    //    //transform.position = Vector3.Lerp(transform.position, _targert.position, Time.deltaTime * _smoothSpeed); 
    //    //transform.position = Vector3.MoveTowards(transform.position, _targert.position, Time.deltaTime * _smoothSpeed); 
    //    transform.position = Vector3.SmoothDamp(transform.position, _targert.position, ref _velocity, Time.deltaTime * _smoothSpeed); 
    //}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_endPosition, transform.localScale);
    }
#endif
    private float CubicIn(float t)
    {
        return t * t * t;
    }

}
