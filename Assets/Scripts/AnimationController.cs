using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _smooth;
    private Animator _animator;
    private Rigidbody _rigidBody;

    private Dictionary<string, int> _paramHashes = new Dictionary<string, int>();   

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); 
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move(_rigidBody.velocity.magnitude);
    }

    public void Jump()
    {
        _animator.SetTrigger(GetHash("Jump"));
    }

    public void Move(float speed)
    {
        _animator.SetFloat(GetHash("Movement"), speed / _maxMovementSpeed, _smooth, Time.deltaTime);
    }

    public void Falling(bool isOnGround)
    {
        _animator.SetBool(GetHash("IsOnGround"), isOnGround);
    }

    private int GetHash(string key)
    {
        if (!_paramHashes.TryGetValue(key, out var hash))
        {
            hash = Animator.StringToHash(key);
            _paramHashes.Add(key, hash);
        }
        return hash;
    }
}
