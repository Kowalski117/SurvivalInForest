using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;

    private void Start()
    {
        SaveGameHandler.Data._playerInventory = new InventorySaveData(PrimaryInventorySystem); // поменять
    }

    public bool AddToInventory(InventoryItemData data, int amount, bool spawnItemOnFail = false)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        if(spawnItemOnFail)
            Instantiate(data.ItemPrefab, transform.position + transform.forward, Quaternion.identity);

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

    public void ToggleInventory()
    {
        OnPlayerInventoryDispleyRequested?.Invoke(PrimaryInventorySystem, Offset);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.PlayerInventory.InventorySystem != null)
        {
            this.PrimaryInventorySystem = data.PlayerInventory.InventorySystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
}