using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    public InventorySlot(InventoryItemData source, int amount, float durability)
    {
        InventoryItemData = source;
        ItemId = ItemData.Id;
        StackSize = amount;
        MaxDurabilityValue = source.Durability;
        ÑurrentDurability = durability;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount, float durability)
    {
        InventoryItemData = data;
        ItemId = ItemData.Id;
        StackSize = amount;
        MaxDurabilityValue = data.Durability;
        ÑurrentDurability = durability;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (ItemData != null)
        {
            if (ItemData.MaxStackSize > 1)
            {
                return StackSize + amountToAdd <= ItemData.MaxStackSize;
            }
            else
            {
                return StackSize < 1; // Ìîæíî äîáàâèòü òîëüêî îäèí ïðåäìåò
            }
        }
        return false;
    }

    public bool SplitStack(out InventorySlot splitSlot)
    {
        if (StackSize <= 1)
        {
            splitSlot = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(StackSize / 2);
        RemoveFromStack(halfStack);

        splitSlot = new InventorySlot(ItemData, halfStack, ÑurrentDurability);
        return true;
    }
}
