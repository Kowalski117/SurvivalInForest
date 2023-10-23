public class Stone : Resource
{
    public override void Die()
    {
        gameObject.SetActive(false);
        SpawnLoot();
        DiedEvent();
    }
}
