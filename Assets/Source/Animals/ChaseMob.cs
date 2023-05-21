using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class ChaseMob : Action
{
    public SharedMob Mob;
    public SharedEnemy Enemy;
    public float SpeedRotate;

    public override TaskStatus OnUpdate()
    {
        var distans = Vector3.Distance(Enemy.Value.transform.position, Mob.Value.transform.position);

        if (distans <= Enemy.Value.AttackDistans)
        {
            Enemy.Value.Run(Enemy.Value.transform.position);
            Enemy.Value.transform.DOLookAt(Mob.Value.transform.position, SpeedRotate);
            return TaskStatus.Success;
        }
        else
        {
            Enemy.Value.Run(Mob.Value.transform.position);
        }

        return TaskStatus.Running;
    }
}