using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem _primaryInventorySystem;

    public static UnityAction<InventorySystem> OnDinamicInventoryDispleyRequested;

    public InventorySystem PrimaryInventorySystem => _primaryInventorySystem;

    protected virtual void Awake()
    {
        _primaryInventorySystem = new InventorySystem(_inventorySize);
    }
}
