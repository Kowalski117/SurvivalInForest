using UnityEngine.Events;

public class CraftItemSlotView : CraftSlotView
{
    private ItemRecipe _recipe;

    public event UnityAction<ItemRecipe, PlayerInventoryHolder> OnCreateRecipeButtonClick;

    public ItemRecipe Recipe => _recipe;

    private void OnEnable()
    {
        CraftedButton.onClick.AddListener(OnCreateRecipeButton);
    }

    private void OnDisable()
    {
        CraftedButton.onClick.RemoveListener(OnCreateRecipeButton);
    }

    public void Init(PlayerInventoryHolder playerInventory, ItemRecipe craftRecipe, Crafting—ategory Òategory, LoadingWindow loadingWindow)
    {
        _recipe = craftRecipe;
        InventoryHolder = playerInventory;
        LoadingWindow = loadingWindow;

        Crafting—ategory = Òategory;

        CraftedIcon.sprite = craftRecipe.CraftedItem.Icon;
        CraftedName.text = craftRecipe.CraftedItem.DisplayName;

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
            LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.CraftedItem.DisplayName);
            LoadingWindow.OnLoadingComplete += OnLoadingComplete;
        }
    }

    private void OnLoadingComplete()
    {
        OnCreateRecipeButtonClick?.Invoke(_recipe, InventoryHolder);
    }
}
