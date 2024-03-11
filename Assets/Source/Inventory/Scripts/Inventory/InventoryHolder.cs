using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem PrimaryInventorySystem;
    [SerializeField] private int _offset = 6;

    public event Action OnInventoryChanged;

    public InventorySystem InventorySystem => PrimaryInventorySystem;
    public int Offset => _offset;

    protected virtual void Awake()
    {
        PrimaryInventorySystem = new InventorySystem(_inventorySize);
    }

    protected virtual void OnEnable()
    {
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnGameSaved += Save;
        SavingGame.OnSaveDeleted += Delete;
    }

    protected virtual void OnDisable()
    {
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnGameSaved -= Save;
        SavingGame.OnSaveDeleted -= Delete;
    }

    protected abstract void Save();
    protected virtual void Load()
    {
        OnInventoryChanged?.Invoke();
    }
    protected abstract void Delete();
}

[System.Serializable]
public struct InventorySaveData
{
    [SerializeField] private InventorySystem _inventorySystem;
    [SerializeField] private List<InventorySlot> _inventorySlots;

    public InventorySystem InventorySystem => _inventorySystem;
    public List<InventorySlot> InventorySlots => _inventorySlots;

    public InventorySaveData(InventorySystem inventorySystem, List<InventorySlot> inventorySlots)
    {
        _inventorySystem = inventorySystem;
        _inventorySlots = inventorySlots;
    }
}

