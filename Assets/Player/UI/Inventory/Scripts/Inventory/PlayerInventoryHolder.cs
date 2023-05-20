using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private int _secondaryInventorySize;
    [SerializeField] private InventorySystem _secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => _secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackInventoryDispleyRequested;

    protected override void Awake()
    {
        base.Awake();

        _secondaryInventorySystem = new InventorySystem(_secondaryInventorySize);
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame) // поменять
            OnPlayerBackpackInventoryDispleyRequested?.Invoke(_secondaryInventorySystem);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        else if (_secondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
