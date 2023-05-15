using System;
using BehaviorDesigner.Runtime;

[Serializable]
public class SharedCharacter  : SharedVariable<Character>
{
    public static implicit operator SharedCharacter(Character value) => new SharedCharacter { Value = value };
}
