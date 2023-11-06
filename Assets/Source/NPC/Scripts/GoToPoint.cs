using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class GoToPoint : Action
{
    [SerializeField] private SetMovement _setMovement;
    [SerializeField] private SharedTransform _point;
    
    public override void OnStart()
    {
        _setMovement.Walk(_point.Value.position);
    }
}
