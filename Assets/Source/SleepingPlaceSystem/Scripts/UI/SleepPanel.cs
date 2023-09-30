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
    [SerializeField] private DelayWindow _loadingWindow;
    [SerializeField] private SaveGame _saveGame;

    private DateTime _sleepTime;
    private bool _isSleepWindowOpen = false;

    public static UnityAction<bool> OnStoppedTime;
    public static UnityAction<float> OnSubtractTime;

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
    private void Update()
    {
        if(_sleepWindow.gameObject.activeInHierarchy)
            _timer.text = _sleepTime.AddHours(_survivalHandler.Sleep.MissingValue).ToString("HH:mm");
    }

    private void SleepButtonClick()
    {
        ExitButtonClick();
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
        _survivalHandler.Hunger.LowerValue(_survivalHandler.Sleep.MissingValue);
        _survivalHandler.Thirst.LowerValue(_survivalHandler.Sleep.MissingValue);

        OnStoppedTime?.Invoke(true);
        _survivalHandler.TimeHandler.ToggleEnable(true);
        _saveGame.Save();
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
            _sleepWindow.gameObject.SetActive(true);
        }
        else
        {
            _playerInputHandler.SetCursorVisible(_isSleepWindowOpen);
            _playerInputHandler.ToggleInventoryInput(!_isSleepWindowOpen);
            _sleepWindow.gameObject.SetActive(false);
        }
    }
}
