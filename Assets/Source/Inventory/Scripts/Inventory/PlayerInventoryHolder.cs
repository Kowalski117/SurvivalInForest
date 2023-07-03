using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;
    public event UnityAction<InventoryItemData, int> OnItemDataChanged;

    private void Start()
    {
        SaveGameHandler.Data._playerInventory = new InventorySaveData(PrimaryInventorySystem); // ��������
    }

    public bool AddToInventory(InventoryItemData data, int amount, bool spawnItemOnFail = false)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            OnItemDataChanged?.Invoke(data, amount);
            return true;
        }

        if(spawnItemOnFail)
            Instantiate(data.ItemPrefab, transform.position + transform.forward, Quaternion.identity);

        return false;
    }

    public bool RemoveInventory(InventoryItemData data, int amount)
    {
        if(PrimaryInventorySystem.RemoveItemsInventory(data, amount))
        {
            OnItemDataChanged?.Invoke(data, -amount);
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