using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopScreen : ScreenUI
{
    [SerializeField] private Button _exitButton;

    public event UnityAction OnExitButton;

    public override void OnEnable()
    {
        base.OnEnable();

        _exitButton.onClick.AddListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleShopScreen += ToggleScreen;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        _exitButton.onClick.RemoveListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleShopScreen -= ToggleScreen;
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
        ToggleScreen();
    }
}
