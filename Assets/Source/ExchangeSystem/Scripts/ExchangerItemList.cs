using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Exchange/Item List", order = 51)]
public class ExchangerItemList : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private List<ExchangerItemData> _items;

    public string Name => _name;
    public List<ExchangerItemData> Items => _items;
}
