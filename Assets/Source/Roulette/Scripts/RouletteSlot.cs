using UnityEngine;

public class RouletteSlot : MonoBehaviour
{
    public InventorySlotUI InventorySlotUI { get; private set; }

    private void Awake()
    {
        InventorySlotUI = GetComponentInChildren<InventorySlotUI>();
    }

    public void Init(InventoryItem inventoryItemData)
    {
        InventorySlotUI.AssignedInventorySlot.AssignItem(inventoryItemData.ItemData, inventoryItemData.Amount, inventoryItemData.ItemData.Durability);
        InventorySlotUI.UpdateItem();
    }

    public void Clear()
    {
        InventorySlotUI.Clear();
    }
}
