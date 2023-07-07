using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Exchanger Item List", order = 51)]
public class ExchangerItemList : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private List<ExchangerInventoryItem> _items;

    public string Name => _name;
    public List<ExchangerInventoryItem> Items => _items;
}

[System.Serializable]
public struct ExchangerInventoryItem
{
    [SerializeField] private ExchangedItem[] _itemsToExchange;
    [SerializeField] private ExchangedItem[] _itemsToReceive;

    public ExchangedItem[] ItemsToExchange => _itemsToExchange;
    public ExchangedItem[] ItemsToReceive => _itemsToReceive;
}

[System.Serializable]
public class ExchangedItem
{
    [SerializeField] private InventoryItemData _item;
    [SerializeField] private int _amount;

    public InventoryItemData ItemData => _item;
    public int Amount => _amount;
}
