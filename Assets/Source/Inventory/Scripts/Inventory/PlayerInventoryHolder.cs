using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;
    public event UnityAction<InventoryItemData, int> OnItemDataChanged;
    public event UnityAction OnUpdateItemSlot;

    public bool AddToInventory(InventoryItemData data, int amount, float durability = 0)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount, durability))
        {
            OnItemDataChanged?.Invoke(data, amount);
            OnUpdateItemSlot?.Invoke();
            return true;
        }

        return false;
    }

    public bool RemoveInventory(InventoryItemData data, int amount)
    {
        if(PrimaryInventorySystem.RemoveItemsInventory(data, amount))
        {
            OnItemDataChanged?.Invoke(data, -amount);
            OnUpdateItemSlot?.Invoke();
            return true;
        }

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

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation);
        ES3.Save("InventoryData", saveData);
    }

    protected override void LoadInventory()
    {
        InventorySaveData saveData = ES3.Load<InventorySaveData>("InventoryData", new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation));
        PrimaryInventorySystem = saveData.InventorySystem;
        transform.position = saveData.Position;
        transform.rotation = saveData.Rotation;
        OnPlayerInventoryChanged?.Invoke();
    }
}