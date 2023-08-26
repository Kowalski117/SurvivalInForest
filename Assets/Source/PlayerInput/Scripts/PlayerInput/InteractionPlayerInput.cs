using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class InteractionPlayerInput : MonoBehaviour
{
    private PlayerInput _playerInput;
    private bool _isButtonPressed;
    private Coroutine _coroutine;

    public event UnityAction OnInteractedConstruction;
    public event UnityAction OnHit;
    public event UnityAction OnPickUp;
    public event UnityAction<bool> OnAttack;
    public event UnityAction OnThrowFishingRod;


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
        _playerInput.WeaponSystem.ThrowFishingRod.performed += ctx => ThrowFishingRod();
    }

    private void OnDisable()
    {
        _playerInput.Player.InteractionConstruction.performed -= ctx => InteractedConstruction();
        _playerInput.Player.Hit.performed -= ctx => Hit();
        _playerInput.Player.PickUp.performed -= ctx => PickUp();
        _playerInput.WeaponSystem.Attack.performed -= ctx => Attack();
        _playerInput.WeaponSystem.ThrowFishingRod.performed -= ctx => ThrowFishingRod();
        _playerInput.Disable();
    }

    public void InteractedConstruction()
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

    public void ThrowFishingRod()
    {
        OnThrowFishingRod?.Invoke();
    }

    public void PressedButton()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Pressed());
    }

    private IEnumerator Pressed()
    {
        yield return new WaitForSeconds(1f);
        TurnOff();
    }
}
