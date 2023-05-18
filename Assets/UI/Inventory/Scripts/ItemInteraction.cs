using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField] private int _distance;
    [SerializeField] private InventorySystem _inventorySystem;
    [SerializeField] private SelectionPlayerInput _selectionPlayerInput;

    private Camera _camera;
    private ItemObject _objectHit;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _selectionPlayerInput.PickUp += PickUp;
    }

    private void OnDisable()
    {
        _selectionPlayerInput.PickUp -= PickUp;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _distance))
        {
            _objectHit = hit.collider.GetComponent<ItemObject>();
        }
        else
        {
            _objectHit = null;
        }
    }

    private void PickUp()
    {
        if (_objectHit != null)
            _inventorySystem.PickUpItem(_objectHit);
    }
}
