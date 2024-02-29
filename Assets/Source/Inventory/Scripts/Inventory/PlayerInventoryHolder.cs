using System;
using UnityEngine;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private BackpackInventory _backpackInventory;
    [SerializeField] private ClothesInventory _clothesInventory;
    [SerializeField] private ChestHandler _chestHandler;
     
    private InventoryItemData _currentItemData;

    public static Action<InventorySystem, int> OnPlayerInventoryDispleyRequested;
    public event Action<InventoryItemData, int> OnItemDataChanged;
    public event Action OnItemSlotUpdated;
    public event Action<InventorySlot, InventoryItemData> OnItemSlotCleared;

    public bool AddItem(InventoryItemData data, int amount, float durability = 0)
    {
        if (PrimaryInventorySystem.AddItem(data, amount, durability) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.AddItem(data, amount, durability))
        {
            OnItemDataChanged?.Invoke(data, amount);
            OnItemSlotUpdated?.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveItem(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.RemoveItem(data, amount) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.RemoveItem(data, amount) || _clothesInventory.InventorySystem.RemoveItem(data, amount))
        {
            OnItemDataChanged?.Invoke(data, -amount);
            OnItemSlotUpdated?.Invoke();
            return true;
        }

        return false;
    }

    public bool RemoveSlot(InventorySlot slot, int amount)
    {
        _currentItemData = slot.ItemData;

        if (PrimaryInventorySystem.RemoveSlot(slot, amount) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.RemoveSlot(slot, amount) || _clothesInventory.InventorySystem.RemoveSlot(slot, amount) || _chestHandler.ChestInventory != null && _chestHandler.ChestInventory.InventorySystem.RemoveSlot(slot, amount))
        {
            OnItemDataChanged?.Invoke(_currentItemData, -amount);
            OnItemSlotUpdated?.Invoke();
            OnItemSlotCleared?.Invoke(slot, _currentItemData);
            _currentItemData = null;
            return true;
        }

        _currentItemData = null;
        return false;
    }

    public bool CheckIfCanCraft(CraftRecipe craftRecipe)
    {
        var itemsHeld = InventorySystem.GetAllItemsHeld();

        foreach (var ingredient in craftRecipe.CraftingIngridients)
        {
            if (!itemsHeld.TryGetValue(ingredient.ItemRequired, out int amountHeld))
                return false;

            if (amountHeld < ingredient.AmountRequured)
                return false;
        }
        return true;
    }

    public void DeletePart(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, PrimaryInventorySystem.GetAllFilledSlots().Count);
            InventorySlot slot = PrimaryInventorySystem.GetAllFilledSlots()[index];
            int amountRandom = UnityEngine.Random.Range(1, slot.Size);
            RemoveSlot(slot, amountRandom);
        }
    }

    protected override void Save()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.PlayerInvetory, saveData);
    }

    protected override void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.PlayerInvetory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.PlayerInvetory);
            PrimaryInventorySystem = saveData.InventorySystem;
            base.Load();
        }
    }
}