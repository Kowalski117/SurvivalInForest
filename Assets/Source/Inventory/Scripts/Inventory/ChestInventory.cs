using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadData += LoadInventory;
    }

    private void Start()
    {
        var chestSaveData = new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation);

        SaveGameHandler.Data.ChestDictionary.Add(GetComponent<UniqueID>().Id, chestSaveData);
    }

    public void Interact(Interactor interactor, out bool interactSuccessfull)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0);
        interactSuccessfull = true;
    }

    public void EndInteraction()
    {

    }

    protected override void LoadInventory(SaveData data)
    {
        if(data.ChestDictionary.TryGetValue(GetComponent<UniqueID>().Id, out InventorySaveData chestData))
        {
            this.PrimaryInventorySystem = chestData.InventorySystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }
}
