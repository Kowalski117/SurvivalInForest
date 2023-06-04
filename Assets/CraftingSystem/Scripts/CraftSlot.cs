using UnityEngine;

[RequireComponent(typeof(CraftSlotView))]
public class CraftSlot : MonoBehaviour
{
    private CraftSlotView _slotView;

    private void Awake()
    {
        _slotView = GetComponent<CraftSlotView>();
    }

    private void OnEnable()
    {
        _slotView.OnCreateRecipeButtonClick += CraftingItem;
    }

    private void OnDisable()
    {
        _slotView.OnCreateRecipeButtonClick -= CraftingItem;
    }

    private void CraftingItem(CraftRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
    {
        if (CheckIfCanCraft(craftRecipe, playerInventoryHolder))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                playerInventoryHolder.InventorySystem.RemoveItemsInventory(ingredient.ItemRequired, ingredient.AmountRequured);
                Debug.Log(ingredient.ItemRequired);
            }

            playerInventoryHolder.AddToInventory(craftRecipe.CraftedItem, craftRecipe.CraftingAmount, true);
        }
    }

    private bool CheckIfCanCraft(CraftRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
    {
        var itemsHeld = playerInventoryHolder.InventorySystem.GetAllItemsHeld();

        foreach (var ingredient in craftRecipe.CraftingIngridients)
        {
            if (!itemsHeld.TryGetValue(ingredient.ItemRequired, out int amountHeld))
                return false;

            if (amountHeld < ingredient.AmountRequured)
                return false;
        }

        return true;
    }
}
