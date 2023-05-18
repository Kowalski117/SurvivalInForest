using UnityEngine;

public class HitPlayerInput : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private PlayerInput _playerInput;


    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Interact.performed += ctx => Hit();
    }

    private void OnDisable()
    {
        _playerInput.Player.Interact.performed -= ctx => Hit();
        _playerInput.Disable();
    }

    private void Hit()
    {
        _animator.SetTrigger("1");
    }
}
