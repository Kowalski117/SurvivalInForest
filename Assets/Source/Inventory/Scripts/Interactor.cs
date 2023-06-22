using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float _rayDistance;
    [SerializeField] private Transform _rayOrigin;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private SelectionPlayerInput _selectionPlayerInput;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;

    private Camera _camera;

    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _selectionPlayerInput.PickUp += PickUp;
        _inventoryPlayerInput.InteractKeyPressed += StartInteractable;
    }

    private void OnDisable()
    {
        _selectionPlayerInput.PickUp -= PickUp;
        _inventoryPlayerInput.InteractKeyPressed -= StartInteractable;
    }

    private void StartInteractable()
    {
        if (IsRayHittingSomething(_interactionLayer, out RaycastHit hitInfo))
        {
            if(hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(this, out bool interactSuccessful);
            }
        }
    }

    private void PickUp()
    {
        if (IsRayHittingSomething(_interactionLayer, out RaycastHit hitInfo))
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