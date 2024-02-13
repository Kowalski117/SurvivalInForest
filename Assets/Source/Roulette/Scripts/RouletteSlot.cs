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
        InventorySlotUI.UpdateUiSlot();
    }

    public void Clear()
    {
        InventorySlotUI.CleanSlot();
    }
}
