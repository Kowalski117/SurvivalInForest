public class Stone : Resource
{
    public override void Die()
    {
        Сollider.enabled = false;
        base.Die();
    }
}
