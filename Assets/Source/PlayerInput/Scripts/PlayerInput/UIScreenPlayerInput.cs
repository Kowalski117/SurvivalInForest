using UnityEngine;
using UnityEngine.Events;

public class UIScreenPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event UnityAction OnTogglePauseScreen;
    public event UnityAction OnToggleShopScreen;
    public event UnityAction OnToggleDailyRewardsScreen;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.UIScreen.TogglePauseScreen.performed += ctx => TogglePauseScreen();
        _playerInput.UIScreen.ToggleShopScreen.performed += ctx => ToggleShopScreen();
        _playerInput.UIScreen.ToggleDailyRewardsScreen.performed += ctx => ToggleDailyRewardsScreen();
    }

    private void OnDisable()
    {
        _playerInput.UIScreen.TogglePauseScreen.performed -= ctx => TogglePauseScreen();
        _playerInput.UIScreen.ToggleShopScreen.performed -= ctx => ToggleShopScreen();
        _playerInput.UIScreen.ToggleDailyRewardsScreen.performed -= ctx => ToggleDailyRewardsScreen();
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

    public void ToggleDailyRewardsScreen()
    {
        OnToggleDailyRewardsScreen?.Invoke();
    }
}
