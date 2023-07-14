using System;
using System.Collections.Generic;
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

    private bool _empty = true;

    public event Action<InventorySlotUI> OnItemClicked;
    public event Action<InventorySlotUI> OnItemDroppedOn;
    public event Action<InventorySlotUI> OnItemBeginDrag;
    public event Action<InventorySlotUI> OnItemEndDrag;
    public event Action<InventorySlotUI> OnRightMouseClick;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        CleanSlot();
        Deselect();
        _iconImage.preserveAspect = true;
    }

    public void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateUiSlot();
    }

    public void Deselect()
    {
        _borderImage.enabled = false;
    }

    public void Select()
    {
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
        _borderImage.enabled = !_borderImage.enabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_assignedInventorySlot.ItemData != null)
            {
                OnRightMouseClick?.Invoke(this);
            }
        }
        else
        {
            _borderImage.enabled = true;
            OnItemClicked?.Invoke(this);
        }
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
