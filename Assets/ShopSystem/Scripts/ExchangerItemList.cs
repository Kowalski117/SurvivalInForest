using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Exchanger Item List", order = 51)]
public class ExchangerItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;

    public List<ShopInventoryItem> Items => _items;
}

[System.Serializable]
public struct ShopInventoryItem
{
    [SerializeField] private InventoryItemData _itemData1;
    [SerializeField] private int _amount1;
    [SerializeField] private InventoryItemData _itemData2;
    [SerializeField] private int _amount2;

    public InventoryItemData ItemData1 => _itemData1;
    public int Amount1 => _amount1;
    public InventoryItemData ItemData2 => _itemData2;
    public int Amount2 => _amount2;
}