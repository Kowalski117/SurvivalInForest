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
    public event UnityAction<Crafting—ategory> OnCraftPlayerWindow;
    public event UnityAction OnBuildingWindow;
    public event UnityAction OnSelectInventoryItem;
    public event UnityAction OnInteractInventoryItem;

    public event UnityAction InteractKeyPressed;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Inventory.performed += ctx => InteractCrafting();
        _playerInput.Inventory.SelectInventoryItem.started += ctx => OnSelectInventoryItem?.Invoke();
        _playerInput.Inventory.InteractItem.started += ctx => OnInteractInventoryItem?.Invoke();
        _playerInput.Player.Interact.performed += ctx => InteractKeyPressed?.Invoke();
    }

    private void OnDisable()
    {
        _playerInput.Player.Inventory.performed -= ctx => InteractCrafting();
        _playerInput.Inventory.SelectInventoryItem.started -= ctx => OnSelectInventoryItem?.Invoke();
        _playerInput.Inventory.InteractItem.started -= ctx => OnInteractInventoryItem?.Invoke();
        _playerInput.Player.Interact.performed -= ctx => InteractKeyPressed?.Invoke();
        _playerInput.Disable();
    }

    private void InteractCrafting() 
    {
        SwitchInventory?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
        OnCraftPlayerWindow?.Invoke(_manualWorkbench.Crafting—ategory);
    }
}
