using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenUI
{
    [SerializeField] private SettingScreen _settingScreen;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _leaderboardButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _settingButton.onClick.AddListener(SettingButtonClick);
        _leaderboardButton.onClick.AddListener(LeaderboardButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _settingButton.onClick.RemoveListener(SettingButtonClick);
        _leaderboardButton.onClick.RemoveListener(LeaderboardButtonClick);
    }

    protected void SettingButtonClick()
    {
        if (!_settingScreen.IsOpenScreen)
            CloseAllScreens();
        _settingScreen.ToggleScreen();
    }

    protected void LeaderboardButtonClick()
    {
        if (!_leaderboardScreen.IsOpenScreen)
            CloseAllScreens();
        _leaderboardScreen.ToggleScreen();
    }

    protected virtual void CloseAllScreens()
    {
        _settingScreen.CloseScreen();
        _leaderboardScreen.CloseScreen();
    }
}
