using BehaviorDesigner.Runtime.Tasks;

public class Attack : Action
{
    public SharedEnemy Enemy;

    public override TaskStatus OnUpdate()
    {
        Enemy.Value.Attack();
        return TaskStatus.Success;
    }
}