using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item", order = 51)]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private int _id = -1;
    [SerializeField] private ItemType _type;
    [SerializeField] private string _displayName;
    [TextArea(4,4)]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxStackSize;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private float _gorenjeTime;
    [SerializeField] private float _numberUses = 10;

    public int Id => _id;
    public ItemType Type => _type;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite Icon => _icon;
    public int MaxStackSize => _maxStackSize;
    public GameObject ItemPrefab => _itemPrefab;
    public float GorenjeTime => _gorenjeTime;
    public float NumberUses => _numberUses;

    public void SetId(int id)
    {
        _id = id;
    }

    public void UseItem()
    {
        
    }

    public void LowerStrength(float amount)
    {
        if(_numberUses > 0)
            _numberUses -= amount;
    }
}

public enum ItemType
{
    Build,
    InteractBuilding,
    Weapon,
    Tool,
    Food,
    Decor,
    Clothing,
    Item
}

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryItemData))]
public class ItemDataEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var data = (InventoryItemData)target;

        if (data == null || data.Icon == null)
            return null;

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(data.Icon.texture, texture);
        return texture;
    }
}
#endif