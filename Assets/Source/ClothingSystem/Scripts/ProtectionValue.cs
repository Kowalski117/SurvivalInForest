using UnityEngine;
using UnityEngine.Events;

public class ProtectionValue : MonoBehaviour
{
    [SerializeField] private float _maxProtectionValue;
    [SerializeField] private float _protectionValue;

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
}
