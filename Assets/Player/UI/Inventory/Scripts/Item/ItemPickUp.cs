using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private float pickUpRadius = 1f;
    [SerializeField] private InventoryItemData _itemData;

    private InventoryHolder _currentInventoryHolder;
    private SphereCollider _myCollider;
    private SelectionPlayerInput _playerInput;

    private void Awake()
    {
        //_playerInput = FindObjectOfType<SelectionPlayerInput>();
        _myCollider = GetComponent<SphereCollider>();
        _myCollider.isTrigger = true;
        _myCollider.radius = pickUpRadius;
    }

    private void OnEnable()
    {
        //_playerInput.PickUp += PickUp;
    }

    private void OnDisable()
    {
        //_playerInput.PickUp -= PickUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventoryHolder inventory))
        {
            if (!inventory)
                return;

            if (inventory.AddToInventory(_itemData, 1))
            {
                Destroy(this.gameObject); // ���������� ��� �����������
            }
            _currentInventoryHolder = inventory;
        }
    }

    private void PickUp()
    {

    }
}
