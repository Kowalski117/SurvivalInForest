public class ClothesInventory : InventoryHolder
{
    private string _clothesInventory = "ClothesInventory";

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots);
        ES3.Save(_clothesInventory, saveData);
    }

    protected override void LoadInventory()
    {
        if (ES3.KeyExists(_clothesInventory))
        {
            InventorySaveData saveData = ES3.Load<InventorySaveData>(_clothesInventory);
            PrimaryInventorySystem = saveData.InventorySystem;

            base.LoadInventory();
        }
    }
}
