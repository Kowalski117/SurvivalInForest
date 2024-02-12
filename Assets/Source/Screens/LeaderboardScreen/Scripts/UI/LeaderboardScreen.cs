using Agava.YandexGames;
using UnityEngine;

public class LeaderboardScreen : ScreenUI
{
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private AuthorizationPanel _authorizationPanel;

    private void Authorization()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (!YandexGamesSdk.IsInitialized)
            return;

        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            _leaderboard.Fill();
            OpenScreen();
            //SaveGame.GetCloudSaveData();
        }
        else
        {
            _authorizationPanel.OpenScreen();
        }
#endif
    }

    public override void ToggleScreen()
    {
        IsOpenScreen = !IsOpenScreen;

        if (IsOpenScreen)
        {
            Authorization();
        }
        else
        {
            CloseScreen();

            if(_authorizationPanel.IsOpenPanel)
                _authorizationPanel.CloseScreen();
        }
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        CloseScreen();
    }
}
