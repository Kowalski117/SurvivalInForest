using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class CraftingÑategory : ScriptableObject
{
    [SerializeField] private string _nameÑategory;
    [SerializeField] private ItemType _defoultType;
    [SerializeField] private List<ItemRecipe> _recipeItemLists;
    [SerializeField] private List<BuildingRecipe> _recipeBuildingLists;

    public string NameÑategory => _nameÑategory;
    public ItemType DefoultType => _defoultType;
    public List<ItemRecipe> RecipeItemLists => _recipeItemLists;
    public List<BuildingRecipe> RecipeBuildingLists => _recipeBuildingLists;
}