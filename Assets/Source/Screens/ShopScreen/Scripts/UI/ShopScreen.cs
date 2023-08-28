using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopScreen : ScreenUI
{
    [SerializeField] private Button _exitButton;

    public event UnityAction OnExitButton;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleShopScreen += ToggleScreen;
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(ExitButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnToggleShopScreen -= ToggleScreen;
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
        ToggleScreen();
    }
}
