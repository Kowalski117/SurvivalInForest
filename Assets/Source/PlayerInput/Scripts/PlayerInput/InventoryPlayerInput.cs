using UnityEngine;
using UnityEngine.Events;

public class InventoryPlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ManualWorkbench _manualWorkbench;

    private PlayerInput _playerInput;

    public event UnityAction<InventorySystem, int> SwitchInventory;
    public event UnityAction<Crafting—ategory> OnCraftPlayerWindow;
    public event UnityAction OnToggleInventory;

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

    public void ToggleInventory() 
    {
        OnToggleInventory?.Invoke();
        SwitchInventory?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
        OnCraftPlayerWindow?.Invoke(_manualWorkbench.Crafting—ategory);
    }
}
