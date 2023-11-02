using BehaviorDesigner.Runtime.Tasks;


public class StoppedMoving : Action
{
    public SetMovement SetMovement;

    public override void OnStart()
    {
        SetMovement.Agent.destination = transform.position;
    }
}