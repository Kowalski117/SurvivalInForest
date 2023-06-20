using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Building System/ Build Data", order = 51)]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private float GridSnapSize;
    [SerializeField] private Building _prefab;
    [SerializeField] private Vector3 _buildingSize;
    [SerializeField] private ItemType _type;

    public string DisplayName => _displayName;
    public Sprite Icon => _icon;
    public Vector3 BuildingSize => _buildingSize;
    public Building Prefab => _prefab;
    public ItemType Type => _type;  
}

public enum PartType
{
    Wall = 0,
    Room = 1,
    Corridor = 3,
    Decoration = 4
}

#if UNITY_EDITOR
[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (BuildingData)target;

        if(data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif
