using BehaviorDesigner.Runtime.Tasks;

public class Eat : Action
{
    public SharedMob Mob;
    
    public override TaskStatus OnUpdate()
    {
        if ((Mob.Value.Agent.pathEndPosition - Mob.Value.transform.position).magnitude == 0)
        {
            Mob.Value.Eat();
            return TaskStatus.Inactive;
        }
        return TaskStatus.Running;
    }
}
