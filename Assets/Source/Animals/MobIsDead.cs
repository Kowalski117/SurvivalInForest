using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MobIsDead : Action
{
    public SharedMob Mob;
    
    public override TaskStatus OnUpdate()
    {
        if (Mob.Value.GetComponent<Animals>().IsDead)
            return TaskStatus.Failure;
        else
            return TaskStatus.Success;
        
    }
}