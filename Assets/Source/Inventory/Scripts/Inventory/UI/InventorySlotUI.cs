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
    [SerializeField] public ItemType _allowedItemTypes = ItemType.None;

    private bool _empty = true;

    public event UnityAction<InventorySlotUI> OnItemUpdate;
    public event UnityAction<InventorySlotUI> OnItemClear;
    public static UnityAction<InventorySlotUI> OnItemRemove;

    public event Action<InventorySlotUI> OnItemClicked;
    public event Action<InventorySlotUI> OnItemDroppedOn;
    public event Action<InventorySlotUI> OnItemBeginDrag;
    public event Action<InventorySlotUI> OnItemEndDrag;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;
    public ItemType AllowedItemTypes => _allowedItemTypes;

    private void Awake()
    {
        CleanSlot();
        Deselect();
        _iconImage.preserveAspect = true;
    }

    //private void Update()
    //{
    //    if(_assignedInventorySlot.ItemData != null && _allowedItemTypes != ItemType.None)
    //        OnItemRemove?.Invoke(this);
    //}

    public void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateUiSlot();
    }

    public void Deselect()
    {
        if (_borderImage != null)
            _borderImage.enabled = false;
    }

    public void Select()
    {
        if (_borderImage != null)
            _borderImage.enabled = true;
    }

    public void UpdateUiSlot()
    {
        if (_assignedInventorySlot.ItemData != null)
        {
            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = _assignedInventorySlot.ItemData.Icon;
            _iconImage.color = Color.white;
            _empty = false;
        }
        else
        {
            CleanSlot();
        }

        if (_assignedInventorySlot.Size > 1)
            _itemCount.text = _assignedInventorySlot.Size.ToString();
        else
            _itemCount.text = "";
    }

    public void UpdateUiSlotEvent()
    {
        if (_assignedInventorySlot.ItemData != null)
        {
            OnItemUpdate?.Invoke(this);
        }
        else
        {
            CleanUiSlotEvent();
        }
    }
    
    public void CleanUiSlotEvent()
    {
        OnItemClear?.Invoke(this);
    }

    public void CleanSlot()
    {
        _assignedInventorySlot?.ClearSlot();
        _iconImage.sprite = null;
        _iconImage.color = Color.clear;
        _itemCount.text = "";
        _iconImage.gameObject.SetActive(false);
        _empty = true;
    }

    public void ToggleHighlight()
    {
        if(_borderImage != null)
            _borderImage.enabled = !_borderImage.enabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_borderImage != null)
            _borderImage.enabled = true;
        OnItemClicked?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_empty)
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

    public void OnDrag(PointerEventData eventData)
    {

    }
}
