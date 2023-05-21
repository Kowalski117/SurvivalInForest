using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Howl : Action
{
    public SharedEnemy Enemy;
    
    public override TaskStatus OnUpdate()
    {
        Enemy.Value.Howl();
        return TaskStatus.Inactive;
    }
}
