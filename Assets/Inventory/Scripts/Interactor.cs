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

    public bool IsInteraction { get; private set; }

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

    private void EndInreraction()
    {
        IsInteraction = false;
    }
}
