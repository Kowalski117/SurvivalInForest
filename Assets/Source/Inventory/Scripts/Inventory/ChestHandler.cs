using System;
using UnityEngine;

public class ChestHandler : Raycast
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private UIInventoryHandler _inventoryHandler;

    private ChestInventory _chestInventory;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;

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
