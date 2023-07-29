using StarterAssets;
using UnityEngine;

public class StaminaAttribute : MonoBehaviour
{
    [SerializeField] private float _maxValue = 100;
    [SerializeField] private float _valueDeplerionRate = 1f;
    [SerializeField] private float _rechargeRate = 2f;
    [SerializeField] private float _rechargeDelay = 1f;
    [SerializeField] private StarterAssetsInputs _playerInput;

    protected float _currentValue;
    private float _currentDelayCounter;

    public float ValuePercent => _currentValue / _maxValue;
    public bool IsNotEmpty => _currentValue > 0;

    public float MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
        }
    }

    private void Start()
    {
        _currentValue = _maxValue;
    }

    public void DecreaseStaminaValue()
    {
        if (_playerInput.sprint && _playerInput.move != Vector2.zero)
        {
            if(_currentValue > 0)
                _currentValue -= _valueDeplerionRate * Time.deltaTime;
            else
                _currentValue = 0;

            _currentDelayCounter = 0;
        }

        if(!_playerInput.sprint && _currentValue < _maxValue)
        {
            if (_currentDelayCounter < _rechargeDelay)
            {
                _currentDelayCounter += Time.deltaTime;
            }
            else
            {
                if (_currentValue < _maxValue)
                    _currentValue += _rechargeRate * Time.deltaTime;
                else
                    _currentValue = _maxValue;
            }
        }
    }

    public void AddMaxValueStamina(float value)
    {
        MaxValue += value;
    }
}
