using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;
    [SerializeField] private InventorySlotUI[] _slots;

    protected override void Start()
    {
        base.Start();

        if (_inventoryHolder != null)
        {
            inventorySystem = _inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssingSlot(inventorySystem);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (_slots.Length != inventorySystem.InventorySize)
            Debug.LogWarning($"Inventory slots out of sync on {this.gameObject}");

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(_slots[i], inventorySystem.InventorySlots[i]);
            _slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
