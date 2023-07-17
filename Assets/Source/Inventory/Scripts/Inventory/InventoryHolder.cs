using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem PrimaryInventorySystem;
    [SerializeField] private int _offset = 6;

    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnDinamicInventoryDisplayRequested;

    public InventorySystem InventorySystem => PrimaryInventorySystem;
    public int Offset => _offset;

    protected virtual void Awake()
    {
        PrimaryInventorySystem = new InventorySystem(_inventorySize);
    }

    private void OnEnable()
    {
        SaveGame.OnLoadData += LoadInventory;
        SaveGame.OnSaveGame += SaveInventory;
    }

    private void OnDisable()
    {
        SaveGame.OnLoadData -= LoadInventory;
        SaveGame.OnSaveGame -= SaveInventory;
    }

    protected abstract void SaveInventory();
    protected abstract void LoadInventory();
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

