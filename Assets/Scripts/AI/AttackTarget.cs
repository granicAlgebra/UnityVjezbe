using UnityEngine;
using TheKiwiCoder;
using DG.Tweening;

public class AttackTarget : ActionNode
{
    public float AttackRate = 2f;
    public float AttackRange = 1.5f;
    public int Damage = 20;

    private Entity _target;
    private float _nextAttack;
    private Tween _attacking;

    protected override void OnStart() 
    {
        _target = context.Target.GetComponent<Entity>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (context.Target == null ||
            !context.Target.gameObject.activeSelf ||
            (context.Target.position - context.transform.position).magnitude > AttackRange ||
            _target.GetParam(ParamType.Health).Value <= 0)
        {
            return State.Failure;
        }

        Vector3 dir = (context.Target.position - context.transform.position).normalized;
        context.transform.rotation = Quaternion.Lerp(context.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (Time.time >= _nextAttack)
        {
            _nextAttack = Time.time + AttackRate;
            context.EnemyAnimationController.Attack();

            if (_attacking != null && _attacking.active)
            {
                _attacking.Kill();
            }

            _attacking = DOVirtual.DelayedCall(1, () =>
            {
                if ((context.Target.position - context.transform.position).magnitude <= AttackRange)
                {
                    var health = _target.GetParam(ParamType.Health);
                    _target.ChangeParam(ParamType.Health, -Damage);
                    _attacking = null;
                }
            });
        }

        return State.Running;
    }
}
