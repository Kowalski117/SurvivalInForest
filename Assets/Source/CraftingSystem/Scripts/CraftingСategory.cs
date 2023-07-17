using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class Crafting�ategory : ScriptableObject
{
    [SerializeField] private string _name�ategory;
    [SerializeField] private ItemType _defoultType;
    [SerializeField] private List<ItemRecipe> _recipeItemLists;
    [SerializeField] private List<BuildingRecipe> _recipeBuildingLists;

    public string Name�ategory => _name�ategory;
    public ItemType DefoultType => _defoultType;
    public List<ItemRecipe> RecipeItemLists => _recipeItemLists;
    public List<BuildingRecipe> RecipeBuildingLists => _recipeBuildingLists;
}