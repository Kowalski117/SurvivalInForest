public class ClothesInventory : InventoryHolder
{
    protected override void Save()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(SaveLoadConstants.ÑlothesInventory, saveData);
    }

    protected override void Load()
    {
        if (ES3.KeyExists(SaveLoadConstants.ÑlothesInventory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(SaveLoadConstants.ÑlothesInventory);
            PrimaryInventorySystem = saveData.InventorySystem;
            base.Load();
        }
    }
}
