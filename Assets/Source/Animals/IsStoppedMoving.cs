using BehaviorDesigner.Runtime.Tasks;


public class IsStoppedMoving : Action
{
    public AnimalsMovement Animals;
    
    public override TaskStatus OnUpdate()
    {
        if ((Animals.Agent.pathEndPosition - Animals.transform.position).magnitude == 0)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }
}
