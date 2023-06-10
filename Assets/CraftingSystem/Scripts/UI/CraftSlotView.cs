using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftSlotView : MonoBehaviour
{
    [SerializeField] private Image _craftedItemIcon;
    [SerializeField] private TMP_Text _craftedItemName;
    [SerializeField] private IngridientSlotView _slotIngridientPrefab;
    [SerializeField] private Transform _ingridientsContainer;
    [SerializeField] private Button _craftedButton;

    private List<IngridientSlotView> ingridientSlots = new List<IngridientSlotView>();
    private PlayerInventoryHolder _inventoryHolder;
    private CraftRecipe _recipe;

    public event UnityAction<CraftRecipe, PlayerInventoryHolder> OnCreateRecipeButtonClick;

    public CraftRecipe Recipe => _recipe;

    private void OnEnable()
    {
        _craftedButton.onClick.AddListener(OnCreateRecipeButton);
    }

    private void OnDisable()
    {
        _craftedButton.onClick.RemoveListener(OnCreateRecipeButton);
    }

    public void Init(PlayerInventoryHolder playerInventory, CraftRecipe craftRecipe)
    {
        _recipe = craftRecipe;
        _inventoryHolder = playerInventory;

        _craftedItemIcon.sprite = craftRecipe.CraftedItem.Icon;
        _craftedItemName.text = craftRecipe.CraftedItem.DisplayName;

        foreach (var ingridient in craftRecipe.CraftingIngridients)
        {
            IngridientSlotView slotView = Instantiate(_slotIngridientPrefab, _ingridientsContainer);
            slotView.Init(playerInventory, ingridient.ItemRequired, ingridient.AmountRequured);
            ingridientSlots.Add(slotView);
        }
    }

    public void OnCreateRecipeButton()
    {
        OnCreateRecipeButtonClick?.Invoke(_recipe, _inventoryHolder);
    }

    public void UpdateRecipe()
    {
        foreach (var ingridient in ingridientSlots)
        {
            ingridient.UpdateAmount(_inventoryHolder);
        }
    }
}
