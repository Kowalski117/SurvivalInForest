using Agava.YandexGames;
using UnityEngine;
using UnityEngine.UI;

public class LoginAccountPanel : ScreenUI
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
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.Authorize();
#endif
        Close();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Close();
    }
}
