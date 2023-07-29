using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ValueBar : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
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
    }

    private void OnDisable()
    {
        _playerInteraction.OnEnableBarValue -= EnableBar;
        _playerInteraction.OnTurnOffBarValue -= TurnOffBar;
        _playerInteraction.OnValueChanged -= UpdateBar;
    }

    public void UpdateBar(float value)
    {
        _valueText.text = value.ToString();

        _bar.fillAmount = value / _maxValue;
    }

    public void EnableBar(float maxValue, float currentCalue)
    {
        _maxValue = maxValue;
        _currenValue = currentCalue;
        _ground.gameObject.SetActive(true);

        UpdateBar(_currenValue);
    }

    public void TurnOffBar()
    {
        _maxValue = 0;
        _ground.gameObject.SetActive(false);

        UpdateBar(_maxValue);
    }
}
