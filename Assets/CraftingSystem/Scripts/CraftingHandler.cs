using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private CraftSlotView _craftSlotPrefab;

    private CraftBench _craftBench;
    private List<CraftSlotView> _craftSlotViews = new List<CraftSlotView>();

    private void OnEnable()
    {
        CraftSlot.OnCraftSlotUpdate += UpdateSlot;
    }

    private void OnDisable()
    {
        CraftSlot.OnCraftSlotUpdate -= UpdateSlot;
    }

    public void DisplayCraftingWindow(CraftBench craftBench)
    {
        _craftBench = craftBench;

        foreach (Transform child in _containerForSlot)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in _craftBench.RecipeItemList.Items)
        {
            CraftSlotView craftSlot = Instantiate(_craftSlotPrefab, _containerForSlot);
            _craftSlotViews.Add(craftSlot);
            craftSlot.Init(_inventoryHolder, item);
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
