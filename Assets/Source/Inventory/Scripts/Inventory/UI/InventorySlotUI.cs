using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _borderImage;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private InventorySlot _assignedInventorySlot;
    [SerializeField] private ItemType _allowedItemTypes;
    [SerializeField] private bool _isMouseSlot = false;

    private bool _isEmpty = true;

    public event UnityAction<InventorySlotUI> OnItemUpdated;
    public event UnityAction<InventorySlot> OnItemCleared;
    public static UnityAction<InventorySlotUI> OnItemRemoved;

    public event Action<InventorySlotUI> OnItemClicked;
    public event Action<InventorySlotUI> OnItemDroppedOn;
    public event Action<InventorySlotUI> OnItemBeginDrag;
    public event Action<InventorySlotUI> OnItemEndDrag;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;
    public ItemType AllowedItemTypes => _allowedItemTypes;

    private void Awake()
    {
        Clear();
        TurnOffHighlight();
        _iconImage.preserveAspect = false;
    }

    private void Update()
    {
        if (_assignedInventorySlot.ItemData != null && _assignedInventorySlot.ItemData.Type != _allowedItemTypes && _allowedItemTypes != ItemType.None && !_isMouseSlot)
            CanDropItem();
    }

    public void CanDropItem()
    {
        if (_allowedItemTypes == ItemType.None)
            return;

        if(_assignedInventorySlot.ItemData.Type == _allowedItemTypes) 
            return;
        else
            OnItemRemoved?.Invoke(this);
    }

    public void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateItem();
    }

    public void UpdateItem()
    {
        if (_assignedInventorySlot.ItemData != null)
        {
            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = _assignedInventorySlot.ItemData.Icon;
            _iconImage.color = Color.white;
            _isEmpty = false;
            _iconImage.preserveAspect = false;
        }
        else
            Clear();

        if (_assignedInventorySlot.Size > 1)
            _itemCount.text = _assignedInventorySlot.Size.ToString();
        else
            _itemCount.text = "";

        UpdateItemEvent();
    }

    private void UpdateItemEvent()
    {
        if (_assignedInventorySlot.ItemData == null)
            ClearItemEvent();

        OnItemUpdated?.Invoke(this);
    }
    
    public void ClearItemEvent()
    {
        OnItemCleared?.Invoke(this._assignedInventorySlot);
    }

    public void Clear()
    {
        _assignedInventorySlot?.ClearSlot();
        _iconImage.sprite = null;
        _iconImage.color = Color.clear;
        _itemCount.text = "";
        _iconImage.gameObject.SetActive(false);
        _isEmpty = true;
    }

    public void ToggleHighlight()
    {
        if(_borderImage != null)
            _borderImage.enabled = !_borderImage.enabled;
    }

    public void TurnOffHighlight()
    {
        if (_borderImage != null && _borderImage.enabled == true)
            _borderImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_borderImage != null)
            _borderImage.enabled = true;
        OnItemClicked?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEmpty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData) { }
}
