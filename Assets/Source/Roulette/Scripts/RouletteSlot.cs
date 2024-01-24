using UnityEngine;

public class RouletteSlot : MonoBehaviour
{
    public InventorySlotUI InventorySlotUI { get; private set; }

    private void Awake()
    {
        InventorySlotUI = GetComponentInChildren<InventorySlotUI>();
    }

    public void Init(InventoryItemData inventoryItemData)
    {
        InventorySlotUI.AssignedInventorySlot.AssignItem(inventoryItemData, 1, inventoryItemData.Durability);
        InventorySlotUI.UpdateUiSlot();
    }

    public void Clear()
    {
        InventorySlotUI.CleanSlot();
    }
}
