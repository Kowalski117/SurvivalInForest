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
    public static Action<InventorySystem, int> OnDinamicInventoryDisplayRequested;

    public InventorySystem InventorySystem => PrimaryInventorySystem;
    public int Offset => _offset;

    protected virtual void Awake()
    {
        PrimaryInventorySystem = new InventorySystem(_inventorySize);
    }

    private void OnEnable()
    {
        SavingGame.OnGameLoaded += Load;
        SavingGame.OnGameSaved += Save;
    }

    private void OnDisable()
    {
        SavingGame.OnGameLoaded -= Load;
        SavingGame.OnGameSaved -= Save;
    }

    protected abstract void Save();
    protected virtual void Load()
    {
        OnInventoryChanged?.Invoke();
    }
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

