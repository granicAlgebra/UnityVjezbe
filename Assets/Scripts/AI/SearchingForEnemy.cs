using UnityEngine;
using TheKiwiCoder;

public class SearchingForEnemy : ActionNode
{
    public LayerMask EnemyLayer;
    public string EnemyTag;
    public float ViewRadius = 12;
    [Range(0, 180)] public float ViewAngle = 120;
    public float CheckInterval = 0.25f;

    private float _nextCheck;
    private LayerMask _raycastLayerMask = ~0;

    protected override void OnStart() 
    {
        _nextCheck = 0;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (Time.time < _nextCheck)
        {
            return State.Running;
        }
        _nextCheck = Time.time + CheckInterval;

        Collider[] hits = Physics.OverlapSphere(context.transform.position, ViewRadius, EnemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].CompareTag(EnemyTag))
            {
               continue;
            }

            Vector3 dir = (hits[i].transform.position - context.transform.position).normalized;

            if (Vector3.Angle(context.transform.forward, dir) > ViewAngle * 0.5f)
            {
                continue;
            }

            if (Physics.Raycast(context.transform.position + Vector3.up * 0.5f, dir, out RaycastHit hitInfo, ViewRadius, _raycastLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider == hits[i])
                {
                    context.Target = hits[i].transform;
                    return State.Success;
                }
            }
        }

        return State.Failure;
    }
}
