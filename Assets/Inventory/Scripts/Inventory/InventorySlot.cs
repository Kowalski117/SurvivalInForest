using UnityEngine;

[System.Serializable]
public class InventorySlot : ISerializationCallbackReceiver
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private int _itemId = -1;
    [SerializeField] private int _stackSize;

    public InventoryItemData ItemData => _itemData;
    public int StackSize => _stackSize;

    public InventorySlot(InventoryItemData source, int amount)
    {
        _itemData = source;
        _itemId = _itemData.Id;
        _stackSize = amount;
    }

    public InventorySlot()
    {
        CleanSlot();
    }

    public void CleanSlot()
    {
        _itemData = null;
        _itemId = -1;
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
            _itemId = _itemData.Id;
            _stackSize = 0;
            AddToStack(inventorySlot.StackSize);
        }
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        _itemData = data;
        _itemId = _itemData.Id;
        _stackSize = amount;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - _stackSize; 
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if(_itemData == null || _itemData != null && _stackSize + amountToAdd <= _itemData.MaxStackSize)
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

    public bool SplitStack(out InventorySlot splitSlot)
    {
        if(_stackSize <= 1)
        {
            splitSlot = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(_stackSize / 2);
        RemoveFromStack(halfStack);

        splitSlot = new InventorySlot(_itemData, halfStack);
        return true;
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (_itemId == -1)
            return;

        var dataBase = Resources.Load<DataBase>("Database");
        _itemData = dataBase.GetItem(_itemId);
    }
}
