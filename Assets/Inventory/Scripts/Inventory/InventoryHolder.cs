using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem PrimaryInventorySystem;
    [SerializeField] private int _offset = 6;
    [SerializeField] private int _gold;

    public static UnityAction<InventorySystem, int> OnDinamicInventoryDispleyRequested;

    public InventorySystem InventorySystem => PrimaryInventorySystem;
    public int Offset => _offset;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadData += LoadInventory;
        PrimaryInventorySystem = new InventorySystem(_inventorySize, _gold);
    }

    protected abstract void LoadInventory(SaveData saveData);
}

[System.Serializable]
public struct InventorySaveData
{
    [SerializeField] private InventorySystem _inventorySystem;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public InventorySystem InventorySystem => _inventorySystem;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public InventorySaveData(InventorySystem inventorySystem, Vector3 position, Quaternion rotation)
    {
        _inventorySystem = inventorySystem;
        _position = position;
        _rotation = rotation;
    }

    public InventorySaveData(InventorySystem inventorySystem)
    {
        _inventorySystem = inventorySystem;
        _position = Vector3.zero;
        _rotation = Quaternion.identity;
    }
}

