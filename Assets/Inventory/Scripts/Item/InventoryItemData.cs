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

    public int Id => _id;
    public ItemType Type => _type;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite Icon => _icon;
    public int MaxStackSize => _maxStackSize;
    public GameObject ItemPrefab => _itemPrefab;



    public void SetId(int id)
    {
        _id = id;
    }
}

public enum ItemType
{
    Build,
    Weapon,
    Food,
    Decor,
    Clothing
}

