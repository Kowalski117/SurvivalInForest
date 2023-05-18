using BehaviorDesigner.Runtime.Tasks;

public class ChaseMob : Action
{
    public SharedMob Mob;
    public SharedEnemy Enemy;

    public override TaskStatus OnUpdate()
    {
        var heading = Mob.Value.transform.position - Enemy.Value.transform.position;
        var distans = heading.magnitude;
        
        if (distans <=2)
        {
            return TaskStatus.Success;
        }
        else
        {
            Enemy.Value.Run(Mob.Value.transform.position);
        }
        return TaskStatus.Running;
    }
}