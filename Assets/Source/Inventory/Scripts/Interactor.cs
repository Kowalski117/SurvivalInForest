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
    [SerializeField] private float _liftingDelay = 2f;
    [SerializeField] private Transform _removeItemPoint;
    [SerializeField] private Transform _playerTransform;

    private IInteractable _currentInteractable;
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
        _buildTool.OnCompletedBuild += ClearIInteractable;
        _playerInputHandler.InteractionPlayerInput.OnPickUp += PickUpItem;
        _playerInputHandler.InventoryPlayerInput.OnToggleIInteractable += InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction += InteractableConstruction;

        _hotbarDisplay.ItemClicked += PlantSeed;

        InventorySlotUI.OnItemRemove += RemoveItem;
    }

    private void OnDisable()
    {
        _buildTool.OnCompletedBuild -= ClearIInteractable;
        _playerInputHandler.InteractionPlayerInput.OnPickUp -= PickUpItem;
        _playerInputHandler.InventoryPlayerInput.OnToggleIInteractable -= InteractableInventory;
        _playerInputHandler.InteractionPlayerInput.OnInteractedConstruction -= InteractableConstruction;

        _hotbarDisplay.ItemClicked -= PlantSeed;

        InventorySlotUI.OnItemRemove -= RemoveItem;
    }

    private void Update()
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
                    if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
                    {
                        if(itemPickUp.enabled == true)
                        {
                            OnTimeUpdate?.Invoke(LookTimerPracent, "");
                            _currentItemPickUp = itemPickUp;
                        }
                    }
                    else if (hitInfo.collider.TryGetComponent(out ObjectPickUp objectPickUp))
                    {
                        OnTimeUpdate?.Invoke(LookTimerPracent, "");
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
                _lookTimer = 0;
                _isIconFilled = false;
            }
        }
        else
        {
            _lookTimer = 0;
            OnTimeUpdate?.Invoke(0f, "");
            _isIconFilled = false;
            _isInventoryFull = false;
        }
    }

    public void PickUpAninationEvent()
    {
        if (_currentItemPickUp != null)
        {
            if (_playerInventoryHolder.AddToInventory(_currentItemPickUp.ItemData, _addAmount, _currentItemPickUp.Durability))
            {
                _currentItemPickUp.PicUp();
                _currentItemPickUp = null;
            }
            else
            {
                _currentItemPickUp.PicUp();
                InstantiateItem(_currentItemPickUp.ItemData, _currentItemPickUp.ItemData.Durability);
                _currentItemPickUp = null;
                _isInventoryFull = true;
            }
        }
        else if(_currentObjectPickUp != null )
        {
            foreach (var itemData in _currentObjectPickUp.ObjectItemsData.Items)
            {
                for (int i = 0; i < itemData.Amount; i++)
                {
                    if (!_playerInventoryHolder.AddToInventory(itemData.ItemData, _addAmount, itemData.ItemData.Durability))
                    {
                        InstantiateItem(itemData.ItemData, itemData.ItemData.Durability);
                    }
                }
            }
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

    public void RemoveItem(InventorySlotUI inventorySlot)
    {
        if (_playerInventoryHolder.InventorySystem.GetItemCount(inventorySlot.AssignedInventorySlot.ItemData) >= 0)
        {
            for (int i = 0; i < inventorySlot.AssignedInventorySlot.Size; i++)
            {
                InstantiateItem(inventorySlot.AssignedInventorySlot.ItemData, inventorySlot.AssignedInventorySlot.Durability);
            }
            _playerInventoryHolder.RemoveInventory(inventorySlot.AssignedInventorySlot, inventorySlot.AssignedInventorySlot.Size);
            //_playerAnimation.RemoveItemAnimationEvent();

            if (inventorySlot.AssignedInventorySlot.ItemData == null)
                inventorySlot.TurnOffHighlight();
        }
    }

    public void InstantiateItem(InventoryItemData itemData, float durability)
    {
        if(itemData.ItemPrefab != null)
        {
            ItemPickUp itemPickUp = Instantiate(itemData.ItemPrefab, _removeItemPoint.position, Quaternion.identity);
            itemPickUp.GenerateNewID();
            itemPickUp.UpdateDurability(durability);
        }
    }
    
    private void InteractableInventory()
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();

                if (_currentInteractable == null)
                {
                    _currentInteractable = interactable;
                }
                else
                {
                    _currentInteractable = null;
                }
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

    private void ClearIInteractable()
    {
        //if (_currentInteractable != null)
        //{
        //    _currentInteractable.Interact();
        //    _currentInteractable = null;
        //}
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