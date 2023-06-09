using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    public InventorySlot(InventoryItemData source, int amount)
    {
        InventoryItemData = source;
        ItemId = ItemData.Id;
        StackSize = amount;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        InventoryItemData = data;
        ItemId = ItemData.Id;
        StackSize = amount;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize; 
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if(ItemData == null || ItemData != null && StackSize + amountToAdd <= ItemData.MaxStackSize)
            return true;
        else
            return false;
    }

    public bool SplitStack(out InventorySlot splitSlot)
    {
        if(StackSize <= 1)
        {
            splitSlot = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(StackSize / 2);
        RemoveFromStack(halfStack);

        splitSlot = new InventorySlot(ItemData, halfStack);
        return true;
    }
}
