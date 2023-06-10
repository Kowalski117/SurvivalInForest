using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private CraftSlotView _craftSlotPrefab;
    [SerializeField] private Transform _containerForSlots;
    [SerializeField] private Crafting�ategory[] _crafting�ategories;

    private Crafting�ategory _crafting�ategory;
    private List<CraftSlotView> _craftSlotViews = new List<CraftSlotView>();

    private void OnEnable()
    {
        CraftSlot.OnCraftSlotUpdate += UpdateSlot;
    }

    private void OnDisable()
    {
        CraftSlot.OnCraftSlotUpdate -= UpdateSlot;
    }

    private void Start()
    {
        foreach (var crafting�ategory in _crafting�ategories) 
        {
            CreateCraftSlot(crafting�ategory);
        }
    }

    public void DisplayCraftingWindow(Crafting�ategory crafting�ategory)
    {
        _crafting�ategory = crafting�ategory;

        for (int i = 0; i < _crafting�ategory.RecipeItemLists.Count; i++)
        {
            foreach (var item in _crafting�ategory.RecipeItemLists[i].Items)
            {
                EnableSlot(item);
            }
        }
    }

    public List<CraftSlotView> GetItemsByType(ItemType itemType)
    {
        List<CraftSlotView> items = new List<CraftSlotView>();

        foreach (var item in _craftSlotViews)
        {
            if (item.Recipe.CraftedItem.Type == itemType)
            {
                items.Add(item);
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }

        return items;
    }

    private void CreateCraftSlot(Crafting�ategory crafting�ategory)
    {
        for (int i = 0; i < crafting�ategory.RecipeItemLists.Count; i++)
        {
            foreach (var item in crafting�ategory.RecipeItemLists[i].Items)
            {
                CraftSlotView craftSlot = Instantiate(_craftSlotPrefab, _containerForSlots);
                _craftSlotViews.Add(craftSlot);
                craftSlot.Init(_inventoryHolder, item);
            }
        }
    }

    private void UpdateSlot()
    {
        foreach (var slot in _craftSlotViews)
        {
            slot.UpdateRecipe();
        }
    }

    private void EnableSlot(CraftRecipe craftRecipe)
    {
        foreach (var item in _craftSlotViews)
        {
            if(item.Recipe == craftRecipe)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
