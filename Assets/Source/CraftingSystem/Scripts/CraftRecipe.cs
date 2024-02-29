using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe : ScriptableObject
{
    [SerializeField] private CraftingType _craftingType;
    [SerializeField] private List<CraftingIngridient> _craftingIngridients;
    [SerializeField] private int _craftingAmout = 1;
    [SerializeField] private float _delayCraft = 3;
    [SerializeField] private float _craftingTime = 1;

    public CraftingType CraftingType => _craftingType;
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
}

public enum CraftingType
{
    Item,
    Tool,
    Weapon,
    Clothing,
    Food,
    Decor,
    InteractBuilding,
}
