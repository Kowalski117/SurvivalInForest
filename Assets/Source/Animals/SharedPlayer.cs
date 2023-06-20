using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedPlayer  : SharedVariable<Health>
{
    public static implicit operator SharedPlayer(Health value) => new SharedPlayer { Value = value };
}
