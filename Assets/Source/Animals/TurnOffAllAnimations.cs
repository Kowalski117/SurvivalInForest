using BehaviorDesigner.Runtime.Tasks;

public class TurnOffAllAnimations : Action
{
    public AnimalsMovement Animals;

    public override void OnStart()
    {
        Animals.TurnOffAnimations();
    }

}
