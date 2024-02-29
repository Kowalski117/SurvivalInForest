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
        _slotView.OnCreatedRecipeButtonClick += Crafting;
    }

    private void OnDisable()
    {
        _slotView.OnCreatedRecipeButtonClick -= Crafting;
    }

    private void Crafting(ItemRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
    {
        if (CheckIfCanCraft(craftRecipe, playerInventoryHolder))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                playerInventoryHolder.RemoveItem(ingredient.ItemRequired, ingredient.AmountRequured);
            }

            playerInventoryHolder.AddItem(craftRecipe.CraftedItem, craftRecipe.CraftingAmount, craftRecipe.CraftedItem.Durability);
            OnCraftSlotUpdate?.Invoke();
        }
    }
}
