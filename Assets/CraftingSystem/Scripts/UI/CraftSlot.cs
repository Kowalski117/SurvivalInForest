using UnityEngine;

public class CraftSlot : MonoBehaviour
{
    protected bool CheckIfCanCraft(CraftRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
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
