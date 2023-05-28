using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField] private List<string> _collectedItems;
    [SerializeField] private SerializableDictionary<string, InventorySaveData> _chestDictionary;
    [SerializeField] private SerializableDictionary<string, ItemPickUpSaveData> _activeItems;

    public InventorySaveData _playerInventory;

    public List<string> CollectedItems => _collectedItems;
    public SerializableDictionary<string, InventorySaveData> ChestDictionary => _chestDictionary;
    public SerializableDictionary<string, ItemPickUpSaveData> ActiveItems => _activeItems;
    public InventorySaveData PlayerInventory => _playerInventory;

    public SaveData()
    {
        _collectedItems = new List<string>();
        _chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        _activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        _playerInventory = new InventorySaveData();
    }

    public void SetPlayerInventory(InventorySaveData playerInventory)
    {
        _playerInventory = playerInventory;
    }
}
