using BehaviorDesigner.Runtime;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item Data/Item", order = 51)]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private int _id = -1;
    [SerializeField] private ItemType _type;
    [SerializeField] private string _displayName;
    [TextArea(4,4)]
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxStackSize;
    [SerializeField] private ItemPickUp _itemPrefab;
    [SerializeField] private float _gorenjeTime;
    [SerializeField] private float _durability;
    [SerializeField] private Color _colorDurability = Color.red;
    [SerializeField] private TypeBehaviorInWater _typeBehaviorInWater = TypeBehaviorInWater.PopsUp;

    public int Id => _id;
    public ItemType Type => _type;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite Icon => _icon;
    public int MaxStackSize => _maxStackSize;
    public ItemPickUp ItemPrefab => _itemPrefab;
    public float GorenjeTime => _gorenjeTime;
    public float Durability => _durability;
    public Color ColorDurability => _colorDurability;
    public TypeBehaviorInWater TypeBehaviorInWater => _typeBehaviorInWater;

    public void SetId(int id)
    {
        _id = id;
    }

    public void UseItem()
    {
        
    }
}

public enum ItemType
{
    Build = 1,
    InteractBuilding = 2,
    Weapon = 3,
    Tool = 4,
    Food = 5,
    Decor = 6,
    Clothes = 7,
    Item = 8,
    Seed = 9,
    None = 10,
    ClothesOnHead = 11,
    ClothesOnTorso = 12,
    ClothesOnFeet = 13,
    ClothingShoes = 14,
    Backpack = 15,

}

public enum TypeBehaviorInWater
{
    None = 0,
    Sinking = 1,
    PopsUp = 2,
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