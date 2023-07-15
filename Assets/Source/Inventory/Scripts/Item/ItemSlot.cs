using UnityEngine;
using UnityEngine.Events;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected InventoryItemData InventoryItemData;
    [SerializeField] protected int ItemId = -1;
    [SerializeField] protected int StackSize;
    [SerializeField] protected float MaxDurabilityValue;

    protected float _currentDurability;

    public event UnityAction<float> OnDurabilityChanged;

    public float DurabilityPercent => _currentDurability / MaxDurabilityValue;
    public InventoryItemData ItemData => InventoryItemData;
    public int Size => StackSize;
    public float Durability => _currentDurability;

    public void ClearSlot()
    {
        InventoryItemData = null;
        ItemId = -1;
        StackSize = -1;
        MaxDurabilityValue = -1;
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
            MaxDurabilityValue = inventorySlot.ItemData.Durability;
            _currentDurability = inventorySlot.Durability;
            AddToStack(inventorySlot.StackSize);
        }
    }

    public void AssignItem(InventoryItemData data, int amount, float durability)
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
            MaxDurabilityValue = data.Durability;
            _currentDurability = durability;
            AddToStack(amount);
        }
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;

        //if (StackSize >= 1)
        //    _currentDurability = MaxDurabilityValue;
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
        _currentDurability -= amount;
    }

    public void UpdateDurabilityIfNeeded()
    {
        if (_currentDurability <= 0 && StackSize > 1)
            _currentDurability = MaxDurabilityValue;
    }
}