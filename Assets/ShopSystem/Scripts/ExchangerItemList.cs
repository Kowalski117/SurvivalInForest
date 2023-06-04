using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item List", order = 51)]
public class ExchangerItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedGold;
    [SerializeField] private float _sellMarkUp;
    [SerializeField] private float _buyMarkUp;

    public List<ShopInventoryItem> Items => _items;
    public int MaxAllowedGold => _maxAllowedGold;
    public float SellMarkUp => _sellMarkUp;
    public float BuyMarkUp => _buyMarkUp;
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