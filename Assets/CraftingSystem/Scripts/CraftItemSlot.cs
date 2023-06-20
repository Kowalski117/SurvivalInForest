using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CraftItemSlotView))]
public class CraftItemSlot : CraftSlot
{
    private CraftItemSlotView _slotView;

    public static UnityAction OnCraftSlotUpdate;

    private void Awake()
    {
        _slotView = GetComponent<CraftItemSlotView>();
    }

    private void OnEnable()
    {
        _slotView.OnCreateRecipeButtonClick += CraftingItem;
    }

    private void OnDisable()
    {
        _slotView.OnCreateRecipeButtonClick -= CraftingItem;
    }

    private void CraftingItem(ItemRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
    {
        if (CheckIfCanCraft(craftRecipe, playerInventoryHolder))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                playerInventoryHolder.InventorySystem.RemoveItemsInventory(ingredient.ItemRequired, ingredient.AmountRequured);
            }

            playerInventoryHolder.AddToInventory(craftRecipe.CraftedItem, craftRecipe.CraftingAmount, true);
            OnCraftSlotUpdate?.Invoke();
        }
    }
}
