using UnityEngine;
using UnityEngine.Events;

public class ProtectionValue : MonoBehaviour
{
    [SerializeField] private float _maxProtectionValue;
    [SerializeField] private float _protectionValue;

    private float _maxPercent = 1;
    private float _protectionPercent = 0.1f;

    public event UnityAction<float> OnValueChanged;

    public float Protection => _protectionValue;

    private void Start()
    {
        OnValueChanged?.Invoke(_protectionValue / _maxProtectionValue);
    }

    public void UpdateProtectionValue(float value)
    {
        _protectionValue += value;
        OnValueChanged?.Invoke(_protectionValue / _maxProtectionValue);
    }

    public float GetProtectionPercent()
    {
        float percent = _protectionValue / _maxProtectionValue;

        if (percent == _maxPercent)
            percent -= _protectionPercent;

        return _maxPercent - percent;
    }
}
