using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _iteractionPoint;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private float _interactionPointRadius = 1.0f;

    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;

    private Collider[] _colliders;
    public PlayerInventoryHolder PlayerInventoryHolder { get; private set; }

    public bool IsInteraction { get; private set; }

    private void Awake()
    {
        PlayerInventoryHolder = GetComponent<PlayerInventoryHolder>();
    }

    private void Update()
    {
        _colliders = Physics.OverlapSphere(_iteractionPoint.position, _interactionPointRadius, _interactionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                var interactable = _colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                    StartInteractable(interactable);
            }
        }
    }

    //private void OnEnable()
    //{
    //    _inventoryPlayerInput.SwitchInventory += Open;
    //}

    //private void OnDisable()
    //{
    //    _inventoryPlayerInput.SwitchInventory -= Open;
    //}

    //private void Open()
    //{
    //    for (int i = 0; i < _colliders.Length; i++)
    //    {
    //        var interactable = _colliders[i].GetComponent<IInteractable>();

    //        if (interactable != null)
    //            StartInteractable(interactable);
    //    }
    //}

    private void StartInteractable(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteraction = true;
    }

    public void CompleteInteraction(IInteractable interactable)
    {
        IsInteraction = false;
    }
}
