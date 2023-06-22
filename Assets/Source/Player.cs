public class Player : SurvivalAttribute, IDamagable
{
    public void TakeDamage(float damage,float overTimeDamage)
    {
        LowerValue(damage);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
