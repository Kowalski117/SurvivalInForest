using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private AnimationUI _animationUI;

    public event UnityAction<int> OnDisplayRefreshed;

    public AnimationUI AnimationUI => _animationUI;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }

    public void Open()
    {
        _animationUI.Open();
    }

    public void Close()
    {
        _animationUI.Close();
    }

    public void Refresh(InventorySystem inventoryToSystem, int offSet)
    {
        ClearSlots();
        _inventorySystem = inventoryToSystem;

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;

        OnDisplayRefreshed?.Invoke(_inventorySystem.InventorySize);
        AssingSlots(inventoryToSystem, offSet);
    }

    public override void AssingSlots(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < Slots.Length; i++)
        {
            if (i < inventoryToDisplay.InventorySize)
            {
                InventorySlotUI viewSlot = Slots[i];
                slotDictionary.Add(viewSlot, inventoryToDisplay.InventorySlots[i + offSet]);
                viewSlot.gameObject.SetActive(true);
                viewSlot.Init(inventoryToDisplay.InventorySlots[i + offSet]);
            }
            else
                Slots[i].gameObject.SetActive(false);
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }

        slotDictionary?.Clear();
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
