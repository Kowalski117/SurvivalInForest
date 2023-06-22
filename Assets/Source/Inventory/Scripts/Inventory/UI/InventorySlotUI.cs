using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _imageSprite;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private GameObject _slotHighlight;
    [SerializeField] private InventorySlot _assignedInventorySlot;

    private Button _button;

    public InventoryDisplay ParentDisplay { get; private set; }

    public static UnityAction<InventorySlot> OnInteract;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        _button = GetComponent<Button>();
        CleanSlot();
        _imageSprite.preserveAspect = true;
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnUISlotClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnUISlotClick);
    }

    public void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateUiSlot();
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            _imageSprite.sprite = slot.ItemData.Icon;
            _imageSprite.color = Color.white;
        }
        else
        {
            CleanSlot();
        }

        if (slot.Size > 1)
            _itemCount.text = slot.Size.ToString();
        else
            _itemCount.text = "";
    }

    public void UpdateUiSlot()
    {
        if (_assignedInventorySlot != null)
            UpdateUISlot(_assignedInventorySlot);

    }

    public void CleanSlot()
    {
        _assignedInventorySlot?.ClearSlot();
        _imageSprite.sprite = null;
        _imageSprite.color = Color.clear;
        _itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void ToggleHighlight()
    {
        _slotHighlight.SetActive(!_slotHighlight.activeInHierarchy);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(_assignedInventorySlot.ItemData != null)
            {
                OnInteract?.Invoke(_assignedInventorySlot);
            }
        }
    }
}
