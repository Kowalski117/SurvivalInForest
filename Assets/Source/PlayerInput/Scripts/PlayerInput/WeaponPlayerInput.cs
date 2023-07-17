using UnityEngine;
using UnityEngine.Events;

public class WeaponPlayerInput : MonoBehaviour
{
    private PlayerInput playerInput;

    public event UnityAction OnShoot;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.WeaponSystem.Shoot.performed += ctx => Shoot();
    }

    private void OnDisable()
    {
        playerInput.WeaponSystem.Shoot.performed -= ctx => Shoot();
        playerInput.Disable();
    }

    public void Shoot()
    {
        OnShoot?.Invoke();
    }
}
