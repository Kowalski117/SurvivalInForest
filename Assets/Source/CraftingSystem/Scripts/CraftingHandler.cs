using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PixelCrushers.QuestMachine.QuestControl))]
public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private MouseItemData _mouseItemData;
    [SerializeField] private ManualWorkbench _manualWorkbench;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private DelayHandler _loadingWindow;
    [SerializeField] private CraftingСategory[] _craftingСategories;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private Transform _craftingWindow;
    [SerializeField] private TMP_Text _nameCategory;
    [SerializeField] private CraftSlotView[] _craftSlotView;
    [SerializeField] private CraftingCategoryButton[] _craftingCategoryButtons;
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defoultColor;

    private List<CraftSlotView> _craftSlots = new List<CraftSlotView>();

    private PixelCrushers.QuestMachine.QuestControl _questControl;
    private CraftingСategory _currentCategory;
    private bool _isCraftPlayerOpen = false;

    public event Action OnInventoryUpdated;
    public event Action OnItemCrafted;
    public event Action<BuildingRecipe> OnBuildCreated;

    private void Awake()
    {
        _questControl = GetComponent<PixelCrushers.QuestMachine.QuestControl>();
    }

    private void OnEnable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindowToggled += ToggleCraftWindow;
        _buildTool.OnBuildingCompleted += UpdateSlot;
        _inventoryHolder.OnItemSlotUpdated += UpdateSlot;
        _mouseItemData.OnUpdatedSlots += UpdateSlot;

        foreach (var slot in _craftSlotView)
        {
            slot.OnCreatedRecipeButtonClick += CraftItem;
        }
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindowToggled -= ToggleCraftWindow;
        _buildTool.OnBuildingCompleted -= UpdateSlot;
        _inventoryHolder.OnItemSlotUpdated -= UpdateSlot;
        _mouseItemData.OnUpdatedSlots -= UpdateSlot;

        foreach (var slot in _craftSlotView)
        {
            slot.OnCreatedRecipeButtonClick -= CraftItem;
        }
    }

    private void Start()
    {
        foreach (var item in _craftSlotView)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var craftingCategory in _craftingСategories)
        {
            CreateCraftSlots(craftingCategory);
        }

        foreach (var button in _craftingCategoryButtons)
        {
            button.ToggleButton(false);
        }

        SetCategory(_manualWorkbench.CraftingСategory);
    }

    private void CreateCraftSlots(CraftingСategory craftingCategory)
    {
        foreach(var recipe in craftingCategory.Recipes)
        {
            foreach (var slot in _craftSlotView)
            {
                if (slot.IsEmpty)
                {
                    slot.gameObject.SetActive(true);
                    slot.Init(_inventoryHolder, recipe, craftingCategory);
                    _craftSlots.Add(slot);
                    break;
                }
            }
        }

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        foreach (var slot in _craftSlots)
        {
            slot.UpdateRecipe();
        }

        OnInventoryUpdated?.Invoke();
    }

    public void SwitchCraftingCategory(CraftingType itemType)
    {
        foreach (var button in _craftingCategoryButtons)
        {
            if (button.ItemType == itemType)
                button.ToggleButton(true);
            else
                button.ToggleButton(false);
        }

        foreach (var slot in _craftSlots)
        {
            if (slot.Recipe.CraftingType == itemType && _currentCategory == slot.Category)
                slot.gameObject.SetActive(true);
            else
                slot.gameObject.SetActive(false);
        }
    }

    public void SetCategory(CraftingСategory craftingCategory)
    {
        _nameCategory.text = craftingCategory.NameCategory;
        _currentCategory = craftingCategory;

        foreach (var button in _craftingCategoryButtons)
        {
            button.gameObject.SetActive(false);
        }

        List<CraftingType> activeCraftingTypes = new List<CraftingType>();

        foreach (var recipe in _currentCategory.Recipes)
        {
            activeCraftingTypes.Add(recipe.CraftingType);
        }

        foreach (var slot in _craftSlots)
        {
            bool slotIsRelevant = false;

            if (_currentCategory == slot.Category && activeCraftingTypes.Contains(slot.Recipe.CraftingType))
            {
                slot.gameObject.SetActive(true);
                slotIsRelevant = true;
            }

            if (!slotIsRelevant)
                slot.gameObject.SetActive(false);
        }

        foreach (var button in _craftingCategoryButtons)
        {
            if (activeCraftingTypes.Contains(button.ItemType))
                button.gameObject.SetActive(true);
        }

        SwitchCraftingCategory(craftingCategory.DefaultType);
    }

    private void ToggleCraftWindow(CraftingСategory craftingCategory)
    {
        _isCraftPlayerOpen = !_isCraftPlayerOpen;

        if (_isCraftPlayerOpen)
            UpdateSlot();
    }

    private void CraftItem(CraftRecipe craftRecipe)
    {
        OnItemCrafted?.Invoke();

        if (_inventoryHolder.CheckIfCanCraft(craftRecipe))
        {
            if (craftRecipe is ItemRecipe itemRecipe)
            {
                if (itemRecipe.CraftedItem.Type == ItemType.Food)
                    _loadingWindow.ShowLoadingWindow(itemRecipe.DelayCraft, itemRecipe.CraftingTime, itemRecipe.CraftedItem.DisplayName, ActionType.Preparing, () => FinishComplete(itemRecipe));
                else
                    _loadingWindow.ShowLoadingWindow(itemRecipe.DelayCraft, itemRecipe.CraftingTime, itemRecipe.CraftedItem.DisplayName, ActionType.CraftItem, () => FinishComplete(itemRecipe));
            }
            else if(craftRecipe is BuildingRecipe buildingRecipe)
                OnBuildCreated?.Invoke(buildingRecipe);

            UpdateSlot();
        }
    }

    private void FinishComplete(ItemRecipe itemRecipe)
    {
        if (_inventoryHolder.CheckIfCanCraft(itemRecipe))
        {
            foreach (var ingredient in itemRecipe.CraftingIngridients)
            {
                _inventoryHolder.RemoveItem(ingredient.ItemRequired, ingredient.AmountRequured);
            }

            _inventoryHolder.AddItem(itemRecipe.CraftedItem, itemRecipe.CraftingAmount, itemRecipe.CraftedItem.Durability);
        }

        _questControl.SendToMessageSystem(MessageConstants.Craft + itemRecipe.CraftedItem.Name);
    }
}
