using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class TurnToPlayer : Action
{
    public SharedPlayer Player;
    public SharedEnemy Enemy;
    public float SpeedRotate;
    
    public override TaskStatus OnUpdate()
    {
        Enemy.Value.transform.DOLookAt(Player.Value.transform.position, SpeedRotate, axisConstraint: AxisConstraint.Y);
            return TaskStatus.Failure;
    }
}
