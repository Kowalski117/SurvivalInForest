using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _playerInput;
    [SerializeField] private Canvas _canvas;

    private bool _isUpdateSlot = false;

    private InventorySlotUI _inventorySlotUI;
    private InventorySlotUI _currentSlot;
    private InventoryItemData _currentItemData;

    public static UnityAction OnUpdatedSlots;

    public InventorySlotUI InventorySlotUI => _inventorySlotUI;
    public InventorySlotUI CurrentSlot => _currentSlot;
    public InventoryItemData CurrentItemData => _currentItemData;

    private void Awake()
    {
        _inventorySlotUI = GetComponentInChildren<InventorySlotUI>();
        Toggle(false);
    }

    private void Update()
    {
        if (_inventorySlotUI.AssignedInventorySlot.ItemData != null && _isUpdateSlot)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
        //    if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera, out Vector2 position))
        //{
        //    transform.position = _canvas.transform.TransformPoint(position);
        //}
    }

    public void CleanSlot()
    {
        _currentSlot = null;
        _currentItemData = null;
        _isUpdateSlot = false;
        _inventorySlotUI.CleanSlot();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        _isUpdateSlot = true;
        _inventorySlotUI.AssignedInventorySlot.AssignItem(inventorySlot);
        UpdateMouseSlot();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateCurrentInventorySlot(InventorySlotUI inventorySlot)
    {
        _currentSlot = inventorySlot;
        _currentItemData = inventorySlot.AssignedInventorySlot.ItemData;
    }

    public void UpdateMouseSlot()
    {
        _inventorySlotUI.UpdateUiSlot();
    }

    public void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, result);
        return result.Count > 0;
    }
}