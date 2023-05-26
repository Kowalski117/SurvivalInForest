using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private Image _image;
    [SerializeField] private SelectionPlayerInput _playerInput;
    [SerializeField] private float pickUpRadius = 1f;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDispleyRequested;

    private Camera _mainCamera;

    protected override  void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        SaveGameHandler.Data._playerInventory = new InventorySaveData(PrimaryInventorySystem); // поменять
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            OnPlayerInventoryDispleyRequested?.Invoke(PrimaryInventorySystem, Offset);
    }

    private void OnEnable()
    {
        _playerInput.PickUp += PickUpItem;
    }

    private void OnDisable()
    {
        _playerInput.PickUp -= PickUpItem;
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.PlayerInventory.InventorySystem != null)
        {
            this.PrimaryInventorySystem = data.PlayerInventory.InventorySystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    private void PickUpItem()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRadius))
        {
            ItemPickUp itemPickUp = hit.collider.GetComponent<ItemPickUp>();

            if (itemPickUp != null)
            {
                if (AddToInventory(itemPickUp.ItemData, 1))
                {
                    itemPickUp.PicUp();
                    return;
                }
            }
        }
    }
}

