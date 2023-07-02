using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : SurvivalAttribute, IDamagable
{
    public event UnityAction<float> OnHealthChanged;

    public float HealthPercent => CurrentValue / MaxValue;

    private void Start()
    {
        CurrentValue = MaxValue;
    }

    public void LowerHealth(float value)
    {
        CurrentValue -= value;
        Debug.Log(value);

        if (CurrentValue <= 0)
            CurrentValue = 0;

        OnHealthChanged?.Invoke(HealthPercent);
    }

    public void TakeDamage(float damage,float overTimeDamage)
    {
        LowerHealth(damage);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
