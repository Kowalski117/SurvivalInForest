using UnityEngine;

[CreateAssetMenu(menuName = "Roulette/Items", order = 51)]
public class ItemsRoulette : ScriptableObject
{
    [SerializeField] private Item[] _items;

    public Item[] Items => _items;
}

[System.Serializable]
public struct Item
{
    [SerializeField] private InventoryItem _itemData;
    [SerializeField] private float _spawnChance;

    public InventoryItem ItemData => _itemData;
    public float SpawnChance => _spawnChance;
}
