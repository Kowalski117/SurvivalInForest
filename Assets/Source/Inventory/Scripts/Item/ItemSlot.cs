using UnityEngine;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected InventoryItemData InventoryItemData;
    [SerializeField] protected int ItemId = -1;
    [SerializeField] protected int StackSize;
    [SerializeField] protected float DurabilityValue;

    private float _currentDurability;

    public InventoryItemData ItemData => InventoryItemData;
    public int Size => StackSize;
    public float Durability => DurabilityValue;

    public void ClearSlot()
    {
        InventoryItemData = null;
        ItemId = -1;
        StackSize = -1;
        DurabilityValue = - 1;
        _currentDurability = -1;
    }

    public void AssignItem(InventorySlot inventorySlot)
    {
        if (ItemData == inventorySlot.ItemData)
        {
            AddToStack(inventorySlot.StackSize);
        }
        else
        {
            InventoryItemData = inventorySlot.ItemData;
            ItemId = ItemData.Id;
            StackSize = 0;
            DurabilityValue = inventorySlot.Durability;
            AddToStack(inventorySlot.StackSize);
        }
    }

    public void AssignItem(InventoryItemData data, int amount)
    {
        if (ItemData == data)
        {
            AddToStack(amount);
        }
        else
        {
            InventoryItemData = data; 
            ItemId = data.Id;
            StackSize = 0;
            DurabilityValue = data.Durability;
            AddToStack(amount);
        }
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;

        if (StackSize > 1)
            _currentDurability = DurabilityValue;
    }

    public void RemoveFromStack(int amount)
    {
        StackSize -= amount;

        if(StackSize <= 0) 
            ClearSlot();
    }

    public void OnAfterDeserialize()
    {
        if (ItemId == -1)
            return;

        var dataBase = Resources.Load<DataBase>("Database");
        InventoryItemData = dataBase.GetItem(ItemId);
    }

    public void OnBeforeSerialize()
    {

    }

    public void LowerStrength(float amount)
    {
        DurabilityValue -= amount;
    }

    public void UpdateDurabilityIfNeeded()
    {
        if (DurabilityValue <= 0 && StackSize > 1)
            DurabilityValue = _currentDurability;
    }
}

public struct InventoryItem
{
    [SerializeField] private InventoryItemData _inventoryItemData;
    [SerializeField] private int _stackSize;
    [SerializeField] private int _itemId;
    [SerializeField] private bool _isEmpty => _inventoryItemData == null;

    
}