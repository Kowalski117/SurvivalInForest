using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyRewardsScreen : ScreenUI
{
    [SerializeField] private Button _exitButton;

    public event UnityAction OnExitButton;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleDailyRewardsScreen += ToggleScreen;
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleDailyRewardsScreen -= ToggleScreen;
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
        ToggleScreen();
    }
}
