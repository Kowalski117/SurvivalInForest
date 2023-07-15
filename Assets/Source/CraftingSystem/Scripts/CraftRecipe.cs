using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe : ScriptableObject
{
    [SerializeField] private List<CraftingIngridient> _craftingIngridients;

    [SerializeField] private int _craftingAmout = 1;
    [SerializeField] private float _delayCraft = 3;
    [SerializeField] private float _craftingTime = 1;

    public List<CraftingIngridient> CraftingIngridients => _craftingIngridients;
    public int CraftingAmount => _craftingAmout;
    public float DelayCraft => _delayCraft;
    public float CraftingTime => _craftingTime;
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
