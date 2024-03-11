using Agava.YandexGames;
using UnityEngine;

public class LeaderboardScreen : ScreenUI
{
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private LoginAccountPanel _authorizationPanel;

    private void Authorization()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (!YandexGamesSdk.IsInitialized)
            return;

        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            _leaderboard.Fill();
            Open();
            //SaveGame.GetCloudSaveData();
        }
        else
            _authorizationPanel.Close();
#endif
    }

    public override void Toggle()
    {
        IsOpenScreen = !IsOpenScreen;

        if (IsOpenScreen)
            Authorization();
        else
        {
            Close();

            if(_authorizationPanel.IsOpenPanel)
                _authorizationPanel.Close();
        }
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Close();
    }
}
