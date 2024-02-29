public class ClothesInventory : InventoryHolder
{
    protected override void Save()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.�lothesInventory, saveData);
    }

    protected override void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.�lothesInventory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.�lothesInventory);
            PrimaryInventorySystem = saveData.InventorySystem;
            base.Load();
        }
    }
}
