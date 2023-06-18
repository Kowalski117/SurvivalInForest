using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Crafting Item List", order = 51)]
public class RecipeItemList : ScriptableObject
{
    [SerializeField] private List<CraftRecipe> _items;

    public List<CraftRecipe> Items => _items;
}
