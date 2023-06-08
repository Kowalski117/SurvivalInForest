using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private CraftingCategoryButton[] _craftingCategoryButton;
    [SerializeField] private CraftSlotView _craftSlotPrefab;

    private CraftingÑategory _craftingÑategory;
    private List<CraftSlotView> _craftSlotViews = new List<CraftSlotView>();

    private void OnEnable()
    {
        CraftSlot.OnCraftSlotUpdate += UpdateSlot;
    }

    private void OnDisable()
    {
        CraftSlot.OnCraftSlotUpdate -= UpdateSlot;
    }

    public void DisplayCraftingWindow(CraftingÑategory craftingÑategory)
    {
        _craftingÑategory = craftingÑategory;

        for (int i = 0; i < _craftingÑategory.RecipeItemLists.Count; i++)
        {
            foreach (var item in _craftingÑategory.RecipeItemLists[i].Items)
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
