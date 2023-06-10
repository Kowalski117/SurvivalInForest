using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCategoryButton : MonoBehaviour
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Crafting—ategory _crafting—ategory;

    private List<CraftSlotView> _craftSlotViews = new List<CraftSlotView>();
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OpenCategory);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OpenCategory);
    }

    private void OpenCategory()
    {
        _craftSlotViews = _craftingHandler.GetItemsByType(_itemType);
    }
}
