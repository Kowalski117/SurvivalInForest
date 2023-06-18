using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private ItemType _defoultType = ItemType.Weapon;
    [SerializeField] private CraftSlotView _craftSlotPrefab;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private CraftingÑategory[] _craftingÑategories;
    [SerializeField] private ManualWorkbench _manualWorkbench;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private Transform _craftingWindow;

    private List<CraftSlotView> _craftSlotViews = new List<CraftSlotView>();
    private CraftingÑategory _currentCategory;
    public Transform CraftingWindow => _craftingWindow;

    private void OnEnable()
    {
        CraftSlot.OnCraftSlotUpdate += UpdateSlot;
        MouseItemData.OnUpdatedSlots += UpdateSlot;
    }

    private void OnDisable()
    {
        CraftSlot.OnCraftSlotUpdate -= UpdateSlot;
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
                CraftSlotView craftSlot = Instantiate(_craftSlotPrefab, _containerForSlots);
                _craftSlotViews.Add(craftSlot);
                craftSlot.Init(_inventoryHolder, recipe, craftingCategory);
                craftSlot.CloseForCrafting();
            }
        }
    }

    public void UpdateSlot()
    {
        foreach (var slot in _craftSlotViews)
        {
            slot.UpdateRecipe();
        }
    }

    public void SwitchCraftingCategory(ItemType itemType)
    {
        foreach (var slot in _craftSlotViews)
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
    }

    public void DisplayCraftWindow(CraftingÑategory craftingCategory)
    {
        _currentCategory = craftingCategory;

        foreach (var recipeList in _currentCategory.RecipeItemLists)
        {
            foreach (var recipe in recipeList.Items)
            {
                foreach (var slot in _craftSlotViews)
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
}
