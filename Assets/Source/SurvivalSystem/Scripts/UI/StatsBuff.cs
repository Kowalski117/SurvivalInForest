using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBuff : MonoBehaviour
{
    private const float Delay = 300f;

    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private Button _buffButton;
    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private YandexAds _andexAds;

    private float _minPercent = 0.3f;
    private float _maxPercent = 0.7f;
    private int _divisor = 2;
    private bool _isActive = true;

    private Coroutine _coroutine;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(Delay);

    public event Action OnUsed;

    private void Awake()
    {
        _animationUI.Close();
    }

    private void OnEnable()
    {
        _buffButton.onClick.AddListener(UseButtonClick);
    }

    private void OnDisable()
    {
        _buffButton.onClick.RemoveListener(UseButtonClick);
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
                    _animationUI.Open();
                }
            }
            else if (_survivalHandler.Hunger.ValuePercent > _maxPercent && _survivalHandler.Thirst.ValuePercent > _maxPercent && _survivalHandler.Sleep.ValuePercent > _maxPercent)
            {
                if (_animationUI.IsOpen)
                    _animationUI.Close();
            }
        }
    }

    private void UseButtonClick()
    {
        _andexAds.ShowRewardAd(() => Use());
    }

    private void Use()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _survivalHandler.Hunger.ReplenishValue(_survivalHandler.Hunger.MaxValueInHours / _divisor);
        _survivalHandler.Thirst.ReplenishValue(_survivalHandler.Thirst.MaxValueInHours / _divisor);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MaxValueInHours / _divisor);
        _survivalHandler.PlayerHealth.Replenish(_survivalHandler.PlayerHealth.MaxHealth / _divisor);

        _coroutine = StartCoroutine(WaitForSeconds());
        OnUsed?.Invoke();
    }

    private IEnumerator WaitForSeconds()
    {
        _animationUI.Close();

        _isActive = false;

        yield return _waitForSeconds;

        _isActive = true;
    }
}
