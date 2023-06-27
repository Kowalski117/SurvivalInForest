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

    public void Init(PlayerInventoryHolder playerInventory, BuildingRecipe craftRecipe, Crafting—ategory Òategory)
    {
        _recipe = craftRecipe;
        InventoryHolder = playerInventory;

        Crafting—ategory = Òategory;

        CraftedIcon.sprite = craftRecipe.BuildingData.Icon;
        CraftedName.text = craftRecipe.BuildingData.DisplayName;

        foreach (var ingridient in craftRecipe.CraftingIngridients)
        {
            IngridientSlotView slotView = Instantiate(SlotIngridientPrefab, IngridientsContainer);
            slotView.Init(playerInventory, ingridient.ItemRequired, ingridient.AmountRequured);
            IngridientSlots.Add(slotView);
        }
    }

    public void OnCreateRecipeButton()
    {
        OnCreateRecipeButtonClick?.Invoke(_recipe);
    }
}
