using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Recipe", order = 51)]
public class ItemRecipe : CraftRecipe
{
    [SerializeField] private InventoryItemData _craftedItem;

    public InventoryItemData CraftedItem => _craftedItem;
}
