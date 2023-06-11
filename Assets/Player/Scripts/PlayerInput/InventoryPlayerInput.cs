using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class InventoryPlayerInput : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ManualWorkbench _manualWorkbench;

    private PlayerInput _playerInput;

    public event UnityAction<InventorySystem, int> SwitchInventory;
    public event UnityAction<Crafting—ategory, bool> OnCraftPlayerWindow;

    public event UnityAction InteractKeyPressed;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        //_playerInput.Player.Interact.performed += ctx => InteractInventory();
        _playerInput.Player.Inventory.performed += ctx => InteractCrafting();
        _playerInput.Player.Interact.performed += ctx => InteractKeyPressed?.Invoke();
    }

    private void OnDisable()
    {
        //_playerInput.Player.Interact.performed -= ctx => InteractInventory();
        _playerInput.Player.Inventory.performed -= ctx => InteractCrafting();
        _playerInput.Player.Interact.performed -= ctx => InteractKeyPressed?.Invoke();
        _playerInput.Disable();
    }

    private void InteractInventory()
    {
        SwitchInventory?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
    }

    private void InteractCrafting() 
    {
        SwitchInventory?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
        OnCraftPlayerWindow?.Invoke(_manualWorkbench.Crafting—ategory, true);
    }
}
