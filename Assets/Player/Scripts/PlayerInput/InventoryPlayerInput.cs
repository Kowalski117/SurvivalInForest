using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class InventoryPlayerInput : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;

    private PlayerInput _playerInput;

    public event UnityAction SwitchInventory;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Inventory.performed += ctx => ToggleInventory();
    }

    private void OnDisable()
    {
        _playerInput.Player.Inventory.performed -= ctx => ToggleInventory();
        _playerInput.Disable();
    }

    private void ToggleInventory()
    {
        SwitchInventory?.Invoke();
    }
}
