using UnityEngine;
using UnityEngine.Events;


public class MouseItemData : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _playerInput;
    [SerializeField] private Canvas _canvas;

    private InventorySlotUI _inventorySlotUI;
    private InventorySlotUI _currentSlot; 

    public static UnityAction OnUpdatedSlots;
    public event UnityAction<InventorySlot> OnInteractItem;

    public InventorySlotUI InventorySlotUI => _inventorySlotUI;
    public InventorySlotUI CurrentSlot => _currentSlot;

    private void Awake()
    {
        _inventorySlotUI = GetComponentInChildren<InventorySlotUI>();
        Toggle(false);
    }

    private void Update()
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera, out Vector2 position))
        {
            if (_inventorySlotUI.AssignedInventorySlot.ItemData != null)
            {
                transform.position = _canvas.transform.TransformPoint(position);
            }
        }
    }

    public void CleanSlot()
    {
        _inventorySlotUI.CleanSlot();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        _inventorySlotUI.AssignedInventorySlot.AssignItem(inventorySlot);
        UpdateMouseSlot();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateCurrentInventorySlot(InventorySlotUI inventorySlot)
    {
        _currentSlot = inventorySlot;
    }

    public void UpdateMouseSlot()
    {
        _inventorySlotUI.UpdateUiSlot();
    }

    public void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}