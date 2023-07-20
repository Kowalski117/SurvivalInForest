using UnityEngine;
using UnityEngine.Events;

public class InteractionPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _isButtonPressed;


    public event UnityAction OnInteractedConstruction;
    public event UnityAction OnHit;
    public event UnityAction OnPickUp;
    public event UnityAction<bool> OnAttack;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.InteractionConstruction.performed += ctx => InteractedConstruction();
        _playerInput.Player.Hit.performed += ctx => Hit();
        _playerInput.Player.PickUp.performed += ctx => PickUp();
        _playerInput.WeaponSystem.Attack.performed += ctx => Attack();
    }

    private void OnDisable()
    {
        _playerInput.Player.InteractionConstruction.performed -= ctx => InteractedConstruction();
        _playerInput.Player.Hit.performed -= ctx => Hit();
        _playerInput.Player.PickUp.performed -= ctx => PickUp();
        _playerInput.WeaponSystem.Attack.performed -= ctx => Attack();
        _playerInput.Disable();
    }

    private void InteractedConstruction()
    {
        OnInteractedConstruction?.Invoke();
    }

    private void Hit()
    {
        OnHit?.Invoke();
    }

    private void PickUp()
    {
        OnPickUp?.Invoke();
    }

    public void Attack()
    {
        _isButtonPressed = !_isButtonPressed;
        OnAttack?.Invoke(_isButtonPressed);
    }

    public void TurnOff()
    {
        _isButtonPressed = false;
        OnAttack?.Invoke(_isButtonPressed);
    }
}
