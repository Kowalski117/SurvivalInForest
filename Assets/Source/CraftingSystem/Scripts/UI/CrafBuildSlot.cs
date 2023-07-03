using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CraftBuildSlotView))]
public class CrafBuildSlot : CraftSlot
{
    private CraftBuildSlotView _slotView;

    public static UnityAction<BuildingRecipe> OnCreateRecipeButtonClick;

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

    private void CraftingItem(BuildingRecipe craftRecipe)
    {
        LoadingWindow.Instance.ShowLoadingWindow(2);
        OnCreateRecipeButtonClick?.Invoke(craftRecipe);
    }
}
