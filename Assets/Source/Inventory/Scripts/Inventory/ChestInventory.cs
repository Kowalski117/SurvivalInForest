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

    public void AddItem(InventoryItemData itemData, int amount)
    {
        PrimaryInventorySystem.AddItem(itemData, amount, itemData.Durability);
    }

    public void RemoveAllItems()
    {
        foreach (InventorySlot slot in PrimaryInventorySystem.GetAllFilledSlots())
        {
            PrimaryInventorySystem.RemoveItem(slot.ItemData, slot.Size);
        }
    }

    protected override void Save()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(_uniqueId.Id, saveData);
    }

    protected override void Load()
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
                        AddItem(inventoryData.ItemData, 1);
                    }
                }
            }
        }
    }
}
