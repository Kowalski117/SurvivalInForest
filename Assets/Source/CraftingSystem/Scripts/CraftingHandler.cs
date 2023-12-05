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
    [SerializeField] private Crafting�ategory[] _crafting�ategories;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private Transform _craftingWindow;
    [SerializeField] private TMP_Text _nameCategory;
    [SerializeField] private CraftItemSlotView _craftItemSlotPrefab;
    [SerializeField] private CraftBuildSlotView _craftBuildingSlotPrefab;
    [SerializeField] private CraftingCategoryButton[] _craftingCategoryButtons;
    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _defoultColor;

    private List<CraftItemSlotView> _craftItemSlots = new List<CraftItemSlotView>();
    private List<CraftBuildSlotView> _craftBuildSlots = new List<CraftBuildSlotView>();

    private QuestControl _questControl;
    private Crafting�ategory _currentCategory;
    private bool _isCraftPlayerOpen = false;

    public event UnityAction OnUpdateSlotInventory;

    public Transform CraftingWindow => _craftingWindow;
    public Crafting�ategory CurrentCrafting�ategory => _currentCategory;

    private void Awake()
    {
        _questControl = GetComponent<QuestControl>();
        _craftingWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow += DisplayCraftPlayerWindow;
        _buildTool.OnCompletedBuild += UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate += UpdateSlot;
        CrafBuildSlot.OnCraftSlotUpdate += UpdateSlot;

        _inventoryHolder.OnUpdateItemSlot += UpdateSlot;
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.OnCraftPlayerWindow -= DisplayCraftPlayerWindow;
        _buildTool.OnCompletedBuild -= UpdateSlot;
        CraftItemSlot.OnCraftSlotUpdate -= UpdateSlot;
        CrafBuildSlot.OnCraftSlotUpdate -= UpdateSlot;
        
        _inventoryHolder.OnUpdateItemSlot -= UpdateSlot;
    }

    private void Start()
    {
        foreach (var craftingCategory in _crafting�ategories)
        {
            CreateCraftSlots(craftingCategory);
        }

        foreach (var button in _craftingCategoryButtons)
        {
            button.ToggleButton(false);
        }

        DisplayCraftWindow(_manualWorkbench.Crafting�ategory);
    }

    private void CreateCraftSlots(Crafting�ategory craftingCategory)
    {
        foreach (var recipe in craftingCategory.RecipeItemLists)
        {
            CraftItemSlotView craftSlot = Instantiate(_craftItemSlotPrefab, _containerForSlots);
            _craftItemSlots.Add(craftSlot);
            craftSlot.Init(_inventoryHolder, recipe, craftingCategory, _loadingWindow, _questControl);
        }

        foreach (var recipe in craftingCategory.RecipeBuildingLists)
        {
            CraftBuildSlotView craftSlot = Instantiate(_craftBuildingSlotPrefab, _containerForSlots);
            _craftBuildSlots.Add(craftSlot);
            craftSlot.Init(_inventoryHolder, recipe, craftingCategory, _loadingWindow);
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

        OnUpdateSlotInventory?.Invoke();
    }

    public void SwitchCraftingCategory(ItemType itemType)
    {
        foreach (var button in _craftingCategoryButtons)
        {
            if (button.ItemType == itemType)
                button.ToggleButton(true);
            else
                button.ToggleButton(false);
        }

        foreach (var slot in _craftItemSlots)
        {
            if (slot.Recipe.CraftedItem.Type == itemType && _currentCategory == slot.Category)            
                slot.OpenForCrafting();           
            else            
                slot.CloseForCrafting();          
        }

        foreach (var slot in _craftBuildSlots)
        {
            if (slot.Recipe.BuildingData.Type == itemType && _currentCategory == slot.Category)           
                slot.OpenForCrafting();           
            else            
                slot.CloseForCrafting();          
        }
    }

    public void DisplayCraftWindow(Crafting�ategory craftingCategory)
    {
        _nameCategory.text = craftingCategory.Name�ategory;
        _currentCategory = craftingCategory;

        foreach (var button in _craftingCategoryButtons)
        {
            button.gameObject.SetActive(false);
        }

        foreach (var button in _craftingCategoryButtons)
        {
            foreach (var recipe in _currentCategory.RecipeItemLists)
            {
                foreach (var slot in _craftItemSlots)
                {
                    if (slot.Recipe == recipe && craftingCategory == slot.Category)
                    {
                        slot.OpenForCrafting();

                        if (button.ItemType == slot.Recipe.CraftedItem.Type)
                        {
                            button.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        slot.CloseForCrafting();
                    }
                }
            }

            foreach (var recipe in _currentCategory.RecipeBuildingLists)
            {
                foreach (var slot in _craftBuildSlots)
                {
                    if (slot.Recipe == recipe && craftingCategory == slot.Category)
                    {
                        slot.OpenForCrafting();

                        if (button.ItemType == slot.Recipe.BuildingData.Type)
                        {
                            button.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        slot.CloseForCrafting();
                    }
                }
            }
        }
        SwitchCraftingCategory(craftingCategory.DefoultType);
    }

    private void DisplayCraftPlayerWindow(Crafting�ategory craftingCategory)
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
