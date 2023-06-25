using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SleepPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Button _sleepButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _sleepWindow;
    [SerializeField] private SurvivalHandler _survivalHandler;

    private DateTime _sleepTime;

    public Transform SleepWindow => _sleepWindow;

    public event UnityAction OnSleepButton;

    private void OnEnable()
    {
        _sleepButton.onClick.AddListener(SleepButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
    }

    private void OnDisable()
    {
        _sleepButton.onClick.RemoveListener(SleepButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);
    }

    public void OpenWindow()
    {
        _sleepWindow.gameObject.SetActive(true);
        _timer.text = _sleepTime.AddSeconds(_survivalHandler.Sleep.MissingValue).ToString("HH:mm");
    }

    private void SleepButtonClick()
    {
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.TimeHandler.AddTime(_survivalHandler.Sleep.MissingValue);
        OnSleepButton?.Invoke();
    }

    private void ExitButtonClick()
    {
        _sleepWindow.gameObject.SetActive(false);
    }
}
