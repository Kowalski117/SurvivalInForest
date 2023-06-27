using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ManualWorkbench _manualWorkbench;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private BuildTool _buildTool;

    [SerializeField] private CraftingÑategory[] _craftingÑategories;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private Transform _craftingWindow;
    [SerializeField] private ItemType _defoultType = ItemType.Weapon;

    [SerializeField] private CraftItemSlotView _craftItemSlotPrefab;
    [SerializeField] private CraftBuildSlotView _craftBuildingSlotPrefab;

    private List<CraftItemSlotView> _craftItemSlots = new List<CraftItemSlotView>();
    private List<CraftBuildSlotView> _craftBuildSlots = new List<CraftBuildSlotView>();

    private CraftingÑategory _currentCategory;
    private bool _isCraftPlayerOpen = false;
    public Transform CraftingWindow => _craftingWindow;

    private void OnEnable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow += DisplayCraftPlayerWindow;

        _buildTool.OnCompletedBuild += UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate += UpdateSlot;
        MouseItemData.OnUpdatedSlots += UpdateSlot;
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow -= DisplayCraftPlayerWindow;

        _buildTool.OnCompletedBuild -= UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate -= UpdateSlot;
        MouseItemData.OnUpdatedSlots -= UpdateSlot;
    }

    private void Start()
    {
        foreach (var craftingCategory in _craftingÑategories)
        {
            CreateCraftSlots(craftingCategory);
        }

        DisplayCraftWindow(_manualWorkbench.CraftingÑategory);
    }

    private void CreateCraftSlots(CraftingÑategory craftingCategory)
    {
        foreach (var recipeList in craftingCategory.RecipeItemLists)
        {
            foreach (var recipe in recipeList.Items)
            {
                CraftItemSlotView craftSlot = Instantiate(_craftItemSlotPrefab, _containerForSlots);
                _craftItemSlots.Add(craftSlot);
                craftSlot.Init(_inventoryHolder, recipe, craftingCategory);
            }
        }

        foreach (var recipeList in craftingCategory.RecipeBuildingLists)
        {
            foreach (var recipe in recipeList.Items)
            {
                CraftBuildSlotView craftSlot = Instantiate(_craftBuildingSlotPrefab, _containerForSlots);
                _craftBuildSlots.Add(craftSlot);
                craftSlot.Init(_inventoryHolder, recipe, craftingCategory);
            }
        }
    }

    public void UpdateSlot()
    {
        foreach (var slot in _craftItemSlots)
        {
            slot.UpdateRecipe();
        }

        foreach (var slot in _craftBuildSlots)
        {
            slot.UpdateRecipe();
        }
    }

    public void SwitchCraftingCategory(ItemType itemType)
    {
        foreach (var slot in _craftItemSlots)
        {
            if (slot.Recipe.CraftedItem.Type == itemType && _currentCategory == slot.Category)
            {
                slot.OpenForCrafting();
            }
            else
            {
                slot.CloseForCrafting();
            }
        }

        foreach (var slot in _craftBuildSlots)
        {
            if (slot.Recipe.BuildingData.Type == itemType && _currentCategory == slot.Category)
            {
                slot.OpenForCrafting();
            }
            else
            {
                slot.CloseForCrafting();
            }
        }
    }

    public void DisplayCraftWindow(CraftingÑategory craftingCategory)
    {
        _currentCategory = craftingCategory;

        foreach (var recipeList in _currentCategory.RecipeItemLists)
        {
            foreach (var recipe in recipeList.Items)
            {
                foreach (var slot in _craftItemSlots)
                {
                    if (slot.Recipe == recipe && craftingCategory == slot.Category)
                    {
                        slot.OpenForCrafting();
                    }
                    else
                    {
                        slot.CloseForCrafting();
                    }
                }

                foreach (var slot in _craftBuildSlots)
                {
                    if (slot.Recipe == recipe && craftingCategory == slot.Category)
                    {
                        slot.OpenForCrafting();
                    }
                    else
                    {
                        slot.CloseForCrafting();
                    }
                }
            }   
        }

        SwitchCraftingCategory(_defoultType);
    }

    private void DisplayCraftPlayerWindow(CraftingÑategory craftingCategory)
    {
        _isCraftPlayerOpen = !_isCraftPlayerOpen;

        if (_isCraftPlayerOpen)
        {
            _craftingWindow.gameObject.SetActive(true);
            UpdateSlot();
        }
        else
        {
            _craftingWindow.gameObject.SetActive(false);
        }
    }
}
