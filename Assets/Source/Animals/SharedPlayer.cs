using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedPlayer  : SharedVariable<Player>
{
    public static implicit operator SharedPlayer(Player value) => new SharedPlayer { Value = value };
}
