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
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    private DateTime _sleepTime;
    private bool _isSleepWindowOpen = false;

    public Transform SleepWindow => _sleepWindow;

    public event UnityAction OnSleepButton;

    private void Start()
    {
        _sleepWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SleepingPlace.OnInteractionSleepingPlace += DisplaySleepWindow;

        _sleepButton.onClick.AddListener(SleepButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
    }

    private void OnDisable()
    {
        SleepingPlace.OnInteractionSleepingPlace -= DisplaySleepWindow;

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
        LoadingWindow.Instance.ShowLoadingWindow(_survivalHandler.Sleep.MissingValue / 3600);
        _survivalHandler.TimeHandler.AddTime(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MissingValue);

        OnSleepButton?.Invoke();
        ExitButtonClick();
    }

    private void ExitButtonClick()
    {
        _playerInputHandler.SetCursorVisible(!_isSleepWindowOpen);
        _playerInputHandler.ToggleInventoryInput(_isSleepWindowOpen);
        _sleepWindow.gameObject.SetActive(false);
    }

    private void DisplaySleepWindow()
    {
        _isSleepWindowOpen = !_isSleepWindowOpen;

        if (_isSleepWindowOpen)
        {
            _playerInputHandler.SetCursorVisible(_isSleepWindowOpen);
            _playerInputHandler.ToggleInventoryInput(!_isSleepWindowOpen);
            OpenWindow();
        }
        else
        {
            _playerInputHandler.SetCursorVisible(_isSleepWindowOpen);
            _playerInputHandler.ToggleInventoryInput(!_isSleepWindowOpen);
            _sleepWindow.gameObject.SetActive(false);
        }
    }
}
