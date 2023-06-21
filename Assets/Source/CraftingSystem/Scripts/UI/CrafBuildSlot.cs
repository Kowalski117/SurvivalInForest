using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CraftBuildSlotView))]
public class CrafBuildSlot : CraftSlot
{
    private CraftBuildSlotView _slotView;

    public static UnityAction<BuildingData> OnCreateRecipeButtonClick;
    public static UnityAction OnCraftSlotUpdate;

    private void Awake()
    {
        _slotView = GetComponent<CraftBuildSlotView>();
    }

    private void OnEnable()
    {
        _slotView.OnCreateRecipeButtonClick += CraftingItem;
    }

    private void OnDisable()
    {
        _slotView.OnCreateRecipeButtonClick -= CraftingItem;
    }

    private void CraftingItem(BuildingRecipe craftRecipe, PlayerInventoryHolder playerInventoryHolder)
    {
        if (CheckIfCanCraft(craftRecipe, playerInventoryHolder))
        {
            foreach (var ingredient in craftRecipe.CraftingIngridients)
            {
                playerInventoryHolder.InventorySystem.RemoveItemsInventory(ingredient.ItemRequired, ingredient.AmountRequured);
            }

            OnCreateRecipeButtonClick?.Invoke(craftRecipe.BuildingData);
            OnCraftSlotUpdate?.Invoke();
        }
    }
}
