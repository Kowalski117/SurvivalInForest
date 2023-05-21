using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Attack : Action
{
    public SharedEnemy Enemy;

    public override TaskStatus OnUpdate()
    {
        Enemy.Value.Attack();
        return TaskStatus.Success;
    }
}