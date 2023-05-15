using BehaviorDesigner.Runtime.Tasks;

public class ChaseMob : Action
{
    public SharedMob Mob;
    public AnimalsMovement AnimalsMovement;

    public override TaskStatus OnUpdate()
    {
        AnimalsMovement.Run(Mob.Value.transform.position);
        return TaskStatus.Running;
    }
}