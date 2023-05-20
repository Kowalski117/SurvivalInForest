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

    public void RefreshDynamicInventory(InventorySystem inventoryToSystem)
    {
        ClearSlots();
        inventorySystem = inventoryToSystem;

        if(inventorySystem != null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot;

        AssingSlot(inventoryToSystem);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            var viewSlot = Instantiate(_slotPrefab, transform); // ���������� ��� �����������
            slotDictionary.Add(viewSlot, inventoryToDisplay.InventorySlots[i]);
            viewSlot.Init(inventoryToDisplay.InventorySlots[i]);
            viewSlot.UpdateUiSlot();
        }
    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject); // ���������� ��� �����������
        }

        if(slotDictionary!= null)
            slotDictionary.Clear();
    }
}
