using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : Raycast
{
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private LayerMask _interactionConstructionLayer;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private PlayerAnimation _playerAnimation;

    [SerializeField] private float _liftingDelay = 2f;

    private IInteractable _currentInteractable;
    private bool _isStartingPick = true;
    private float _lookTimer = 0;
    private ItemPickUp _currentItemPickUp;

    public event UnityAction<float> OnTimeUpdate;

    public float LookTimerPracent => _lookTimer / _liftingDelay;
    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

    private void OnEnable()
    {
        _buildTool.OnCompletedBuild += ClearIInteractable;

        _playerInputHandler.SelectionPlayerInput.PickUp += PickUpItem;
        _playerInputHandler.InventoryPlayerInput.InteractKeyPressed += InteractableInventory;
        _playerInputHandler.InteractionConstructionPlayerInput.OnInteractedConstruction += InteractableConstruction;
    }

    private void OnDisable()
    {
        _buildTool.OnCompletedBuild -= ClearIInteractable;

        _playerInputHandler.SelectionPlayerInput.PickUp -= PickUpItem;
        _playerInputHandler.InventoryPlayerInput.InteractKeyPressed -= InteractableInventory;
        _playerInputHandler.InteractionConstructionPlayerInput.OnInteractedConstruction -= InteractableConstruction;
    }

    private void Update()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
            {
                if(_isStartingPick)
                {
                    _lookTimer += Time.deltaTime;
                    OnTimeUpdate?.Invoke(LookTimerPracent);
                    if (_lookTimer >= _liftingDelay)
                    {
                        _playerAnimation.PickUp();
                        _lookTimer = 0;
                        _currentItemPickUp = itemPickUp;
                    }
                }
            }
        }
        else
        {
            _lookTimer = 0;
            OnTimeUpdate?.Invoke(LookTimerPracent);
        }
    }

    private void InteractableInventory()
    {
        if (IsRayHittingSomething(_interactionConstructionLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(this, out bool interactSuccessful);
                _currentInteractable = interactable;
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
            }
        }
    }

    private void PickUpItem()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
            {
                if(LookTimerPracent >= 1)
                {
                    _playerAnimation.PickUp();
                    _currentItemPickUp = itemPickUp;
                }
            }
        }
    }

    public void PickUpAninationEvent()
    {
        if(_currentItemPickUp != null)
        {
            if (_playerInventoryHolder.AddToInventory(_currentItemPickUp.ItemData, 1))
            {
                _currentItemPickUp.PicUp();
                _currentItemPickUp = null;
            }
            _isStartingPick = true;
        }
    }

    public void StartPickUpAninationEvent()
    {
        _isStartingPick = false;
    }

    private void ClearIInteractable()
    {
        _currentInteractable.Interact(this, out bool interactSuccessful);
        _currentInteractable = null;
    }
}