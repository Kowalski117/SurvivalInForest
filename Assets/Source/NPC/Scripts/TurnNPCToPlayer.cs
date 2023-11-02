using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class TurnNPCToPlayer : Action
{
    public SharedPlayer Player;
    public SharedNPC Npc;
    public float SpeedRotate;

    public override TaskStatus OnUpdate()
    {
        Npc.Value.transform.DOLookAt(Player.Value.transform.position, SpeedRotate,
            axisConstraint: AxisConstraint.Y);
        return TaskStatus.Failure;
    }
}