using System;
using UnityEngine.Events;

public class CraftBuildSlotView : CraftSlotView
{
    private BuildingRecipe _recipe;

    public event UnityAction<BuildingRecipe> OnCreateRecipeButtonClick;

    public BuildingRecipe Recipe => _recipe;

    private void OnEnable()
    {
        CraftedButton.onClick.AddListener(OnCreateRecipeButton);
    }

    private void OnDisable()
    {
        CraftedButton.onClick.RemoveListener(OnCreateRecipeButton);
    }

    public void Init(PlayerInventoryHolder playerInventory, BuildingRecipe craftRecipe, Crafting�ategory �ategory, LoadingWindow loadingWindow)
    {
        CraftedTime = DateTime.MinValue;
        _recipe = craftRecipe;
        InventoryHolder = playerInventory;
        LoadingWindow = loadingWindow;
        Crafting�ategory = �ategory;
        CraftedIcon.sprite = craftRecipe.BuildingData.Icon;
        CraftedName.text = craftRecipe.BuildingData.DisplayName;
        CraftedTime = CraftedTime + TimeSpan.FromHours(craftRecipe.CraftingTime);
        CraftedTimeText.text = CraftedTime.ToString("HH:mm");

        foreach (var ingridient in craftRecipe.CraftingIngridients)
        {
            IngridientSlotView slotView = Instantiate(SlotIngridientPrefab, IngridientsContainer);
            slotView.Init(playerInventory, ingridient.ItemRequired, ingridient.AmountRequured);
            IngridientSlots.Add(slotView);
        }
    }

    public void OnCreateRecipeButton()
    {
        if (InventoryHolder.CheckIfCanCraft(_recipe))
        {
            LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.BuildingData.DisplayName, ActionType.CraftBuild);
            LoadingWindow.OnLoadingComplete += OnLoadingComplete;
        }
    }

    private void OnLoadingComplete()
    {
        OnCreateRecipeButtonClick?.Invoke(_recipe);
        LoadingWindow.OnLoadingComplete -= OnLoadingComplete;
    }
}
