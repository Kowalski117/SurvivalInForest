using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class CraftingСategory : ScriptableObject
{
    [SerializeField] private List<RecipeItemList> _recipeItemLists;

    public List<RecipeItemList> RecipeItemLists => _recipeItemLists;
}
