using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class Crafting�ategory : ScriptableObject
{
    [SerializeField] private string _name�ategory;
    [SerializeField] private CraftingType _defoultType;
    [SerializeField] private List<CraftRecipe> _recipes;

    public string Name�ategory => _name�ategory;
    public CraftingType DefoultType => _defoultType;
    public List<CraftRecipe> Recipes => _recipes;
}