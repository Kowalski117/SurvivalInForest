using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyRewardsScreen : ScreenUI
{
    [SerializeField] private Button _exitButton;

    public event UnityAction OnExitButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _exitButton.onClick.AddListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleDailyRewardsScreen += ToggleScreen;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _exitButton.onClick.RemoveListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleDailyRewardsScreen -= ToggleScreen;
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
        ToggleScreen();
    }
}
