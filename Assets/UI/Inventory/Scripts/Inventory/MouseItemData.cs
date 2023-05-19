using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private InventorySlot _assignedInventorySlot;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        _itemSprite.color = Color.clear;
        _itemCount.text = "";
    }

    private void Update()
    {
        if(_assignedInventorySlot.ItemData !=  null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if(Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                CleanSlot();
            }
        }
    }

    public void CleanSlot()
    {
        _assignedInventorySlot.CleanSlot();
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _itemCount.text = "";

    }

    public void UpdateMouseSlot(InventorySlot inventorySlot)
    {
        _assignedInventorySlot.AssignItem(inventorySlot);
        _itemSprite.sprite = inventorySlot.ItemData.Icon;
        _itemSprite.color = Color.white;
        _itemCount.text = inventorySlot.StackSize.ToString();
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
