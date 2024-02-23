using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] private ItemsRoulette _itemsRoulette;
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

        foreach (Item item in _itemsRoulette.Items)
        {
            int spawnCount = Mathf.FloorToInt(item.SpawnChance * _rouletteSlot.Length);
            for (int i = 0; i < spawnCount; i++)
            {
                itemPool.Add(item);
            }
        }

        for (int i = 0; i < _rouletteSlot.Length; i++)
        {
            if (itemPool.Count > 0)
            {
                int randomIndex = Random.Range(0, itemPool.Count);
                Item randomItem = itemPool[randomIndex];
                _rouletteSlot[i].Init(randomItem.ItemData);
                itemPool.Remove(randomItem);
            }
        }
    }
}