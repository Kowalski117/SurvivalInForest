using UnityEngine;
using UnityEngine.Events;

public class InteractionPlayerInput : MonoBehaviour
{
    private PlayerInput playerInput;

    public event UnityAction OnHit;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Hit.performed += ctx => OnPerformed();
    }

    private void OnDisable()
    {
        playerInput.Player.Hit.performed -= ctx => OnPerformed();
        playerInput.Disable();
    }

    private void OnPerformed()
    {
        OnHit?.Invoke();
    }
}
