using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class ChasePlayer : Action
{
    public SharedPlayer Player;
    public SharedEnemy Enemy;
    public float SpeedRotate;

    public override TaskStatus OnUpdate()
    {
        var distans = Vector3.Distance(Enemy.Value.transform.position, Player.Value.transform.position);
        
        if (distans <= Enemy.Value.AttackDistans)
        {
            Enemy.Value.Run(Enemy.Value.transform.position);
            Enemy.Value.transform.DOLookAt(Player.Value.transform.position, SpeedRotate,
                axisConstraint: AxisConstraint.Y);
            return TaskStatus.Success;
        }
        else
        {
            Enemy.Value.Run(Player.Value.transform.position);
        }

        return TaskStatus.Running;
    }
}