using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationPanel : ScreenUI
{
    [SerializeField] private Button _authorizationButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _authorizationButton.onClick.AddListener(AuthorizationButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _authorizationButton.onClick.RemoveListener(AuthorizationButtonClick);
    }

    private void AuthorizationButtonClick()
    {
        PlayerAccount.Authorize();

        CloseScreen();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        CloseScreen();
    }
}
