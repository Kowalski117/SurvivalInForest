using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/FishingRod", order = 51)]
public class FishingRodItemData : ToolItemData
{
    [SerializeField] private Vector2 _randomTime;
    [SerializeField] private FishingRodExtraction[] _extractions;

    public Vector2 RandomTime => _randomTime;
    public FishingRodExtraction[] Extractions => _extractions;
}

[System.Serializable]
public class FishingRodExtraction
{
    [SerializeField] private InventoryItemData _inventoryItemData;
    [SerializeField] private float _chance;

    public InventoryItemData InventoryItemData => _inventoryItemData;
    public float Chance => _chance;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ToolItemData))]
public class FishingRodToolItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (FishingRodItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif
