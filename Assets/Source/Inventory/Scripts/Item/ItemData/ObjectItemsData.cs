using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/ObjectItemsData", order = 51)]
public class ObjectItemsData : ScriptableObject
{
    [SerializeField] private InventoryItem[] _items;

    public InventoryItem[] Items => _items;
}
