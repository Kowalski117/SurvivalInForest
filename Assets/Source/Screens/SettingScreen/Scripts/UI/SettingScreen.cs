using Agava.YandexGames;
using UnityEngine;

public class SettingScreen : ScreenUI
{
    [SerializeField] private bool _isOpenMainMenu;
    [SerializeField] private Transform[] _panelsOpenMainMenu;
    [SerializeField] private Transform[] _panelsGame;
    [SerializeField] private Transform _authorizePanel;

    protected override void Awake()
    {
        if (_isOpenMainMenu)
        {
            for (int i = 0; i < _panelsOpenMainMenu.Length; i++)
            {
                _panelsOpenMainMenu[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < _panelsGame.Length; i++)
            {
                _panelsGame[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _panelsGame.Length; i++)
            {
                _panelsGame[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < _panelsOpenMainMenu.Length; i++)
            {
                _panelsOpenMainMenu[i].gameObject.SetActive(false);
            }
        }
           
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void ToggleScreen()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
            _authorizePanel.gameObject.SetActive(false);
        else
            _authorizePanel.gameObject.SetActive(true);
#endif

        base.ToggleScreen();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
        PlayerPrefs.Save();
    }
}
