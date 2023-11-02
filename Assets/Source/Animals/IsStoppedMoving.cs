using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsStoppedMoving : Action
{
    [SerializeField] private float _minMagnitude;
    
    public SetMovement SetMovement;

    public override TaskStatus OnUpdate()
    {
        if ((SetMovement.Agent.pathEndPosition - SetMovement.transform.position).magnitude <= _minMagnitude)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }
}
