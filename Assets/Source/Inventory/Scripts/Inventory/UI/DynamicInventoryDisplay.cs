using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventorySlotUI _slotPrefab;

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

        if(inventorySystem != null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot;

        AssingSlot(inventoryToSystem, offSet);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = offSet; i < inventoryToDisplay.InventorySize; i++)
        {
            var viewSlot = Instantiate(_slotPrefab, transform); // переделать для оптимизации
            slotDictionary.Add(viewSlot, inventoryToDisplay.InventorySlots[i]);
            viewSlot.Init(inventoryToDisplay.InventorySlots[i]);
            viewSlot.UpdateUiSlot();
        }
    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject); // переделать для оптимизации
        }

        if(slotDictionary!= null)
            slotDictionary.Clear();
    }
}
