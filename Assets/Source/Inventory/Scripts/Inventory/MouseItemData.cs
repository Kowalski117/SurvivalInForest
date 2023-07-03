using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private InventorySlot _assignedInventorySlot;
    [SerializeField] private float _dropOffset = 0.5f;
    [SerializeField] private LayerMask _interactionLayer;
    [SerializeField] private InventoryPlayerInput _playerInput;

    private Transform _playerTransform;

    private InventorySlot _initialInventorySlot;

    public static UnityAction OnUpdatedSlots;
    public event UnityAction<InventorySlot> OnInteractItem;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemCount.text = "";

        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //ПЕРЕДЕЛАТЬ
    }

    private void Update()
    {
        if (_assignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    private void OnEnable()
    {
        _playerInput.OnSelectInventoryItem += MouseClick;
        _playerInput.OnToggleInventory += CleanMouseSlot;
    }

    private void OnDisable()
    {
        _playerInput.OnSelectInventoryItem -= MouseClick;
        _playerInput.OnToggleInventory -= CleanMouseSlot;
    }

    public void MouseClick()
    {
        if (_assignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (!IsPointerOverUIObject())
            {
                // Действия при зажатии левой кнопки мыши
                Instantiate(_assignedInventorySlot.ItemData.ItemPrefab, _playerTransform.position + _playerTransform.forward * _dropOffset, Quaternion.identity); //ПЕРЕДЕЛАТЬ

                if (_assignedInventorySlot.Size > 1)
                {
                    _assignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    CleanSlot();
                }
            }
        }
    }

    public void CleanMouseSlot()
    {
        if (_initialInventorySlot != null)
        {
            _initialInventorySlot.AssignItem(_assignedInventorySlot);
            _initialInventorySlot.UpdateInventorySlot(_assignedInventorySlot.ItemData, _assignedInventorySlot.Size);
        }
        else
        {
            _assignedInventorySlot.ClearSlot();
        }
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _itemCount.text = "";
        OnUpdatedSlots?.Invoke();
    }

    public void CleanSlot()
    {
        _assignedInventorySlot.ClearSlot();
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _itemCount.text = "";
        OnUpdatedSlots?.Invoke();
    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        _initialInventorySlot = inventorySlot;
        _assignedInventorySlot.AssignItem(inventorySlot);
        UpdateMouseSlot();
        OnUpdatedSlots?.Invoke();   
    }

    public void UpdateMouseSlot()
    {
        _itemSprite.sprite = _assignedInventorySlot.ItemData.Icon;
        _itemSprite.color = Color.white;
        _itemCount.text = _assignedInventorySlot.Size.ToString();
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, result);
        return result.Count > 0;
    }
}
