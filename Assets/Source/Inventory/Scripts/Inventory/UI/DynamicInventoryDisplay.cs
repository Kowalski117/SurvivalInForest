using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Transform _parentTransform;

    public CanvasGroup CanvasGroup => _canvasGroup;

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }

    public void RefreshDynamicInventory(InventorySystem inventoryToSystem, int offSet)
    {
        ClearSlots();
        inventorySystem = inventoryToSystem;

        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot;

        AssingSlot(inventoryToSystem, offSet);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay, int offSet)
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
                viewSlot.UpdateUiSlot();

                Slots[i].OnItemClicked += HandleItemSelection;
                Slots[i].OnItemBeginDrag += HandleBeginDrag;
                Slots[i].OnItemDroppedOn += HandleSwap;
                Slots[i].OnItemEndDrag += HandleEndDrag;
            }
            else
            {
                Slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }

        if (slotDictionary != null)
            slotDictionary.Clear();
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
