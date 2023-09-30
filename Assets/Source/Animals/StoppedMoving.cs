using BehaviorDesigner.Runtime.Tasks;


public class StoppedMoving : Action
{
    public AnimalsMovement Animals;

    public override void OnStart()
    {
        Animals.Agent.destination = transform.position;
    }
}