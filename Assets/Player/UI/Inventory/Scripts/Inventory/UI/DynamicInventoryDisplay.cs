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

    public void RefreshDynamicInventory(InventorySystem inventoryToSystem)
    {
        ClearSlots();
        inventorySystem = inventoryToSystem;
        AssingSlot(inventoryToSystem);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay)
    {
        ClearSlots();

        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
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
