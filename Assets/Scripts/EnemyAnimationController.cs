using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navmeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _maxSpeed;

    private int _movementHash;
    private int _attackHash;

    void Start()
    {
        _movementHash = Animator.StringToHash("Movement");
        _attackHash = Animator.StringToHash("Attack");
    }

    void Update()
    {
        if (_navmeshAgent != null)
        {
            _animator.SetFloat(_movementHash, _navmeshAgent.velocity.magnitude / _maxSpeed);

        }
    }

    public void Attack()
    {
        _animator.SetTrigger(_attackHash);
    }
}
