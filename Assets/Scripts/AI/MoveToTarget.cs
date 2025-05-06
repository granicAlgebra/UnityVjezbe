using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToTarget : ActionNode
{
    public float Speed = 5f;
    public float StopDistance = 1f;
    public float Acceleration = 20f;

    protected override void OnStart() 
    {
        if (context.Target != null)
        {
            context.agent.speed = Speed;
            context.agent.isStopped = false;
            context.agent.acceleration = Acceleration;
            context.agent.SetDestination(context.Target.position);  
        }
    }


    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.Target == null)
        {
            return State.Failure;
        }

        context.agent.SetDestination(context.Target.position);

        if (!context.agent.pathPending && context.agent.remainingDistance <= StopDistance)
        {
            context.agent.isStopped = true;
            return State.Success;
        }

        return State.Running;
    }
}
