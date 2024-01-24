using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Recipe", order = 51)]
public class ItemRecipe : CraftRecipe
{
    [SerializeField] private InventoryItemData _craftedItem;

    public InventoryItemData CraftedItem => _craftedItem;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ItemRecipe))]
public class ItemRecipeEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (ItemRecipe)target;

        if (data == null || data.CraftedItem.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.CraftedItem.Icon.texture, texture);
        return texture;
    }
}
#endif