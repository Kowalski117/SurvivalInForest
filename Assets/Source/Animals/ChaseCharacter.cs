using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class ChaseCharacter : Action
{
    public SharedCharacter Character;
    public SharedEnemy Enemy;
    public float SpeedRotate;

    public override TaskStatus OnUpdate()
    {
        var distans = Vector3.Distance(Enemy.Value.transform.position, Character.Value.transform.position);

        if (distans <= Enemy.Value.AttackDistans)
        {
            Enemy.Value.Run(Enemy.Value.transform.position);
            Enemy.Value.transform.DOLookAt(Character.Value.transform.position, SpeedRotate);
            return TaskStatus.Success;
        }
        else
        {
            Enemy.Value.Run(Character.Value.transform.position);
        }

        return TaskStatus.Running;
    }
}