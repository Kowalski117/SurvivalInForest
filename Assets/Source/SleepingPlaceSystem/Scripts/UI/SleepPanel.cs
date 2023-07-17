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
    [SerializeField] private LoadingWindow _loadingWindow;

    private DateTime _sleepTime;
    private bool _isSleepWindowOpen = false;

    public static UnityAction<float> OnSleepButton;

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
        _timer.text = _sleepTime.AddHours(_survivalHandler.Sleep.MissingValue).ToString("HH:mm");
    }

    private void SleepButtonClick()
    {
        ExitButtonClick();
        _loadingWindow.ShowLoadingWindow(3, _survivalHandler.Sleep.MissingValue, string.Empty, ActionType.Sleep);
        _loadingWindow.OnLoadingComplete += OnLoadingComplete;
    }

    private void OnLoadingComplete()
    {
        _survivalHandler.TimeHandler.AddTime(_survivalHandler.Sleep.MissingValue);
        OnSleepButton?.Invoke(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Sleep.ReplenishValue(_survivalHandler.Sleep.MissingValue);

        _loadingWindow.OnLoadingComplete -= OnLoadingComplete;
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
