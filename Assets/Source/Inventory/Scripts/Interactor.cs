using UnityEngine;
using UnityEngine.Events;

public class Interactor : Raycast
{
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private LayerMask _interactionConstructionLayer;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private SelectionPlayerInput _selectionPlayerInput;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private InteractionConstructionPlayerInput _interactionConstructionPlayerInput;

    [SerializeField] private float _liftingDelay = 2f;

    private float _lookTimer = 0;

    public event UnityAction<float> OnTimeUpdate;

    public float LookTimerPracent => _lookTimer / _liftingDelay;
    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

    private void OnEnable()
    {
        _selectionPlayerInput.PickUp += PickUp;
        _inventoryPlayerInput.InteractKeyPressed += InteractableInventory;
        _interactionConstructionPlayerInput.OnInteractedConstruction += InteractableConstruction;
    }

    private void OnDisable()
    {
        _selectionPlayerInput.PickUp -= PickUp;
        _inventoryPlayerInput.InteractKeyPressed -= InteractableInventory;
        _interactionConstructionPlayerInput.OnInteractedConstruction -= InteractableConstruction;
    }

    private void Update()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
            {
                _lookTimer += Time.deltaTime;
                OnTimeUpdate?.Invoke(LookTimerPracent);

                if (_lookTimer >= _liftingDelay)
                {
                    if (_playerInventoryHolder.AddToInventory(itemPickUp.ItemData, 1))
                    {
                        itemPickUp.PicUp();
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

    private void PickUp()
    {
        if (IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
            {
                if (_playerInventoryHolder.AddToInventory(itemPickUp.ItemData, 1))
                {
                    itemPickUp.PicUp();
                }
            }
        }
    }
}