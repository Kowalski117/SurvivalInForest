using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;

    private void Start()
    {
        Refresh();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _inventoryHolder.OnInventoryChanged += Refresh;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _inventoryHolder.OnInventoryChanged -= Refresh;
    }

    public override void AssingSlots(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for (int i = 0; i < Slots.Length; i++)
        {
            slotDictionary.Add(Slots[i], inventoryToDisplay.InventorySlots[i + offSet]);
            Slots[i].Init(inventoryToDisplay.InventorySlots[i + offSet]);
        }
    }

    private void Refresh()
    {
        if (_inventoryHolder != null)
        {
            _inventorySystem = _inventoryHolder.InventorySystem;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }

        AssingSlots(_inventorySystem, 0);
    }

    public override void HandleSwap(InventorySlotUI inventorySlotUI)
    {
        int index = Array.IndexOf(Slots, inventorySlotUI);

        if (index == -1)
            return;
;
        SlotClicked(inventorySlotUI);
        HandleItemSelection(inventorySlotUI);
    }
}
