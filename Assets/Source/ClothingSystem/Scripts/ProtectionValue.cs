using UnityEngine;
using UnityEngine.UI;

public class ProtectionValue : MonoBehaviour
{
    [SerializeField] private Image _imageValue;
    [SerializeField] private float _maxProtectionValue;
    [SerializeField] private float _protectionValue;

    private void Awake()
    {
        _imageValue.fillAmount = _protectionValue / _maxProtectionValue;
    }

    public void UpdateProtectionValue(float value)
    {
        _protectionValue += value;
        _imageValue.fillAmount = _protectionValue / _maxProtectionValue;
    }
}
