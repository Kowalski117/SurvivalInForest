using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private BackpackInventory _backpackInventory;
    [SerializeField] private ClothesInventory _clothesInventory;
    [SerializeField] private ChestHandler _chestHandler;
     
    private InventoryItemData _currentItemData;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;
    public event UnityAction<InventoryItemData, int> OnItemDataChanged;
    public event UnityAction OnUpdateItemSlot;
    public event UnityAction<InventorySlot, InventoryItemData> OnClearItemSlot;

    public bool AddToInventory(InventoryItemData data, int amount, float durability = 0)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount, durability) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.AddToInventory(data, amount, durability))
        {
            OnItemDataChanged?.Invoke(data, amount);
            OnUpdateItemSlot?.Invoke();
            return true;
        }
        return false;
    }

    public bool RemoveInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.RemoveItemsInventory(data, amount) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.RemoveItemsInventory(data, amount) || _clothesInventory.InventorySystem.RemoveItemsInventory(data, amount))
        {
            OnItemDataChanged?.Invoke(data, -amount);
            OnUpdateItemSlot?.Invoke();
            return true;
        }

        return false;
    }

    public bool RemoveInventory(InventorySlot slot, int amount)
    {
        _currentItemData = slot.ItemData;

        if (PrimaryInventorySystem.RemoveItemsInventory(slot, amount) || _backpackInventory.IsEnable == true && _backpackInventory.InventorySystem.RemoveItemsInventory(slot, amount) || _clothesInventory.InventorySystem.RemoveItemsInventory(slot, amount) || _chestHandler.ChestInventory != null && _chestHandler.ChestInventory.InventorySystem.RemoveItemsInventory(slot, amount))
        {
            OnItemDataChanged?.Invoke(_currentItemData, -amount);
            OnUpdateItemSlot?.Invoke();
            OnClearItemSlot?.Invoke(slot, _currentItemData);
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

    public void DeletePartOfInventory(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, PrimaryInventorySystem.GetAllFilledSlots().Count);
            InventorySlot slot = PrimaryInventorySystem.GetAllFilledSlots()[index];
            int amountRandom = Random.Range(1, slot.Size);
            RemoveInventory(slot, amountRandom);
        }
    }

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.PlayerInvetory, saveData);
    }

    protected override void LoadInventory()
    {
        if (ES3.KeyExists(SaveLoadConstants.PlayerInvetory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.PlayerInvetory);
            _playerInputHandler.FirstPersonController.enabled = false;
            PrimaryInventorySystem = saveData.InventorySystem;
            _playerInputHandler.FirstPersonController.enabled = true;

            base.LoadInventory();
        }
    }
}