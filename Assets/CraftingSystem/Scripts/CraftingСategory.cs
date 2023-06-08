using System.Collections.Generic;
using UnityEngine;

public class CraftingСategory : MonoBehaviour
{
    [SerializeField] private List<RecipeItemList> _recipeItemLists;

    public List<RecipeItemList> RecipeItemLists => _recipeItemLists;
}
