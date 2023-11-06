using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image _bar;
    [SerializeField] private Image _ground;
    [SerializeField] private TMP_Text _valueText;

    private float _maxValue;
    private float _currenValue;

    private void OnEnable()
    {
        _playerInteraction.OnEnableBarValue += EnableBar;
        _playerInteraction.OnTurnOffBarValue += TurnOffBar;
        _playerInteraction.OnValueChanged += UpdateBar;

        _playerHealth.OnDied += TurnOffBar;
        _playerHealth.OnRevived += TurnOffBar;
    }

    private void OnDisable()
    {
        _playerInteraction.OnEnableBarValue -= EnableBar;
        _playerInteraction.OnTurnOffBarValue -= TurnOffBar;
        _playerInteraction.OnValueChanged -= UpdateBar;

        _playerHealth.OnDied -= TurnOffBar;
        _playerHealth.OnRevived -= TurnOffBar;
    }

    public void UpdateBar(float value)
    {
        _valueText.text = value.ToString();

        _bar.fillAmount = value / _maxValue;

        if(_bar.fillAmount <= 0)
        {
            _bar.fillAmount = 0;
            TurnOffBar();
        }
    }

    public void EnableBar(float maxValue, float currentCalue)
    {
        if (!_playerHealth.IsDied)
        {
            _maxValue = maxValue;
            _currenValue = currentCalue;
            _ground.gameObject.SetActive(true);

            UpdateBar(_currenValue);
        }
    }

    public void TurnOffBar()
    {
        _maxValue = 0;
        _ground.gameObject.SetActive(false);

        UpdateBar(_maxValue);
    }
}
