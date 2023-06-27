using UnityEngine;
using UnityEngine.Events;

public class InteractionConstructionPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event UnityAction OnInteractedConstruction;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.InteractionConstruction.performed += ctx => Interaction();
    }

    private void OnDisable()
    {
        _playerInput.Player.InteractionConstruction.performed -= ctx => Interaction();
        _playerInput.Disable();
    }

    private void Interaction()
    {
        OnInteractedConstruction?.Invoke();
    }
}
