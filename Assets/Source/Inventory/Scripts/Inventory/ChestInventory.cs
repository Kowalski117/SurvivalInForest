using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private List<InventoryItemData> _startingItems;

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
    }

    private void Start()
    {
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
        else
        {
            foreach (var item in _startingItems)
            {
                PrimaryInventorySystem.AddToInventory(item, 1, item.Durability);
            }
        }
    }
}
