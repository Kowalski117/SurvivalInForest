using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class CraftingÑategory : ScriptableObject
{
    [SerializeField] private string _nameÑategory;
    [SerializeField] private CraftingType _defoultType;
    [SerializeField] private List<CraftRecipe> _recipes;

    public string NameÑategory => _nameÑategory;
    public CraftingType DefoultType => _defoultType;
    public List<CraftRecipe> Recipes => _recipes;
}