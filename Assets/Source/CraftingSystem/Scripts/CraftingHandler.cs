using PixelCrushers.QuestMachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ManualWorkbench _manualWorkbench;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private DelayWindow _loadingWindow;
    [SerializeField] private CraftingСategory[] _craftingСategories;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private Transform _craftingWindow;
    [SerializeField] private TMP_Text _nameCategory;
    [SerializeField] private CraftItemSlotView _craftItemSlotPrefab;
    [SerializeField] private CraftBuildSlotView _craftBuildSlotPrefab;
    [SerializeField] private CraftingCategoryButton[] _craftingCategoryButtons;
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defoultColor;

    private List<CraftSlotView> _craftSlots = new List<CraftSlotView>();

    private QuestControl _questControl;
    private CraftingСategory _currentCategory;
    private bool _isCraftPlayerOpen = false;

    public event UnityAction OnUpdateSlotInventory;
    public event UnityAction OnItemCrafted;

    public Transform CraftingWindow => _craftingWindow;
    public CraftingСategory CurrentCraftingСategory => _currentCategory;

    private void Awake()
    {
        _questControl = GetComponent<QuestControl>();
        //_craftingWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow += DisplayCraftPlayerWindow;
        _buildTool.OnCompletedBuilding += UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate += UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate += CraftItem;
        CrafBuildSlot.OnCraftSlotUpdate += UpdateSlot;

        _inventoryHolder.OnUpdateItemSlot += UpdateSlot;
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow -= DisplayCraftPlayerWindow;
        _buildTool.OnCompletedBuilding -= UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate -= UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate -= CraftItem;
        CrafBuildSlot.OnCraftSlotUpdate -= UpdateSlot;
        
        _inventoryHolder.OnUpdateItemSlot -= UpdateSlot;
    }

    private void Start()
    {
        foreach (var craftingCategory in _craftingСategories)
        {
            CreateCraftSlots(craftingCategory);
        }

        foreach (var button in _craftingCategoryButtons)
        {
            button.ToggleButton(false);
        }

        DisplayCraftWindow(_manualWorkbench.CraftingСategory);
    }

    private void CreateCraftSlots(CraftingСategory craftingCategory)
    {
        foreach(var recipe in craftingCategory.Recipes)
        {
            if(recipe is ItemRecipe itemRecipe)
            {
                CraftSlotView currentRecipe = Instantiate(_craftItemSlotPrefab, _containerForSlots);

                if (currentRecipe is CraftItemSlotView itemSlotView)
                    itemSlotView.Init(_inventoryHolder, itemRecipe, craftingCategory, _loadingWindow, _questControl);

                _craftSlots.Add(currentRecipe);
            }
            else if (recipe is BuildingRecipe buildRecipe)
            {
                CraftSlotView currentRecipe = Instantiate(_craftBuildSlotPrefab, _containerForSlots);

                if (currentRecipe is CraftBuildSlotView buildSlotView)
                    buildSlotView.Init(_inventoryHolder, buildRecipe, craftingCategory, _loadingWindow);

                _craftSlots.Add(currentRecipe);
            }
        }
    }

    public void UpdateSlot()
    {
        foreach (var slot in _craftSlots)
        {
            slot.UpdateRecipe();
        }

        OnUpdateSlotInventory?.Invoke();
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
            if (slot is CraftItemSlotView itemSlotView && itemSlotView.Recipe.CraftingType == itemType && _currentCategory == slot.Category)
                slot.OpenForCrafting();
            else if (slot is CraftBuildSlotView buildSlotView && buildSlotView.Recipe.CraftingType == itemType && _currentCategory == slot.Category)
                slot.OpenForCrafting();
            else
                slot.CloseForCrafting();
        }
    }

    public void DisplayCraftWindow(CraftingСategory craftingCategory)
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

            if (slot is CraftItemSlotView itemSlotView && _currentCategory == slot.Category &&
                activeCraftingTypes.Contains(itemSlotView.Recipe.CraftingType))
            {
                itemSlotView.OpenForCrafting();
                slotIsRelevant = true;
            }
            else if (slot is CraftBuildSlotView buildSlotView && _currentCategory == slot.Category &&
                     activeCraftingTypes.Contains(buildSlotView.Recipe.CraftingType))
            {
                buildSlotView.OpenForCrafting();
                slotIsRelevant = true;
            }

            if (!slotIsRelevant)
            {
                slot.CloseForCrafting();
            }
        }

        foreach (var button in _craftingCategoryButtons)
        {
            if (activeCraftingTypes.Contains(button.ItemType))
            {
                button.gameObject.SetActive(true);
            }
        }

        SwitchCraftingCategory(craftingCategory.DefaultType);
    }

    private void DisplayCraftPlayerWindow(CraftingСategory craftingCategory)
    {
        _isCraftPlayerOpen = !_isCraftPlayerOpen;

        if (_isCraftPlayerOpen)
        {
            //_craftingWindow.gameObject.SetActive(true);
            UpdateSlot();
        }
        else
        {
            //_craftingWindow.gameObject.SetActive(false);
        }
    }

    private void CraftItem()
    {
        OnItemCrafted?.Invoke();
    }
}
