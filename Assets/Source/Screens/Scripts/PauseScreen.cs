using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreen : ScreenUI
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitButton;

    public event UnityAction OnContinueButton;
    public event UnityAction OnSaveButton;
    public event UnityAction OnExitButton;

    public override void OnEnable()
    {
        base.OnEnable();
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _saveButton.onClick.AddListener(SaveButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleScreen;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _saveButton.onClick.RemoveListener(SaveButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleScreen;
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
}
