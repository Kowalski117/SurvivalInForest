using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _playerInput;
    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private Canvas _canvas;

    private bool _isUpdateSlot = false;
    private float _delay = 0.05f;
    private Coroutine _coroutine;
    private CanvasGroup _canvasGroup;

    private InventorySlotUI _inventorySlotUI;
    private InventorySlotUI _previousSlot;
    private InventoryItemData _currentItemData;

    public event UnityAction OnUpdatedSlots;
    public event UnityAction<InventoryItemData, int> OnItemDataChanged;

    public InventorySlotUI InventorySlotUI => _inventorySlotUI;
    public InventorySlotUI CurrentSlot => _previousSlot;
    public InventoryItemData CurrentItemData => _currentItemData;

    private void Awake()
    {
        _inventorySlotUI = GetComponentInChildren<InventorySlotUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Toggle(false);
    }

    private void Update()
    {
        if (_inventorySlotUI.AssignedInventorySlot.ItemData != null && _isUpdateSlot)
            transform.position = Mouse.current.position.ReadValue();
    }

    public void Clear()
    {
        _previousSlot = null;
        _currentItemData = null;
        _isUpdateSlot = false;
        _inventorySlotUI.Clear();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateSlot(InventorySlot inventorySlot)
    {
        _isUpdateSlot = true;
        _inventorySlotUI.AssignedInventorySlot.AssignItem(inventorySlot);
        UpdateSlot();
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateSlotUI(InventorySlotUI inventorySlot)
    {
        _previousSlot = inventorySlot;
        _previousSlot.TurnOffHighlight();
        _currentItemData = inventorySlot.AssignedInventorySlot.ItemData;
    }

    public void TurnOffPreviousSlot()
    {
        if(_previousSlot)
            _previousSlot.TurnOffHighlight();
    }

    public void UpdateSlot()
    {
        _inventorySlotUI.UpdateItem();
    }

    public void Toggle(bool toggle)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SwitchActiveWithDelay(toggle));

        TurnOffPreviousSlot();
    }

    public void ReturnCurrentSlot()
    {
        if (_inventorySlotUI.AssignedInventorySlot.ItemData != null && _previousSlot != null)
        {
            if (IsPointerOverUIObject(GameConstants.Invetory))
            {
                _previousSlot.AssignedInventorySlot.AssignItem(_inventorySlotUI.AssignedInventorySlot);
                _previousSlot.UpdateItem();
            }
            else
            {
                OnItemDataChanged?.Invoke(_inventorySlotUI.AssignedInventorySlot.ItemData, -_inventorySlotUI.AssignedInventorySlot.Size);
                _inventoryOperator.RemoveItems(_inventorySlotUI, _inventorySlotUI.AssignedInventorySlot.Size);
            }

            Toggle(false);
            Clear();
        }
    }

    public bool IsPointerOverUIObject(string tagToCheck)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, result);

        foreach (RaycastResult raycastResult in result)
        {
            if (raycastResult.gameObject.CompareTag(tagToCheck))
                return true;
        }

        return false;
    }

    private IEnumerator SwitchActiveWithDelay(bool isActive)
    {
        yield return new WaitForSeconds(_delay);
        _canvasGroup.alpha = isActive ? 1 : 0;
        _canvasGroup.blocksRaycasts = isActive;
    }
}