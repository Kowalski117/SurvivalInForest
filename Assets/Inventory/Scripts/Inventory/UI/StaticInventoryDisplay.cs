using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;
    [SerializeField] private InventorySlotUI[] _slots;

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }

    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        for (int i = 0; i < _inventoryHolder.Offset; i++)
        {
            slotDictionary.Add(_slots[i], inventorySystem.InventorySlots[i]);
            _slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }

    private void RefreshStaticDisplay()
    {
        if (_inventoryHolder != null)
        {
            inventorySystem = _inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssingSlot(inventorySystem, 0);
    }
}
