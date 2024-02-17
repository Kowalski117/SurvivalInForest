using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class Crafting—ategory : ScriptableObject
{
    [SerializeField] private string _name—ategory;
    [SerializeField] private CraftingType _defoultType;
    [SerializeField] private List<CraftRecipe> _recipes;

    public string NameCategory => _name—ategory;
    public CraftingType DefaultType => _defoultType;
    public List<CraftRecipe> Recipes => _recipes;
}