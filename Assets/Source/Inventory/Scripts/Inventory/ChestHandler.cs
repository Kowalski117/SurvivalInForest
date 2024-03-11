using System;
using UnityEngine;

public class ChestHandler : Raycast
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private UIInventoryHandler _inventoryHandler;

    private ChestType _currentChestType;
    private ChestInventory _chestInventory;
    private bool _isRay = false;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;
    public event Action<ChestType> OnChestTypeChanged;

    public ChestInventory ChestInventory => _chestInventory;

    private void Update()
    {
        if (IsRayHittingSomething(_layerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ChestInventory chestInventory))
            {
                if(chestInventory != _chestInventory)
                {
                    _isRay = true;
                    Open(chestInventory);
                }
            }
        }
        else if(_isRay) 
        {
            _isRay = false;
            Close();
        }
    }

    public void Open(ChestInventory chestInventory)
    {
        if (chestInventory && _inventoryHandler && !_inventoryHandler.IsChestOpen)
        {
            _chestInventory = chestInventory;
            _currentChestType = _chestInventory.ChestType;
            _inventoryHandler.DisplayChestInventory(chestInventory, 0);
            OnChestTypeChanged?.Invoke(_currentChestType);
            OnInteractionStarted?.Invoke();
        }
    }

    public void Close()
    {
        if (_chestInventory != null && _inventoryHandler.IsChestOpen)
        {
            _inventoryHandler.DisplayChestInventory(_chestInventory, 0);
            _chestInventory = null;
            OnInteractionFinished?.Invoke();
        }
    }
}

public enum ChestType
{
    SurvivalChest,
    BonusChest
}
