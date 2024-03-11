using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private TargetInteractionHandler _playerInteraction;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _bar;
    [SerializeField] private Image _ground;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private AnimationUI _animationUI;

    private float _maxValue;
    private float _currenValue;

    private void OnEnable()
    {
        _playerInteraction.OnEnableBarValue += Enable;
        _playerInteraction.OnTurnOffBarValue += TurnOff;
        _playerInteraction.OnValueChanged += UpdateValue;

        _playerHealth.OnDied += TurnOff;
        _playerHealth.OnRevived += TurnOff;
    }

    private void OnDisable()
    {
        _playerInteraction.OnEnableBarValue -= Enable;
        _playerInteraction.OnTurnOffBarValue -= TurnOff;
        _playerInteraction.OnValueChanged -= UpdateValue;

        _playerHealth.OnDied -= TurnOff;
        _playerHealth.OnRevived -= TurnOff;
    }

    public void UpdateValue(float value)
    {
        if(value >= 0)
            _valueText.text = value.ToString();

        _bar.fillAmount = value / _maxValue;

        if(_bar.fillAmount <= 0)
        {
            TurnOff();
        }
    }

    public void Enable(float maxValue, float currentCalue)
    {
        if (!_playerHealth.IsDied)
        {
            _maxValue = maxValue;
            _currenValue = currentCalue;
            _animationUI.Open();

            UpdateValue(_currenValue);
        }
    }

    public void TurnOff()
    {
        _animationUI.Close();
    }
}
