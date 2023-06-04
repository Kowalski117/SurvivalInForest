using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotView : MonoBehaviour
{
    [SerializeField] private Image _craftedItemIcon;
    [SerializeField] private TMP_Text _craftedItemName;
    [SerializeField] private IngridientSlotView _slotIngridientPrefab;
    [SerializeField] private Transform _ingridientsContainer;
    [SerializeField] private Button _craftedButton;

    private CraftRecipe _recipe;

    public void Init(CraftRecipe craftRecipe)
    {
        _recipe = craftRecipe;

        _craftedItemIcon.sprite = craftRecipe.CraftedItem.Icon;
        _craftedItemName.text = craftRecipe.CraftedItem.DisplayName;

        foreach (var ingridient in craftRecipe.CraftingIngridients)
        {
            IngridientSlotView slotView = Instantiate(_slotIngridientPrefab, _ingridientsContainer);
            slotView.Init(ingridient.ItemRequired, ingridient.AmountRequured);
        }
    }
}
