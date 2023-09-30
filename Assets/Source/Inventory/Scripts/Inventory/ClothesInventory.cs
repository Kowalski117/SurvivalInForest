public class ClothesInventory : InventoryHolder
{
    private string _clothesInventory = "ClothesInventory";

    protected override void SaveInventory()
    {
        InventorySaveData saveData = new InventorySaveData(PrimaryInventorySystem, PrimaryInventorySystem.InventorySlots, transform.position, transform.rotation);
        ES3.Save(_clothesInventory, saveData);
    }

    protected override void LoadInventory()
    {
        InventorySaveData saveData = ES3.Load<InventorySaveData>(_clothesInventory);
        PrimaryInventorySystem = saveData.InventorySystem;

        base.LoadInventory();
    }
}
