using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> _inventorySlots;

    public event UnityAction<InventorySlot> OnInventorySlotChanged;

    public List<InventorySlot> InventorySlots => _inventorySlots;
    public int InventorySize => _inventorySlots.Count;

    public InventorySystem(int size)
    {
        Create(size);
    }

    public bool AddItem(InventoryItemData item, int amount, float durability)
    {
        if (ContainsItem(item, out List<InventorySlot> inventorySlots))
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.EnoughRoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
                else if (slot.ItemData != null) 
                {
                    int spaceLeft = slot.ItemData.MaxStackSize - slot.Size; 
                    slot.AddToStack(spaceLeft); 
                    OnInventorySlotChanged?.Invoke(slot);

                    amount -= spaceLeft;
                }
            }
        }

        foreach (var slot in _inventorySlots)
        {
            if (slot.ItemData == null)
            {
                int stackSizeToAdd = Mathf.Min(amount, item.MaxStackSize);
                slot.UpdateItem(item, stackSizeToAdd, durability);
                OnInventorySlotChanged?.Invoke(slot);

                amount -= stackSizeToAdd;

                if (amount <= 0)
                    return true;
            }
        }

        return false;
    }


    public bool RemoveItem(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out List<InventorySlot> inventorySlots))
        {
            int totalAmount = inventorySlots.Sum(slot => slot.Size);

            if (totalAmount >= amount)
            {
                int remainingAmount = amount;

                foreach (var slot in inventorySlots)
                {
                    int stackSize = slot.Size;

                    if (stackSize >= remainingAmount)
                    {
                        slot.RemoveFromStack(remainingAmount);
                        OnInventorySlotChanged?.Invoke(slot);
                        return true;
                    }
                    else if (stackSize > 0)
                    {
                        slot.RemoveFromStack(stackSize);
                        remainingAmount -= stackSize;
                    }
                }
            }
        }

        return false;
    }

    public bool RemoveSlot(InventorySlot slot, int amount)
    {
        foreach (var inventorySlot in _inventorySlots)
        {
            if(inventorySlot == slot)
            {
                if (slot != null && slot.ItemData != null)
                {
                    int stackSize = slot.Size;

                    if (stackSize >= amount)
                    {
                        slot.RemoveFromStack(amount);
                        OnInventorySlotChanged?.Invoke(slot);
                        return true;
                    }
                    else if (stackSize > 0)
                    {
                        slot.RemoveFromStack(stackSize);
                        OnInventorySlotChanged?.Invoke(slot);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> inventorySlots)
    {
        inventorySlots = _inventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        return inventorySlots == null ? false : true;
    }

    public Dictionary<InventoryItemData, int> GetAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in _inventorySlots)
        {
            if (item.ItemData == null)
                continue;

            if (!distinctItems.ContainsKey(item.ItemData))
                distinctItems.Add(item.ItemData, item.Size);
            else
                distinctItems[item.ItemData] += item.Size;
        }

        return distinctItems;
    }

    public int GetItemCount(InventoryItemData item)
    {
        int count = 0;

        foreach (var slot in _inventorySlots)
        {
            if (slot.ItemData == item)
            {
                count += slot.Size;
            }
        }

        if (count <= 0)
            count = 0;

        return count;
    }

    public List<InventorySlot> GetAllFilledSlots()
    {
        return _inventorySlots.Where(slot => slot.ItemData != null).ToList();
    }

    private void Create(int size)
    {
        _inventorySlots = new List<InventorySlot>(size);
 
        for (int i = 0; i < size; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
    }
}
