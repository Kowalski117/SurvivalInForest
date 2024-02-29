using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Tool", order = 51)]
public class ToolItemData : InventoryItemData
{
    [SerializeField] private ToolType _toolType;
    [SerializeField] private ResourseType _resourseType;
    [SerializeField] private float _damageResources;
    [SerializeField] private float _damageLiving;
    [SerializeField] private float _speed;

    public ToolType ToolType => _toolType;
    public ResourseType ResourseType => _resourseType;
    public float DamageResources => _damageResources;
    public float DamageLiving => _damageLiving;
    public float Speed => _speed;
}

public enum ToolType
{
    Axe,
    Pickaxe,
    Arm,
    FishingRod,
    Torch,
    Shovel,
}

public enum ResourseType
{
    Stone,
    Coal,
    Iron,
    All,
    None,
}

#if UNITY_EDITOR
[CustomEditor(typeof(ToolItemData))]
public class ToolItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (ToolItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif
