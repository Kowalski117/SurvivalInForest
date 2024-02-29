using UnityEngine;

[System.Serializable]
public class InventorySlot : ItemSlot
{
    private float _addAmount = 1;
    private float _divider = 2;

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

    public void UpdateItem(InventoryItemData data, int amount, float durability)
    {
        InventoryItemData = data;
        ItemId = ItemData.Id;
        StackSize = amount;
        MaxDurabilityValue = data.Durability;
        ÑurrentDurability = durability;
    }

    public bool EnoughRoomLeftInStack(int amountAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize;
        return EnoughRoomLeftInStack(amountAdd);
    }

    public bool EnoughRoomLeftInStack(int amountAdd)
    {
        if (ItemData != null)
        {
            if (ItemData.MaxStackSize > _addAmount)
                return StackSize + amountAdd <= ItemData.MaxStackSize;
            else
                return StackSize < _addAmount;
        }

        return false;
    }

    public bool SplitStack(out InventorySlot splitSlot)
    {
        if (StackSize <= _addAmount)
        {
            splitSlot = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(StackSize / _divider);
        RemoveFromStack(halfStack);

        splitSlot = new InventorySlot(ItemData, halfStack, ÑurrentDurability);
        return true;
    }
}
