using UnityEngine;
using UnityEngine.Events;

public class BackpackInventory : InventoryHolder
{
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private Interactor _interactor;

    public event UnityAction<InventorySystem, int> OnDinamicInventoryDisplayInventory;

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
        OnDinamicInventoryDisplayInventory?.Invoke(PrimaryInventorySystem, 0);
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

    protected override void LoadInventory()
    {
        
    }

    protected override void SaveInventory()
    {
        
    }
}
