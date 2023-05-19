using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private int _stackSize;

    public InventoryItemData ItemData => _itemData;
    public int StackSize => _stackSize;

    public InventorySlot(InventoryItemData source, int amount)
    {
        _itemData = source;
        _stackSize = amount;
    }

    public InventorySlot()
    {
        CleanSlot();
    }

    public void CleanSlot()
    {
        _itemData = null;
        _stackSize = -1;
    }

    public void AssignItem(InventorySlot inventorySlot)
    {
        if (_itemData == inventorySlot.ItemData)
        {
            AddToStack(inventorySlot.StackSize);
        }
        else
        {
            _itemData = inventorySlot.ItemData;
            _stackSize = 0;
            AddToStack(inventorySlot.StackSize);
        }

    }

    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        _itemData = data;
        _stackSize = amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - _stackSize; 
        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        if(_stackSize + amountToAdd <= _itemData.MaxStackSize)
            return true;
        else
            return false;
    }

    public void AddToStack(int amount)
    {
        _stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        _stackSize -= amount;
    }
}
