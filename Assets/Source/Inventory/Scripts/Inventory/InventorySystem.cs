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
        CreateInventory(size);
    }

    public InventorySystem(int size, int gold)
    {
        CreateInventory(size);
    }

    public bool AddToInventory(InventoryItemData item, int amount)
    {
        if(ContainsItem(item, out List<InventorySlot> inventorySlots))
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.EnoughRoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }
        
        if(HasFreeSlot(out InventorySlot freeSlot))
        {
            if (freeSlot.EnoughRoomLeftInStack(amount))
            {
                freeSlot.UpdateInventorySlot(item, amount);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }
        return false;
    }

    public bool RemoveItemsInventory(InventoryItemData data, int amount)
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

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> inventorySlots)
    {
        inventorySlots = _inventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        return inventorySlots == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = _inventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }

    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {
        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData, this.InventorySlots[i].Size);

        }

        foreach (var item in shoppingCart)
        {
            for (int i = 0; i < item.Value; i++)
            {
                if (!clonedSystem.AddToInventory(item.Key, 1))
                    return false;
            }
        }

        return true;
    }

    private void CreateInventory(int size)
    {
        _inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
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
}
