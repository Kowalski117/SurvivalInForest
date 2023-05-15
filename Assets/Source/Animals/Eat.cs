using BehaviorDesigner.Runtime.Tasks;

public class Eat : Action
{
    public SharedMob SelfMob;
    
    public override TaskStatus OnUpdate()
    {
        if ((SelfMob.Value.Agent.pathEndPosition - SelfMob.Value.transform.position).magnitude == 0)
        {
            SelfMob.Value.Eat();
            return TaskStatus.Inactive;
        }
        return TaskStatus.Running;
    }
}
