using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Category lists items", order = 51)]
public class CraftingСategory : ScriptableObject
{
    [SerializeField] private CraftingType _defoultType;
    [SerializeField] private List<CraftRecipe> _recipes;
    [SerializeField] private string _displayName;
    [SerializeField] private TextTable _textTableDisplayName;
    
    public string NameCategory => _textTableDisplayName.GetFieldTextForLanguage(_displayName,Localization.language);
    public CraftingType DefaultType => _defoultType;
    public List<CraftRecipe> Recipes => _recipes;
}