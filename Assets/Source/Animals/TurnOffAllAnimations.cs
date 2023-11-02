using BehaviorDesigner.Runtime.Tasks;

public class TurnOffAllAnimations : Action
{
    public SetMovement SetMovement;

    public override void OnStart()
    {
        SetMovement.TurnOffAnimations();
    }

}
