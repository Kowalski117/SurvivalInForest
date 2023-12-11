using UnityEngine;
using UnityEngine.Events;

public class SurvivalAttribute : MonoBehaviour
{
    [SerializeField] protected float MaxValue = 2;
    [SerializeField] protected float ValueDeplerionRate = 1f;

    protected float CurrentValue;
    private float _hourInSeconds = 3600;

    public event UnityAction<float> OnValueChanged;

    private float _maxValue => MaxValue * _hourInSeconds;
    public float CurrentAttribute => CurrentValue;
    public float CurrentValueAttribute => CurrentValue * _hourInSeconds;
    public float MaxValueInSeconds => _maxValue;
    public float MaxValueInHours => MaxValue;
    public float MissingValue => (_maxValue - CurrentValue) / _hourInSeconds;
    public float ValuePercent => CurrentValue / _maxValue;

    private void Awake()
    {
        CurrentValue = _maxValue;
        OnValueChanged?.Invoke(ValuePercent);
    }

    public void ReplenishValue(float value)
    {
        CurrentValue += value * _hourInSeconds;

        if (CurrentValue > _maxValue)
            CurrentValue = _maxValue;

        OnValueChanged?.Invoke(ValuePercent);
    }

    public void LowerValue(float value)
    {
        CurrentValue -= value;

        if (CurrentValue <= 0)
            CurrentValue = 0;

        OnValueChanged?.Invoke(ValuePercent);
    }

    public void LowerValueInFours(float value)
    {
        LowerValue(value * _hourInSeconds);
    }

    public void SetValue(float value)
    {
        CurrentValue = value;
        OnValueChanged?.Invoke(ValuePercent);
    }
}
