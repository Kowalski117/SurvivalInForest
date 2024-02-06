using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Exchange/Item", order = 51)]
public class ExchangerItemData : ScriptableObject
{
    [SerializeField, Range(1, 3)] private int _rating;
    [SerializeField] private ItemsToExchange[] _itemsToExchange;
    [SerializeField] private ExchangedItem[] _itemsToReceive;

    public int Rating => _rating;
    public ItemsToExchange[] ItemsToExchange => _itemsToExchange;
    public ExchangedItem[] ItemsToReceive => _itemsToReceive;
}

[System.Serializable]
public struct ItemsToExchange
{
    [SerializeField] private ExchangedItem[] _itemsToExchange;

    public ExchangedItem[] Items => _itemsToExchange;
}

[System.Serializable]
public struct ExchangedItem
{
    [SerializeField] private InventoryItemData _item;
    [SerializeField] private int _amount;

    public InventoryItemData ItemData => _item;
    public int Amount => _amount;
}
