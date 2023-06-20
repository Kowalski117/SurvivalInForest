using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class Crafting—ategory : ScriptableObject
{
    [SerializeField] private List<RecipeItemList> _recipeItemLists;
    [SerializeField] private List<RecipeBuildingList> _recipeBuildingLists;

    public List<RecipeItemList> RecipeItemLists => _recipeItemLists;
    public List<RecipeBuildingList> RecipeBuildingLists => _recipeBuildingLists;
}

[System.Serializable]
public class RecipeItemList
{
    [SerializeField] private List<ItemRecipe> _items;

    public List<ItemRecipe> Items => _items;
}

[System.Serializable]
public class RecipeBuildingList
{
    [SerializeField] private List<BuildingRecipe> _items;

    public List<BuildingRecipe> Items => _items;
}