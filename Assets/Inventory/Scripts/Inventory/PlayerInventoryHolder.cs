using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;

    private void Start()
    {
        SaveGameHandler.Data._playerInventory = new InventorySaveData(PrimaryInventorySystem); // поменять
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            OnPlayerInventoryDispleyRequested?.Invoke(PrimaryInventorySystem, Offset);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.PlayerInventory.InventorySystem != null)
        {
            this.PrimaryInventorySystem = data.PlayerInventory.InventorySystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
}

