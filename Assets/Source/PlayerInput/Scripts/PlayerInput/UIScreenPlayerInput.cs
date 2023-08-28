using UnityEngine;
using UnityEngine.Events;

public class UIScreenPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event UnityAction OnTogglePauseScreen;
    public event UnityAction OnToggleShopScreen;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.UIScreen.TogglePauseScreen.performed += ctx => TogglePauseScreen();
        _playerInput.UIScreen.ToggleShopScreen.performed += ctx => ToggleShopScreen();
    }

    private void OnDisable()
    {
        _playerInput.UIScreen.TogglePauseScreen.performed -= ctx => TogglePauseScreen();
        _playerInput.UIScreen.ToggleShopScreen.performed -= ctx => ToggleShopScreen();
        _playerInput.Disable();
    }

    public void TogglePauseScreen()
    {
        OnTogglePauseScreen?.Invoke();
    }

    public void ToggleShopScreen()
    {
        OnToggleShopScreen?.Invoke();
    }
}
