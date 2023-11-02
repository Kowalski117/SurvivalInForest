using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private ObjectItemsData[] _startingItems;

    private UniqueID _uniqueId;
    private DistanceHandler _distanceHandler;

    public static UnityAction<ChestInventory, int> OnDinamicChestDisplayRequested;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public DistanceHandler DistanceHandler => _distanceHandler;

    protected override void Awake()
    {
        _uniqueId = GetComponentInParent<UniqueID>();
        base.Awake();
        _distanceHandler = GetComponentInChildren<DistanceHandler>();
        LoadInventory();
    }

    private void OnEnable()
    {
        _distanceHandler.OnDistanceExceeded += Interact;
    }

    private void OnDisable()
    {
        _distanceHandler.OnDistanceExceeded -= Interact;
    }

    public void Interact()
    {
        OnDinamicChestDisplayRequested?.Invoke(this, 0);
    }

    public void EndInteraction() { }

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(_uniqueId.Id, saveData);
    }

    protected override void LoadInventory()
    {
        if (ES3.KeyExists(_uniqueId.Id))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(_uniqueId.Id);
            PrimaryInventorySystem = saveData.InventorySystem;
        }
        else
        {
            if (_startingItems.Length > 0) 
            {
                foreach (var slot in SetLootItem().Items)
                {
                    for (int i = 0; i < slot.Items.Length; i++)
                    {
                        for(int j = 0; j < slot.Items[i].Amount; j++)
                        {
                            PrimaryInventorySystem.AddToInventory(slot.Items[i].ItemData, 1, slot.Items[i].ItemData.Durability);
                        }
                    }
                }
            }
        }
    }

    private ObjectItemsData SetLootItem()
    {
        return _startingItems[Random.Range(0, _startingItems.Length)];
    }
}
