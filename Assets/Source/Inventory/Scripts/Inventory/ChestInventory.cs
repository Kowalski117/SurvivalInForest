using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder
{
    [SerializeField] private ObjectItemsData[] _startingItems;

    private UniqueID _uniqueId;

    protected override void Awake()
    {
        _uniqueId = GetComponentInParent<UniqueID>();
        base.Awake();;
    }

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
                ObjectItemsData objectItemsData = _startingItems[Random.Range(0, _startingItems.Length)];
                LootItems lootItems = objectItemsData.LootRandomItems;

                foreach (var inventoryData in lootItems.Items)
                {
                    for (var i = 0; i < inventoryData.Amount; i++)
                    {
                        PrimaryInventorySystem.AddToInventory(inventoryData.ItemData, 1, inventoryData.ItemData.Durability);
                    }
                }
            }
        }
    }
}
