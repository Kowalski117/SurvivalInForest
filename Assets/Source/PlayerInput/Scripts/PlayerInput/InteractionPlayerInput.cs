using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractionPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _isEnable = true;

    public event UnityAction OnInteractedConstruction;
    public event UnityAction OnHit;
    public event UnityAction OnPickUp;
    public event UnityAction<bool> OnAttack;
    public event UnityAction OnUse;
    public event UnityAction OnAim;


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
        _playerInput.WeaponSystem.Use.performed += ctx => Use();
        _playerInput.WeaponSystem.Aim.performed += ctx => Aim();
    }

    private void OnDisable()
    {
        _playerInput.Player.InteractionConstruction.performed -= ctx => InteractedConstruction();
        _playerInput.Player.Hit.performed -= ctx => Hit();
        _playerInput.Player.PickUp.performed -= ctx => PickUp();
        _playerInput.WeaponSystem.Attack.performed -= ctx => Attack();
        _playerInput.WeaponSystem.Use.performed -= ctx => Use();
        _playerInput.WeaponSystem.Aim.performed -= ctx => Aim();
        _playerInput.Disable();
    }

    public void InteractedConstruction()
    {
        if(_isEnable)
            OnInteractedConstruction?.Invoke();
    }

    private void Hit()
    {
        if (_isEnable)
            OnHit?.Invoke();
    }

    private void PickUp()
    {
        if (_isEnable)
            OnPickUp?.Invoke();
    }

    public void Attack()
    {
        if (_isEnable)
        {
            OnAttack?.Invoke(_playerInput.WeaponSystem.Attack.IsPressed());
        }
        else
        {
            OnAttack?.Invoke(false);
        }
    }

    public void Use()
    {
        if (_isEnable)
            OnUse?.Invoke();
    }

    public void Aim()
    {
        if (_isEnable)
            OnAim?.Invoke();
    }

    public void SetEnable(bool enable)
    {
        _isEnable = enable;
    }
}
