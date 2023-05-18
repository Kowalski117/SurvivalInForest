using BehaviorDesigner.Runtime.Tasks;

public class ChaseCharacter : Action
{
    public SharedCharacter Character;
    public AnimalsMovement AnimalsMovement;

    public override TaskStatus OnUpdate()
    {
        AnimalsMovement.Run(Character.Value.transform.position);
        return TaskStatus.Running;
    }
}
