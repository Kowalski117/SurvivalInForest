using System;
using UnityEngine.Events;
using UnityEngine;
using PixelCrushers.QuestMachine;
using PixelCrushers.DialogueSystem;

public class CraftItemSlotView : CraftSlotView
{
    private ItemRecipe _recipe;
    private QuestControl _questControl;

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

    public void Init(PlayerInventoryHolder playerInventory, ItemRecipe craftRecipe, CraftingСategory сategory, DelayWindow loadingWindow, QuestControl questControl)
    {
        CraftedTime = DateTime.MinValue;
        _recipe = craftRecipe;
        InventoryHolder = playerInventory;
        LoadingWindow = loadingWindow;
        CraftingСategory = сategory;
        CraftedIcon.sprite = craftRecipe.CraftedItem.Icon;
        CraftedName.text = craftRecipe.CraftedItem.DisplayName;
        CraftedTime = CraftedTime + TimeSpan.FromHours(craftRecipe.CraftingTime);
        CraftedTimeText.text = CraftedTime.ToString("HH:mm");
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
                LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.CraftedItem.DisplayName, ActionType.Preparing);
            else
                LoadingWindow.ShowLoadingWindow(_recipe.DelayCraft, _recipe.CraftingTime, _recipe.CraftedItem.DisplayName, ActionType.CraftItem);

            LoadingWindow.OnLoadingComplete += OnLoadingComplete;
        }
    }

    private void OnLoadingComplete()
    {
        OnCreateRecipeButtonClick?.Invoke(_recipe, InventoryHolder);
        _questControl.SendToMessageSystem(MessageConstants.Craft + _recipe.CraftedItem.Name);
        LoadingWindow.OnLoadingComplete -= OnLoadingComplete;
    }

    protected override void UpdateLanguage()
    {
        CraftedName.text = _recipe.CraftedItem.DisplayName;
    }
}
