using UnityEngine;
using UnityEngine.Events;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [SerializeField] protected InventoryItemData InventoryItemData;
    [SerializeField] protected int ItemId = -1;
    [SerializeField] protected int StackSize;
    [SerializeField] protected float MaxDurabilityValue;

    protected float ĐurrentDurability;

    public event UnityAction<float> OnDurabilityChanged;

    public float DurabilityPercent => ĐurrentDurability / MaxDurabilityValue;
    public InventoryItemData ItemData => InventoryItemData;
    public int Size => StackSize;
    public float Durability => ĐurrentDurability;

    public void ClearSlot()
    {
        InventoryItemData = null;
        ItemId = -1;
        StackSize = -1;
        MaxDurabilityValue = -1;
        ĐurrentDurability = -1;
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
            ĐurrentDurability = inventorySlot.Durability;
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
            ĐurrentDurability = durability;
            AddToStack(amount);
        }
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;
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
        ĐurrentDurability -= amount;
    }

    public void UpdateDurabilityIfNeeded()
    {
        if (ĐurrentDurability <= 0 && StackSize > 1)
            ĐurrentDurability = MaxDurabilityValue;
    }
}