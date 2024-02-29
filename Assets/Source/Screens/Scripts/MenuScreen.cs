using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenUI
{
    [SerializeField] private SettingScreen _settingScreen;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _leaderboardButton;

    protected override void Awake()
    {
        base.Awake();
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        _leaderboardButton.gameObject.SetActive(true);
#endif
#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        _leaderboardButton.gameObject.SetActive(false);
#endif
    }

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
        if (!_settingScreen.IsOpenPanel)
            CloseAll();
        _settingScreen.Toggle();
    }

    protected void LeaderboardButtonClick()
    {
        if (!_leaderboardScreen.IsOpenPanel)
            CloseAll();
        _leaderboardScreen.Toggle();
    }

    protected virtual void CloseAll()
    {
        _settingScreen.Close();
        _leaderboardScreen.Close();
    }
}
