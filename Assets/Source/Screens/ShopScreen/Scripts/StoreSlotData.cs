using UnityEngine;

[CreateAssetMenu(menuName = "Shop/StoreSlotData", order = 51)]
public class StoreSlotData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _price;
    [SerializeField] private bool _isOpenAds;
    [SerializeField] private Product[] _products;

    public string Id => _id;
    public string Title => _title;
    public string Description => _description;
    public Sprite Sprite => _sprite;
    public int Price => _price;
    public bool IsOpenAds => _isOpenAds;
    public Product[] Products => _products;
}

[System.Serializable]
public class Product
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private int _amount;

    public InventoryItemData ItemData => _itemData;
    public int Amount => _amount;
}
