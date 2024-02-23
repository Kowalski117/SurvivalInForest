using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBuff : MonoBehaviour
{
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private Button _buffButton;
    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private YandexAds _andexAds;

    private float _minPercent = 0.3f;
    private float _maxPercent = 0.7f;
    private bool _isActive = true;
    private float _delay = 300f;
    private Coroutine _coroutine;

    public event Action OnUseBuff;

    private void Awake()
    {
        _animationUI.CloseAnimation();
    }

    private void OnEnable()
    {
        _buffButton.onClick.AddListener(UseBuffButtonClick);
    }

    private void OnDisable()
    {
        _buffButton.onClick.RemoveListener(UseBuffButtonClick);
    }

    private void Update()
    {
        if (_isActive)
        {
            if (_survivalHandler.Hunger.ValuePercent < _minPercent || _survivalHandler.Thirst.ValuePercent < _minPercent || _survivalHandler.Sleep.ValuePercent < _minPercent)
            {
                if (!_animationUI.IsOpen)
                {
                    _animationUI.SetActivePanel(true);
                    _animationUI.OpenAnimation();
                }
            }
            else if (_survivalHandler.Hunger.ValuePercent > _maxPercent && _survivalHandler.Thirst.ValuePercent > _maxPercent && _survivalHandler.Sleep.ValuePercent > _maxPercent)
            {
                if (_animationUI.IsOpen)
                    _animationUI.CloseAnimation();
            }
        }
    }

    private void UseBuffButtonClick()
    {
        _andexAds.ShowRewardAd(() => UseBuff());
    }

    private void UseBuff()
    {
        _coroutine = null;

        _survivalHandler.Hunger.ReplenishValue(_survivalHandler.Hunger.MaxValueInHours / 2);
        _survivalHandler.Thirst.ReplenishValue(_survivalHandler.Thirst.MaxValueInHours / 2);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MaxValueInHours / 2);
        _survivalHandler.PlayerHealth.ReplenishHealth(_survivalHandler.PlayerHealth.MaxHealth / 2);
        _coroutine = StartCoroutine(WaitForSeconds());
        OnUseBuff?.Invoke();
    }

    private IEnumerator WaitForSeconds()
    {
        _animationUI.CloseAnimation();

        _isActive = false;

        yield return new WaitForSeconds(_delay);

        _isActive = true;
    }
}
