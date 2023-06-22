using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedMob : SharedVariable<MobMovement>
{
    public static implicit operator SharedMob(MobMovement value) => new SharedMob {Value = value};
}