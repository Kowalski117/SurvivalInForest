using UnityEngine;
using UnityEngine.Events;

public class SurvivalAttribute : MonoBehaviour
{
    [SerializeField] protected float MaxValue = 2;
    [SerializeField] protected float ValueDeplerionRate = 1f;

    protected float CurrentValue;
    private float _hourInSeconds = 3600;
    private float _maxValue => MaxValue * _hourInSeconds;

    public event UnityAction<float> OnValueChanged;
    public event UnityAction OnZeroValueReached;

    public float MissingValue => _maxValue - CurrentValue;
    public float ValuePercent => CurrentValue / _maxValue;

    private void Start()
    {
        CurrentValue = MaxValue * _hourInSeconds;
    }

    //public void DecreaseValue()
    //{
    //    if(CurrentValue > 0)
    //    {
    //        CurrentValue -= ValueDeplerionRate * Time.deltaTime;
    //    }
    //    else
    //    {
    //        CurrentValue = 0;
    //        OnZeroValueReached?.Invoke();
    //    }
    //    OnValueChanged?.Invoke(CurrentValue);
    //}

    public void ReplenishValue(float value)
    {
        Debug.Log(CurrentValue);
        Debug.Log(value);
        CurrentValue += value;
        Debug.Log(CurrentValue);
        if(CurrentValue > _maxValue)
            CurrentValue = _maxValue;

        OnValueChanged?.Invoke(ValuePercent);
    }

    public void LowerValue(float value)
    {
        CurrentValue -= value * Time.deltaTime;

        if (CurrentValue <= 0)
            CurrentValue = 0;

        OnValueChanged?.Invoke(ValuePercent);
    }
}
