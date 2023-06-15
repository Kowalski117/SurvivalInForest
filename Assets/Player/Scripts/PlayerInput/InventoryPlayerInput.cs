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

    public event UnityAction InteractKeyPressed;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.Inventory.performed += ctx => InteractCrafting();
        _playerInput.Player.Build.performed += ctx => InteractBuildingMenu();
        _playerInput.Player.Interact.performed += ctx => InteractKeyPressed?.Invoke();
    }

    private void OnDisable()
    {
        _playerInput.Player.Inventory.performed -= ctx => InteractCrafting();
        _playerInput.Player.Build.performed -= ctx => InteractBuildingMenu();
        _playerInput.Player.Interact.performed -= ctx => InteractKeyPressed?.Invoke();
        _playerInput.Disable();
    }

    private void InteractBuildingMenu()
    {
        OnBuildingWindow?.Invoke();
    }

    private void InteractCrafting() 
    {
        SwitchInventory?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
        OnCraftPlayerWindow?.Invoke(_manualWorkbench.Crafting—ategory);
    }
}
