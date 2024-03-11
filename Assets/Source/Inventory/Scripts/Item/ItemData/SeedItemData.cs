using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Seed", order = 51)]
public class SeedItemData : InventoryItemData
{
    [SerializeField] private float _growthTime;
    [SerializeField] private BeingLiftedObject _objectPickUp;
    [SerializeField] private ObjectItemsData[] _lootItems;

    public float GrowthTime => _growthTime;
    public BeingLiftedObject ObjectPickUp => _objectPickUp;
    public ObjectItemsData[] LootItems => _lootItems;
}

#if UNITY_EDITOR
[CustomEditor(typeof(FoodItemData))]
public class SeedItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (SeedItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif