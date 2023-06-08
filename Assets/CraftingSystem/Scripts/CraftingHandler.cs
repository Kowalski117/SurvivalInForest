using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private CraftingCategoryButton[] _craftingCategoryButton;
    [SerializeField] private CraftSlotView _craftSlotPrefab;

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

    public void DisplayCraftingWindow(Crafting�ategory crafting�ategory)
    {
        _crafting�ategory = crafting�ategory;

        for (int i = 0; i < _crafting�ategory.RecipeItemLists.Count; i++)
        {
            foreach (var item in _crafting�ategory.RecipeItemLists[i].Items)
            {
                CraftSlotView craftSlot = Instantiate(_craftSlotPrefab, _craftingCategoryButton[i].ContainerForSlots);
                _craftSlotViews.Add(craftSlot);
                craftSlot.Init(_inventoryHolder, item);
            }
            _craftingCategoryButton[i].ContainerForSlots.gameObject.SetActive(false);
        }
    }

    private void UpdateSlot()
    {
        foreach (var slot in _craftSlotViews)
        {
            slot.UpdateRecipe();
        }
    }
}
