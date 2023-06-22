using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class StaminaAttribute : SurvivalAttribute
{
    [SerializeField] private float _rechargeRate = 2f;
    [SerializeField] private float _rechargeDelay = 1f;

    [SerializeField] private StarterAssetsInputs _playerInput;

    private float _currentDelayCounter;

    public void DecreaseStaminaValue()
    {
        if (_playerInput.sprint && _playerInput.move != Vector2.zero)
        {
            if(CurrentValue > 0)
                CurrentValue -= ValueDeplerionRate * Time.deltaTime;
            else
                CurrentValue = 0;

            _currentDelayCounter = 0;
        }

        if(!_playerInput.sprint && CurrentValue < MaxValue)
        {
            if (_currentDelayCounter < _rechargeDelay)
            {
                _currentDelayCounter += Time.deltaTime;
            }
            else
            {
                if (CurrentValue < MaxValue)
                    CurrentValue += _rechargeRate * Time.deltaTime;
                else
                    CurrentValue = MaxValue;
            }
        }
    }
}
