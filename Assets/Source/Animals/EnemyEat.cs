using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyEat : Action
{
    public SharedEnemy Enemy;
    public SharedMob Mob;
    
    public override TaskStatus OnUpdate()
    {
        if (Mob.Value.GetComponent<Animals>().IsDead)
        {
            Enemy.Value.Eat();
            return TaskStatus.Failure;
        }
        else
        {
            return TaskStatus.Success;
        }
    }
}