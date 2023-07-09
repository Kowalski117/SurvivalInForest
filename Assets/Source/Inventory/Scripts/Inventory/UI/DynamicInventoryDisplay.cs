using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventorySlotUI[] _slots;

    protected override void Start()
    {
        base.Start();
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

        // Включаем или выключаем слоты в зависимости от размера инвентаря
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < inventoryToDisplay.InventorySize)
            {
                InventorySlotUI viewSlot = _slots[i];
                slotDictionary.Add(viewSlot, inventoryToDisplay.InventorySlots[i + offSet]);
                viewSlot.gameObject.SetActive(true);
                viewSlot.Init(inventoryToDisplay.InventorySlots[i + offSet]);
                viewSlot.UpdateUiSlot();
            }
            else
            {
                // Если слоты в массиве _slots закончились, выключаем оставшиеся слоты
                _slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in _slots)
        {
            slot.gameObject.SetActive(false);
        }

        if (slotDictionary != null)
            slotDictionary.Clear();
    }
}
