using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedEnemy  : SharedVariable<EnemyMovement>
{
    public static implicit operator SharedEnemy(EnemyMovement value) => new SharedEnemy() { Value = value };
}