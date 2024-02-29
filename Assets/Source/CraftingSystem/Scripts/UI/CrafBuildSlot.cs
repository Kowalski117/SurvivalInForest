using System;
using UnityEngine;

[RequireComponent(typeof(CraftBuildSlotView))]
public class CrafBuildSlot : CraftSlot
{
    private CraftBuildSlotView _slotView;

    public static Action OnCraftSlotUpdated;
    public static Action<BuildingRecipe> OnCreatedRecipeButton;

    private void Awake()
    {
        _slotView = GetComponent<CraftBuildSlotView>();
    }

    private void OnEnable()
    {
        _slotView.OnCreatedRecipeButtonClick += CraftingItem;
    }

    private void OnDisable()
    {
        _slotView.OnCreatedRecipeButtonClick -= CraftingItem;
    }

    private void CraftingItem(BuildingRecipe craftRecipe)
    {
        OnCraftSlotUpdated?.Invoke();
        OnCreatedRecipeButton?.Invoke(craftRecipe);
    }
}
