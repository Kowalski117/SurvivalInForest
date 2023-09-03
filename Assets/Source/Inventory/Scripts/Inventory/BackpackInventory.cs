using UnityEngine;
using UnityEngine.Events;

public class BackpackInventory : InventoryHolder
{
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private Interactor _interactor;

    private string _backpackInventory = "BackpackInventory";
    private bool _isEnable = false;

    public event UnityAction<InventorySystem, int> OnDinamicDisplayInventory;

    public bool IsEnable => _isEnable;

    private void OnEnable()
    {
        _clothesSlotsHandler.OnInteractionBackpack += Show;
        _clothesSlotsHandler.OnRemoveBackpack += Show;
        _clothesSlotsHandler.OnRemoveBackpack += RemoveAllItems;
    }

    private void OnDisable()
    {
        _clothesSlotsHandler.OnInteractionBackpack -= Show;
        _clothesSlotsHandler.OnRemoveBackpack -= Show;
        _clothesSlotsHandler.OnRemoveBackpack -= RemoveAllItems;
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
            if(slot.ItemData != null)
            {
                _interactor.InstantiateItem(slot.ItemData, slot.Durability);
                PrimaryInventorySystem.RemoveItemsInventory(slot, slot.Size);
            }
        }
    }

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots, transform.position, transform.rotation);
        ES3.Save(_backpackInventory, saveData);
    }

    protected override void LoadInventory()
    {
        Debug.Log(_backpackInventory);
        InventorySaveData saveData = ES3.Load<InventorySaveData>(_backpackInventory);
        PrimaryInventorySystem = saveData.InventorySystem;
    }
}
