using UnityEngine;
using UnityEngine.Events;

public class UIScreenPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event UnityAction OnTogglePauseScreen;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.UIScreen.TogglePauseScreen.performed += ctx => TogglePauseScreen();
    }

    private void OnDisable()
    {
        _playerInput.BuildSystem.PutBuilding.performed -= ctx => TogglePauseScreen();
        _playerInput.Disable();
    }

    public void TogglePauseScreen()
    {
        OnTogglePauseScreen?.Invoke();
    }
}
