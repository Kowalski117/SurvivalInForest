using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedPlayer  : SharedVariable<PlayerHealth>
{
    public static implicit operator SharedPlayer(PlayerHealth value) => new SharedPlayer { Value = value };
}
