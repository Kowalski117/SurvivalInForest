using System;
using UnityEngine;

public class BackpackInventory : InventoryHolder
{
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private InventoryOperator _inventoryOperator;

    private bool _isEnable = false;

    public event Action<InventorySystem, int> OnDinamicDisplayInventory;

    public bool IsEnable => _isEnable;

    protected override void OnEnable()
    {
        base.OnEnable();
        _clothesSlotsHandler.OnBackpackInteractioned += Show;
        _clothesSlotsHandler.OnBackpackRemoved += Show;
        _clothesSlotsHandler.OnBackpackRemoved += RemoveAllItems;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _clothesSlotsHandler.OnBackpackInteractioned -= Show;
        _clothesSlotsHandler.OnBackpackRemoved -= Show;
        _clothesSlotsHandler.OnBackpackRemoved -= RemoveAllItems;
    }

    public void Show()
    {
        OnDinamicDisplayInventory?.Invoke(PrimaryInventorySystem, 0);
        _isEnable = !_isEnable;
    }
    public void RemoveAllItems()
    {
        foreach (var slot in PrimaryInventorySystem.InventorySlots)
        {
            if (slot.ItemData != null)
            {
                for (int i = 0; i < slot.Size; i++)
                {
                    _inventoryOperator.InstantiateItem(slot.ItemData, slot.Durability);
                }
                PrimaryInventorySystem.RemoveSlot(slot, slot.Size);
            }
        }
    }

    protected override void Save()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.BackpackInventory, saveData);
    }

    protected override void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.BackpackInventory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.BackpackInventory);
            PrimaryInventorySystem = saveData.InventorySystem;
            base.Load();
        }
    }

    protected override void Delete()
    {
        if (ES3.KeyExists(SaveLoadConstants.BackpackInventory))
            ES3.DeleteKey(SaveLoadConstants.BackpackInventory);
    }
}
