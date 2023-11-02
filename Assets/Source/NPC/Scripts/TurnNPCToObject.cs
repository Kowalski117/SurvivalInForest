using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine.AI;

public class TurnNPCToObject : Action
{
    public SharedTransform Transform;
    public SharedNPC Npc;
    public float SpeedRotate;

    public override void OnStart()
    {
        Npc.Value.transform.DOLookAt(Transform.Value.position,SpeedRotate);
    }
}
