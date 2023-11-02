using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedNPC  : SharedVariable<NPC>
{
    public static implicit operator SharedNPC(NPC value) => new SharedNPC { Value = value };
}