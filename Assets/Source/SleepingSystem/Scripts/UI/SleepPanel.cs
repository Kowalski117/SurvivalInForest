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
    [SerializeField] private DelayHandler _loadingWindow;
    [SerializeField] private SavingGame _saveGame;

    private DateTime _sleepTime;
    private float _maximumDivisor = 3;
    private float _delay = 3;

    public static event Action<bool> OnTimeStopped;
    public static event Action<float> OnTimeSubtracted;
    public event Action OnPlayerSleeped;

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

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Toggle();
    }

    private void SleepButtonClick()
    {
        Close();
        OnTimeStopped?.Invoke(false);
        _loadingWindow.ShowLoadingWindow(_delay, _survivalHandler.Sleep.MissingValue, string.Empty, ActionType.Sleep, () => FinishComplete());
    }

    private void FinishComplete()
    {
        OnTimeSubtracted?.Invoke(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.TimeHandler.AddTimeInSeconds(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Hunger.LowerValue(_survivalHandler.Sleep.MissingValue / _maximumDivisor);
        _survivalHandler.Thirst.LowerValue(_survivalHandler.Sleep.MissingValue / _maximumDivisor);

        OnPlayerSleeped?.Invoke();
        OnTimeStopped?.Invoke(true);
        _survivalHandler.TimeHandler.ToggleEnable(true);
        _saveGame.Save();
    }
}
