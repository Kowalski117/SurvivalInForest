using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine.AI;

public class TurnAwayFromObject : Action
{
    public SharedTransform Transform;
    public SharedNPC Npc;
    public float SpeedRotate;

    public override void OnStart()
    {
        Npc.Value.transform.DORotateQuaternion(Transform.Value.rotation,SpeedRotate);
    }
}
