using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreen : ScreenUI
{
    [SerializeField] private PlayerInputHandler _playerInputHandler;

    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;

    private bool _isOpenScreen = false;

    public event UnityAction OnContinueButton;
    public event UnityAction OnSaveButton;
    public event UnityAction OnExitButton;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _saveButton.onClick.AddListener(SaveButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);

        _playerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleScreen;
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _saveButton.onClick.RemoveListener(SaveButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);

        _playerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleScreen;
    }

    public void ContinueButtonClick()
    {
        OnContinueButton?.Invoke();
        ToggleScreen();
    }

    public void SaveButtonClick()
    {
        OnSaveButton?.Invoke();
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
    }

    public void ToggleScreen()
    {
        _isOpenScreen = !_isOpenScreen;

        if (_isOpenScreen)
        {
            OpenScreen();
            _playerInputHandler.SetCursorVisible(true);
            _playerInputHandler.ToggleAllInput(false);
        }
        else
        {
            CloseScreen();
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleAllInput(true);
        }
    }
}
