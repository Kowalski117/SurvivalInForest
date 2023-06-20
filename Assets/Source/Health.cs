public class Health : SurvivalAttribute, IDamagable
{
    public void TakeDamage(float damage)
    {
        ReplenishValue(damage); 
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
