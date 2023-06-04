using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Howl : Action
{
    public SharedEnemy Enemy;
    public SharedBool IsAttacking;

    public override TaskStatus OnUpdate()
    {
        Debug.Log(IsAttacking.Value);
        if (IsAttacking.Value == false)
        {
            Enemy.Value.Howl();
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
        
    }
}
