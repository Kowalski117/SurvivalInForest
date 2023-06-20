using UnityEngine;
using UnityEngine.Events;

public class SurvivalAttribute : MonoBehaviour
{
    [SerializeField] protected float MaxValue = 100;
    [SerializeField] protected float ValueDeplerionRate = 1f;

    protected float CurrentValue;

    public event UnityAction<float> OnValueChanged;
    public event UnityAction OnZeroValueReached;

    public float ValuePercent => CurrentValue / MaxValue;

    public bool IsNotEmpty => CurrentValue > 0;

    private void Start()
    {
        CurrentValue = MaxValue;
    }

    public void DecreaseValue()
    {
        if(CurrentValue > 0)
        {
            CurrentValue -= ValueDeplerionRate * Time.deltaTime;
        }
        else
        {
            CurrentValue = 0;
            OnZeroValueReached?.Invoke();
        }
        OnValueChanged?.Invoke(CurrentValue);
    }

    public void ReplenishValue(float value)
    {
        CurrentValue += value;

        if(CurrentValue > MaxValue)
            CurrentValue = MaxValue;

        OnValueChanged?.Invoke(CurrentValue);
    }
}
