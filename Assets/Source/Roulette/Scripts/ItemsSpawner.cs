using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    [SerializeField] private RouletteSlot[] _rouletteSlot;

    private void Start()
    {
        SpawnItems();
    }

    public void SpawnItems()
    {
        for (int i = 0; i < _rouletteSlot.Length; i++)
        {
            _rouletteSlot[i].Clear();
        }

        List<Item> itemPool = new List<Item>();

        foreach (Item item in _items)
        {
            int spawnCount = Mathf.FloorToInt(item.SpawnChance * _rouletteSlot.Length);
            for (int i = 0; i < spawnCount; i++)
            {
                itemPool.Add(item);
            }
        }

        for (int i = 0; i < _rouletteSlot.Length; i++)
        {
            int randomIndex = Random.Range(0, itemPool.Count);
            Item randomItem = itemPool[randomIndex];
            _rouletteSlot[i].Init(randomItem.ItemData);
            itemPool.Remove(randomItem);
        }
    }
}

[System.Serializable]
public struct Item
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private float _spawnChance;

    public InventoryItemData ItemData => _itemData;
    public float SpawnChance => _spawnChance;
}