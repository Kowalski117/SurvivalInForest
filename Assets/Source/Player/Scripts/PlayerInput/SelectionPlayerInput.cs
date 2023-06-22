using UnityEngine.Events;
using UnityEngine;

public class SelectionPlayerInput : MonoBehaviour
{
    private PlayerInput playerInput;

    public event UnityAction PickUp;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.PickUp.started += ctx => OnPerformed();
    }

    private void OnDisable()
    {
        playerInput.Player.PickUp.started -= ctx => OnPerformed();
        playerInput.Disable();
    }

    private void OnPerformed()
    {
        PickUp?.Invoke();
        return;
    }
}