using System;
using PixelCrushers.QuestMachine;

public class CraftItemSlotView : CraftSlotView
{
    private ItemRecipe _recipe;
    private QuestControl _questControl;

    public event Action<ItemRecipe, PlayerInventoryHolder> OnCreatedRecipeButtonClick;

    public ItemRecipe Recipe => _recipe;

    private void OnEnable()
    {
        CraftedButton.onClick.AddListener(OnCreateRecipeButton);
    }

    private void OnDisable()
    {
        CraftedButton.onClick.RemoveListener(OnCreateRecipeButton);
    }

    public void Init(PlayerInventoryHolder playerInventory, ItemRecipe craftRecipe, CraftingСategory сategory, DelayHandler loadingWindow, QuestControl questControl)
    {
        CraftedTime = DateTime.MinValue;
        _recipe = craftRecipe;
        InventoryHolder = playerInventory;
        LoadingWindow = loadingWindow;
        CraftingСategory = сategory;
        CraftedIcon.sprite = craftRecipe.CraftedItem.Icon;
        CraftedName.text = craftRecipe.CraftedItem.DisplayName;
        CraftedTime = CraftedTime + TimeSpan.FromHours(craftRecipe.CraftingTime);
        CraftedTimeText.text = CraftedTime.ToString(GameConstants.HHmm);
        _questControl = questControl;

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
            if(_recipe.CraftedItem.Type == ItemType.Food)
                LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.CraftedItem.DisplayName, ActionType.Preparing, () => FinishComplete());
            else
                LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.CraftedItem.DisplayName, ActionType.CraftItem, () => FinishComplete());
        }
    }

    private void FinishComplete()
    {
        OnCreatedRecipeButtonClick?.Invoke(_recipe, InventoryHolder);
        _questControl.SendToMessageSystem(MessageConstants.Craft + _recipe.CraftedItem.Name);
    }

    protected override void UpdateLanguage()
    {
        CraftedName.text = _recipe.CraftedItem.DisplayName;
    }
}
