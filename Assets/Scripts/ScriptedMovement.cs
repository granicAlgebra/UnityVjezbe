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
    private bool _isOpen;

    private Coroutine _moveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _isOpen = false;

        //transform.DOMove(_endPosition, _timeToMove).SetEase(Ease.OutQuint);
        //StartCoroutine(MoveRoutine());
    }

    public void OpenDoor()
    {
        if (_moveCoroutine == null)
        {
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }
        else
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = StartCoroutine(MoveRoutine());
        }
    }

    private IEnumerator MoveRoutine()
    {
        _isOpen = !_isOpen;
        float time = 0;
        while (time < _timeToMove)
        {
            time += Time.deltaTime;

            //transform.position = Vector3.Lerp(_startPosition, _endPosition, time / _timeToMove);
            //transform.position = Vector3.Lerp(_startPosition, _endPosition, CubicIn(time / _timeToMove));
            //transform.position = Vector3.Lerp(_startPosition, _endPosition, _animationCurve.Evaluate(time / _timeToMove));
            if (_isOpen)
            {
                transform.position = Vector3.Lerp(transform.position, _startPosition, CubicIn(time / _timeToMove));
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, _endPosition, CubicIn(time / _timeToMove));
            }

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
