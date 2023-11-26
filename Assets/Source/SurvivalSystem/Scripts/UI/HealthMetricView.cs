using UnityEngine;

public class HealthMetricView : ValueMetricView
{
    [SerializeField] private PlayerHealth _health;

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateFillAmount;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged += UpdateFillAmount;
    }
}
