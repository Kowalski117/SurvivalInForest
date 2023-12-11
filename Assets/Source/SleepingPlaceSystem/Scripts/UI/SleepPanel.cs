using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SleepPanel : ScreenUI
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Button _sleepButton;
    [SerializeField] private Transform _sleepWindow;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private DelayWindow _loadingWindow;
    [SerializeField] private SaveGame _saveGame;

    private DateTime _sleepTime;
    private float _maximumDivisor = 3;

    public static Action<bool> OnStoppedTime;
    public static Action<float> OnSubtractTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        _sleepButton.onClick.AddListener(SleepButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _sleepButton.onClick.RemoveListener(SleepButtonClick);
    }

    private void Update()
    {
        if(_sleepWindow.gameObject.activeInHierarchy)
            _timer.text = _sleepTime.AddHours(_survivalHandler.Sleep.MissingValue).ToString(GameConstants.HHmm);
    }

    private void SleepButtonClick()
    {
        CloseScreen();
        _loadingWindow.ShowLoadingWindow(3, _survivalHandler.Sleep.MissingValue, string.Empty, ActionType.Sleep);
        _survivalHandler.TimeHandler.ToggleEnable(false);
        OnStoppedTime?.Invoke(false);
        _loadingWindow.OnLoadingComplete += OnLoadingComplete;
    }

    private void OnLoadingComplete()
    {
        OnSubtractTime?.Invoke(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.TimeHandler.AddTime(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Hunger.LowerValue(_survivalHandler.Sleep.MissingValue / _maximumDivisor);
        _survivalHandler.Thirst.LowerValue(_survivalHandler.Sleep.MissingValue / _maximumDivisor);

        OnStoppedTime?.Invoke(true);
        _survivalHandler.TimeHandler.ToggleEnable(true);
        _saveGame.Save();
        _loadingWindow.OnLoadingComplete -= OnLoadingComplete;
    }

    protected override void ExitButtonClick()
    {
        ToggleScreen();
    }
}
