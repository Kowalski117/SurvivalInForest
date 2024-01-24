using System;
using UnityEngine;
using UnityEngine.Events;

public class ChestHandler : Raycast
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private UIInventoryHandler _inventoryHandler;

    private ChestInventory _chestInventory;

    public event UnityAction OnInteractionStarted;
    public event UnityAction OnInteractionFinished;

    public ChestInventory ChestInventory => _chestInventory;

    private void Update()
    {
        if (IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ChestInventory chestInventory))
            {
                _chestInventory = chestInventory;
                if (_inventoryHandler && !_inventoryHandler.IsChestOpen)
                {
                    _inventoryHandler.DisplayChestInventory(_chestInventory, 0);
                    OnInteractionStarted?.Invoke(); 
                }
            }
        }
        else
        {
            CloseChest();
        }
    }

    private void CloseChest()
    {
        if (_chestInventory != null && _inventoryHandler.IsChestOpen)
        {
            _inventoryHandler.DisplayChestInventory(_chestInventory, 0);
            _chestInventory = null;
            OnInteractionFinished?.Invoke();
        }
    }
}
