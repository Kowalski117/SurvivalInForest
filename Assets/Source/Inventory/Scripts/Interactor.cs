using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : Raycast
{
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private LayerMask _interactionConstructionLayer;

    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private SaveItemHandler _saveItemHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private ClothesSlotsHandler _clothesSlotsHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private InventoryOperator _inventoryOperator;

    [SerializeField] private float _liftingDelay = 2f;
    [SerializeField] private Transform _removeItemPoint;
    [SerializeField] private Transform _playerTransform;

    private bool _isStartingPick = true;
    private float _lookTimer = 0;
    private ItemPickUp _currentItemPickUp;
    private ObjectPickUp _currentObjectPickUp;
    private bool _isIconFilled = false;
    private bool _isInventoryFull = false;
    private SleepPointSaveData _sleepPointSaveData;

    private int _addAmount = 1;


    public event UnityAction<float, string> OnTimeUpdate;

    public float LookTimerPracent => _lookTimer / _liftingDelay;
    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;
    public SleepPointSaveData SleepPointSaveData => _sleepPointSaveData;

    private void Start()
    {
        _sleepPointSaveData = new SleepPointSaveData(_playerTransform.position, _playerTransform.rotation);
    }

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickUp += PickUpItem;
        _playerInputHandler.InventoryPlayerInput.OnToggleIInteractable += InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction += InteractableConstruction;

        _hotbarDisplay.ItemClicked += PlantSeed;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickUp -= PickUpItem;
        _playerInputHandler.InventoryPlayerInput.OnToggleIInteractable -= InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction -= InteractableConstruction;

        _hotbarDisplay.ItemClicked -= PlantSeed;
    }

    private void Update()
    {
        HandleLookTimer();
        HandleInventoryFull();
    }

    private void HandleLookTimer()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (_isStartingPick && !_isInventoryFull)
            {
                _lookTimer += Time.deltaTime;

                OnTimeUpdate?.Invoke(LookTimerPracent, "");

                if (_lookTimer >= _liftingDelay && !_isIconFilled)
                {
                    _isIconFilled = true;
                    _playerAnimation.PickUp();

                    if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp) && itemPickUp.enabled)
                    {
                        _currentItemPickUp = itemPickUp;
                    }
                    else if (hitInfo.collider.TryGetComponent(out ObjectPickUp objectPickUp))
                    {
                        _currentObjectPickUp = objectPickUp;
                    }

                    PickUpAninationEvent();
                }
                else
                {
                    _isIconFilled = false;
                }
            }
            else
            {
                ResetLookTimer();
            }
        }
        else
        {
            ResetLookTimer();
        }
    }

    private void HandleInventoryFull()
    {
        if (_isInventoryFull)
        {
            ResetLookTimer();
        }
    }

    private void ResetLookTimer()
    {
        _lookTimer = 0;
        OnTimeUpdate?.Invoke(0f, "");
        _isIconFilled = false;
        _isInventoryFull = false;
    }

    public void PickUpAninationEvent()
    {
        if (_currentItemPickUp != null)
        {
            if (!_playerInventoryHolder.AddToInventory(_currentItemPickUp.ItemData, _addAmount, _currentItemPickUp.Durability))
            {
                _inventoryOperator.InstantiateItem(_currentItemPickUp.ItemData, _currentItemPickUp.ItemData.Durability);
                _isInventoryFull = true;
            }
            _currentItemPickUp.PicUp();
            _currentItemPickUp = null;
        }
        else if (_currentObjectPickUp != null)
        {
            foreach (var inventoryData in _currentObjectPickUp.ObjectItemsData.Items)
            {
                for (int i = 0; i < inventoryData.Amount; i++)
                {
                    if (!_playerInventoryHolder.AddToInventory(inventoryData.ItemData, _addAmount, inventoryData.ItemData.Durability))
                    {
                        _inventoryOperator.InstantiateItem(inventoryData.ItemData, inventoryData.ItemData.Durability);
                    }
                }
            }
                //{

                //List<InventoryItem> itemsWithInsufficientSpace = _inventoryOperator.GetItemsWithInsufficientSpace(_currentObjectPickUp.ObjectItemsData.Items);

                //foreach (var inventoryData in itemsWithInsufficientSpace)
                //{
                //    Debug.Log(inventoryData.Amount);
                //    for (int i = 0; i < inventoryData.Amount; i++)
                //    {
                //        _inventoryOperator.InstantiateItem(inventoryData.ItemData, inventoryData.ItemData.Durability);
                //    }
                //}
            _currentObjectPickUp.PicUp();
            _currentObjectPickUp = null;
        }
        _isIconFilled = false;
        _isStartingPick = true;
    }


    public void StartPickUpAninationEvent()
    {
        _isStartingPick = false;
    }
    
    private void InteractableInventory()
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    private void InteractableConstruction()
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out SleepingPlace interactable))
            {
                interactable.Interact(this, out bool interactSuccessful);
                _sleepPointSaveData = new SleepPointSaveData(_playerTransform.position, _playerTransform.rotation);
            }

            if (hitInfo.collider.TryGetComponent(out Fire fire))
            {
                InventorySlot slot = _hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot;

                if (fire.AddFire(slot))
                {
                    _playerInventoryHolder.RemoveInventory(slot, _addAmount);
                }
            }
        }
    }

    private void PickUpItem()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
            {
                if (LookTimerPracent >= 1)
                {
                    _playerAnimation.PickUp();
                    _currentItemPickUp = itemPickUp;
                }
            }
        }
    }

    private void PlantSeed(InventorySlot slot)
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out GardenBed gardenBed))
            {
                if(gardenBed.StartGrowingSeed(slot.ItemData))
                {
                    _playerInventoryHolder.RemoveInventory(slot, _addAmount);
                }
            }
        }
    }
}

[System.Serializable]
public struct SleepPointSaveData
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;

    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;

    public SleepPointSaveData(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }
}