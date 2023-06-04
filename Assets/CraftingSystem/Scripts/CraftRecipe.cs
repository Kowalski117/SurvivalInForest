using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Craft Recipe", order = 51)]
public class CraftRecipe : ScriptableObject
{
    [SerializeField] private List<CraftingIngridient> _craftingIngridients;
    [SerializeField] private InventoryItemData _craftedItem;
    [SerializeField] private int _craftingAmout = 1;

    public List<CraftingIngridient> CraftingIngridients => _craftingIngridients;
    public InventoryItemData CraftedItem => _craftedItem;
    public int CraftingAmount => _craftingAmout;
}

[System.Serializable]
public struct CraftingIngridient
{
    [SerializeField] private InventoryItemData _itemRequired;
    [SerializeField] private int _amountRequured;

    public InventoryItemData ItemRequired => _itemRequired;
    public int AmountRequured => _amountRequured;

    public CraftingIngridient(InventoryItemData itemRequired, int amountRequured)
    {
        _itemRequired = itemRequired;
        _amountRequured = amountRequured;
    }
}
