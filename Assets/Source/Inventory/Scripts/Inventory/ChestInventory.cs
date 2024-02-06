using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder
{
    [SerializeField] private ObjectItemsData[] _startingItems;
    [SerializeField] private ChestType chestType = ChestType.SurvivalChest;

    private UniqueID _uniqueId;

    public ChestType ChestType => chestType;

    protected override void Awake()
    {
        _uniqueId = GetComponentInParent<UniqueID>();
        base.Awake();;
    }

    public void AddInventoryItem(InventoryItemData itemData, int amount)
    {
        PrimaryInventorySystem.AddToInventory(itemData, amount, itemData.Durability);
    }

    public void RemoveAllItems()
    {
        foreach (InventorySlot slot in PrimaryInventorySystem.GetAllFilledSlots())
        {
            PrimaryInventorySystem.RemoveItemsInventory(slot.ItemData, slot.Size);
        }
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
                        AddInventoryItem(inventoryData.ItemData, 1);
                    }
                }
            }
        }
    }
}
