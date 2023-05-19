using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem _inventorySystem;

    public static event UnityAction<InventorySystem> OnDinamicInventoryDispleyRequested;

    public InventorySystem InventorySystem => _inventorySystem;

    private void Awake()
    {
        _inventorySystem = new InventorySystem(_inventorySize);
    }
}
