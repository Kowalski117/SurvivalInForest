using System.Collections.Generic;
using UnityEngine;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private StoreSlot _prefabSlot;
    [SerializeField] private StoreSlotData[] _storeSlotsData;
    [SerializeField] private Transform _containerSlots;

    private List<StoreSlot> _storeSlots = new List<StoreSlot>();

    private void Awake()
    {
        CreateSlots();
    }

    private void OnEnable()
    {
        foreach (var slot in _storeSlots)
        {
            slot.OnPayButton += PaySlot;
        }
    }

    private void OnDisable()
    {
        foreach (var slot in _storeSlots)
        {
            slot.OnPayButton -= PaySlot;
        }
    }

    public void CreateSlots()
    {
        foreach (var slot in _storeSlotsData)
        {
            StoreSlot storeSlot = Instantiate(_prefabSlot, _containerSlots);
            _storeSlots.Add(storeSlot);
            storeSlot.Init(slot);
        }
    }

    private void PaySlot(StoreSlotData data)
    {
        foreach(var product in data.Products)
        {
            _inventoryHolder.AddToInventory(product.ItemData, product.Amount);
        }
    }
}
