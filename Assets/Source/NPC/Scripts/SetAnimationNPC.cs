
public class SetAnimationNPC : SetAnimation
{
    private string _sitOnTheGrass ="SitOnTheGrass";

    public override void TurnOffAnimations()
    {
        base.TurnOffAnimations();
        Animator.SetBool(_sitOnTheGrass, false);
    }
}
