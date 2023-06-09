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
    private Camera _camera;

    public static UnityAction OnUpdatedSlots;
    public event UnityAction<InventorySlot> OnInteractItem;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        _camera = Camera.main;
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
    }

    private void OnDisable()
    {
        _playerInput.OnSelectInventoryItem -= MouseClick;
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

    //private bool IsRayHittingSomething(LayerMask layerMask, out RaycastHit hitInfo)
    //{
    //    Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //    return Physics.Raycast(ray, out hitInfo, layerMask);
    //}
}
