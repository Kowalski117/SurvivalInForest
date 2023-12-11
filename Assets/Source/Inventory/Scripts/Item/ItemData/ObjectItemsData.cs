using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/ObjectItemsData", order = 51)]
public class ObjectItemsData : ScriptableObject
{
    [SerializeField] private LootItems[] _items;

    public LootItems[] Items => _items;
    public LootItems LootRandomItems => _items[Random.Range(0, _items.Length)];
}

[System.Serializable]
public struct LootItems
{
    [SerializeField] private InventoryItem[] _items;

    public InventoryItem[] Items => _items;
}
