using System;
using UnityEngine;

public class Protection : MonoBehaviour
{
    [SerializeField] private float _maxProtectionValue;
    [SerializeField] private float _protectionValue;

    private float _maxPercent = 1;
    private float _protectionPercent = 0.1f;

    public event Action<float> OnValueChanged;

    public float ProtectionValue => _protectionValue;

    private void Start()
    {
        OnValueChanged?.Invoke(_protectionValue / _maxProtectionValue);
    }

    public void UpdateValue(float value)
    {
        _protectionValue += value;
        OnValueChanged?.Invoke(_protectionValue / _maxProtectionValue);
    }

    public float GetPercent()
    {
        float percent = _protectionValue / _maxProtectionValue;

        if (percent == _maxPercent)
            percent -= _protectionPercent;

        return _maxPercent - percent;
    }
}
