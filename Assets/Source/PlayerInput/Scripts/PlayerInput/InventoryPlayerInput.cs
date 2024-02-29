using System;
using UnityEngine;

public class InventoryPlayerInput : PlayerInputAction
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ManualWorkbench _manualWorkbench;

    public event Action<InventorySystem, int> OnInventorySwitched;
    public event Action<Crafting—ategory> OnCraftPlayerWindowToggled;
    public event Action OnInventoryToggled;

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerInput.Player.Inventory.performed += ctx => Toggle();
    }

    protected override void OnDisable()
    {
        PlayerInput.Player.Inventory.performed -= ctx => Toggle();

        base.OnDisable();
    }

    public void Toggle() 
    {
        OnInventoryToggled?.Invoke();
        OnInventorySwitched?.Invoke(_inventoryHolder.InventorySystem, _inventoryHolder.Offset);
        OnCraftPlayerWindowToggled?.Invoke(_manualWorkbench.Crafting—ategory);
    }
}
