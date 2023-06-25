using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float _rayDistance;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private LayerMask _interactionInventoryLayer;
    [SerializeField] private LayerMask _interactionItemLayer;
    [SerializeField] private LayerMask _interactionConstructionLayer;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private SelectionPlayerInput _selectionPlayerInput;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private InteractionConstructionPlayerInput _interactionConstructionPlayerInput;

    private Camera _camera;

    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

    private void Awake()
    {
        _camera = Camera.main;
    }

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
                if (_playerInventoryHolder.InventorySystem.AddToInventory(itemPickUp.ItemData, 1))
                {
                    itemPickUp.PicUp();
                }
            }
        }
    }

    private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(_rayOrigin.position, _camera.transform.forward * _rayDistance);
        return Physics.Raycast(ray, out hitInfo, _rayDistance, layerMask);
    }
}