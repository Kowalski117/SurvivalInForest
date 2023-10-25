using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;

    private void Start()
    {
        RefreshStaticDisplay();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _inventoryHolder.OnInventoryChanged += RefreshStaticDisplay;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _inventoryHolder.OnInventoryChanged -= RefreshStaticDisplay;
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for (int i = 0; i < Slots.Length; i++)
        {
            slotDictionary.Add(Slots[i], inventoryToDisplay.InventorySlots[i + offSet]);
            Slots[i].Init(inventoryToDisplay.InventorySlots[i + offSet]);

            Slots[i].OnItemClicked += HandleItemSelection;
            Slots[i].OnItemBeginDrag += HandleBeginDrag;
            Slots[i].OnItemDroppedOn += HandleSwap;
            Slots[i].OnItemEndDrag += HandleEndDrag;
        }
    }

    private void RefreshStaticDisplay()
    {
        if (_inventoryHolder != null)
        {
            inventorySystem = _inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssingSlot(inventorySystem, 0);
    }

    public override void HandleSwap(InventorySlotUI inventorySlotUI)
    {
        int index = Array.IndexOf(Slots, inventorySlotUI);


        if (index == -1)
        {
            return;
        }
;
        SlotClicked(inventorySlotUI);
        HandleItemSelection(inventorySlotUI);
    }
}
