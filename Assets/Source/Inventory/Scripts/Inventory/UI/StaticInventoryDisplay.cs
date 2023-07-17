using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;

    protected override void Start()
    {
        RefreshStaticDisplay();
    }

    protected virtual void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    protected virtual void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
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
}
