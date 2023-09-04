using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Xml;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private List<InventoryItemData> _startingItems;

    private UniqueID _uniqueId;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        _uniqueId = GetComponentInParent<UniqueID>();
        base.Awake();
    }

    private void Start()
    {
        foreach (var item in _startingItems)
        {
            PrimaryInventorySystem.AddToInventory(item, 1, item.Durability);
        }

        LoadInventory();
    }
    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0);
        interactSuccessfull = true;
    }

    public void EndInteraction()
    {

    }

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots, transform.position, transform.rotation);
        ES3.Save(_uniqueId.Id, saveData);
    }

    protected override void LoadInventory()
    {
        if (ES3.KeyExists(_uniqueId.Id))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(_uniqueId.Id);
            PrimaryInventorySystem = saveData.InventorySystem;
        }
    }
}
