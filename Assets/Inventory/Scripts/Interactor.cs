using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _interactionPointRadius = 1.0f;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;

    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;

    private Collider[] _colliders;

    public bool IsInteraction { get; private set; }

    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

    private void OnEnable()
    {
        _inventoryPlayerInput.InteractKeyPressed += StartInteractable;
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.InteractKeyPressed -= StartInteractable;
    }

    private void Update()
    {
        _colliders = Physics.OverlapSphere(transform.position, _interactionPointRadius, _interactionLayer);
    }

    private void StartInteractable()
    {
        if (_colliders.Length > 0)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                var interactable = _colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                    interactable.Interact(this, out bool interactSuccessful);
            }
        }
    }

    public void CompleteInteraction(IInteractable interactable)
    {
        IsInteraction = false;
    }
}
//public class Interactor : MonoBehaviour
//{
//    //[SerializeField] private Transform _iteractionPoint;
//    //[SerializeField] private LayerMask _interactionLayer;
//    [SerializeField] private float _interactionPointRadius = 1.0f;
//    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
//    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;

//    private SphereCollider _collider;
//    private List<IInteractable> _interactables = new List<IInteractable>();

//    public bool IsInteraction { get; private set; }

//    public PlayerInventoryHolder PlayerInventoryHolder => _playerInventoryHolder;

//    private void Awake()
//    {
//        _collider = GetComponent<SphereCollider>();
//        _collider.radius = _interactionPointRadius;
//    }

//    private void OnEnable()
//    {
//        _inventoryPlayerInput.InteractKeyPressed += StartInteractable;
//    }

//    private void OnDisable()
//    {
//        _inventoryPlayerInput.InteractKeyPressed -= StartInteractable;
//    }

//    //private void Update()
//    //{
//    //    _colliders = Physics.OverlapSphere(_iteractionPoint.position, _interactionPointRadius, _interactionLayer);
//    //}

//    private void StartInteractable()
//    {
//        if(_interactables.Count > 0)
//        {
//            for (int i = 0; i < _interactables.Count; i++)
//            {
//                if (_interactables[i] != null)
//                    _interactables[i].Interact(this, out bool interactSuccessful);
//            }
//        }
//    }

//    public void CompleteInteraction(IInteractable interactable)
//    {
//        IsInteraction = false;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.TryGetComponent(out ExchangeKeeper interactable))
//        {
//            if (interactable != null)
//            {
//                _interactables.Add(interactable);
//            }
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.TryGetComponent(out ExchangeKeeper interactable))
//        {
//            if (interactable != null)
//            {
//                _interactables.Remove(interactable);
//            }
//        }
//    }
//}
