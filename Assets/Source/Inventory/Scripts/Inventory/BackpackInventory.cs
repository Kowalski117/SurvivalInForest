using UnityEngine;
using UnityEngine.Events;

public class BackpackInventory : InventoryHolder
{
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private InventoryOperator _inventoryOperator;

    private int _addAmount = 1;
    private bool _isEnable = false;

    public event UnityAction<InventorySystem, int> OnDinamicDisplayInventory;

    public bool IsEnable => _isEnable;

    private void OnEnable()
    {
        _clothesSlotsHandler.OnInteractionBackpack += Show;
        _clothesSlotsHandler.OnRemoveBackpack += Show;
        _clothesSlotsHandler.OnRemoveBackpack += RemoveAllItems;

        SaveGame.OnSaveGame += SaveInventory;
        SaveGame.OnLoadData += LoadInventory;
    }

    private void OnDisable()
    {
        _clothesSlotsHandler.OnInteractionBackpack -= Show;
        _clothesSlotsHandler.OnRemoveBackpack -= Show;
        _clothesSlotsHandler.OnRemoveBackpack -= RemoveAllItems;

        SaveGame.OnSaveGame -= SaveInventory;
        SaveGame.OnLoadData -= LoadInventory;
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
                PrimaryInventorySystem.RemoveItemsInventory(slot, slot.Size);
            }
        }
    }

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.BackpackInventory, saveData);
    }

    protected override void LoadInventory()
    {
        if (ES3.KeyExists(SaveLoadConstants.BackpackInventory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.BackpackInventory);
            PrimaryInventorySystem = saveData.InventorySystem;

            base.LoadInventory();
        }
    }
}
