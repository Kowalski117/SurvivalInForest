using BehaviorDesigner.Runtime.Tasks;

public class Sit : Action
{
    public SharedMob SelfMob;

    public override TaskStatus OnUpdate()
    {
        if ((SelfMob.Value.Agent.pathEndPosition - SelfMob.Value.transform.position).magnitude == 0)
        {
            SelfMob.Value.Sit();
            return TaskStatus.Inactive;
        }

        return TaskStatus.Running;
    }
}