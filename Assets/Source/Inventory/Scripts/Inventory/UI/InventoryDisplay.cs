using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] protected MouseItemData MouseInventoryItem;
    [SerializeField] protected InventoryDescriptionUI InventoryDescription;
    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] protected InventorySlotUI[] Slots;

    private string _invetoryTag = "Inventory";

    private InventorySlotUI _currentSlot;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;

    public event UnityAction<InventorySlotUI> OnSlotSelected;

    public InventorySystem InventorySystem => inventorySystem;
    public InventorySlotUI[] SlotsUI => Slots;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => slotDictionary;

    public abstract void AssingSlot(InventorySystem inventoryToDisplay, int offSet);
    public abstract void HandleSwap(InventorySlotUI inventorySlotUI);

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        if (MouseInventoryItem.CurrentItemData != null &&
        (clickedUISlot.AllowedItemTypes == MouseInventoryItem.CurrentItemData.Type ||
         clickedUISlot.AllowedItemTypes == ItemType.None))
        {
            if (clickedUISlot.AssignedInventorySlot.ItemData == null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
                clickedUISlot.UpdateUiSlot();
                ResetDraggedItem();
                return;
            }
            else if (clickedUISlot.AssignedInventorySlot.ItemData != null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
            {
                bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData;
                if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.Size))
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
                    clickedUISlot.UpdateUiSlot();
                    ResetDraggedItem();
                    return;
                }
                else if (isSameItem && !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.Size, out int leftInStack))
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
                else if (!isSameItem)
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
            }
        }
    }

    public void ResetSelection()
    {
        DeselectAllItems();
        InventoryDescription.ResetDescription();
    }

    public void ReturnCurrentSlot()
    {
        if (MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null && _currentSlot != null)
        {
            if (MouseInventoryItem.IsPointerOverUIObject(_invetoryTag))
            {
                _currentSlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
                _currentSlot.UpdateUiSlot();
            }
            else
            {
                _inventoryOperator.RemoveItem(MouseInventoryItem.InventorySlotUI);
            }

            MouseInventoryItem.CleanSlot();
            ResetDraggedItem();
        }
    }

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUiSlot();
            }
        }
    }

    protected void HandleEndDrag(InventorySlotUI inventorySlotUI)
    {
        MouseInventoryItem.ReturnCurrentSlot();
    }

    protected void HandleBeginDrag(InventorySlotUI inventorySlotUI)
    {
        int index = Array.IndexOf(Slots, inventorySlotUI);

        if (index == -1)
            return;

        HandleItemSelection(inventorySlotUI);
        if (Slots[index].AssignedInventorySlot.ItemData != null)
        {
            CreateDraggedItem(Slots[index]);
        }
    }

    private void CreateDraggedItem(InventorySlotUI inventorySlotUI)
    {
        MouseInventoryItem.Toggle(true);
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if (inventorySlotUI.AssignedInventorySlot.ItemData != null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData == null)
        {
            if (isShiftPressed && inventorySlotUI.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                MouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                MouseInventoryItem.UpdateCurrentInventorySlot(inventorySlotUI);
                inventorySlotUI.UpdateUiSlot();
                _currentSlot = inventorySlotUI;
                return;
            }
            else
            {
                MouseInventoryItem.UpdateMouseSlot(inventorySlotUI.AssignedInventorySlot);
                MouseInventoryItem.UpdateCurrentInventorySlot(inventorySlotUI);
                _currentSlot = inventorySlotUI;
                inventorySlotUI.CleanUiSlotEvent();
                inventorySlotUI.CleanSlot();
                return;
            }
        }
    }

    protected void HandleItemSelection(InventorySlotUI inventorySlotUI)
    {
        ResetSelection();

        int index = Array.IndexOf(Slots, inventorySlotUI);

        if (index == -1)
            return;

        if (Slots[index].AssignedInventorySlot.ItemData != null)
        {
            InventoryDescription.SetDescription(Slots[index]);
            OnSlotSelected?.Invoke(inventorySlotUI);
        }
    }

    private void DeselectAllItems()
    {
        foreach (var slotUI in Slots)
        {
            slotUI.TurnOffHighlight();
        }
    }

    private void ResetDraggedItem()
    {
        MouseInventoryItem.Toggle(false);
        MouseInventoryItem.CleanSlot();
    }


    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        if (MouseInventoryItem.CurrentSlot.AssignedInventorySlot.ItemData == clickedUISlot.AssignedInventorySlot.ItemData || MouseInventoryItem.CurrentSlot.AssignedInventorySlot.ItemData != null)
            return;

        MouseInventoryItem.CurrentSlot.AssignedInventorySlot.AssignItem(clickedUISlot.AssignedInventorySlot);
        MouseInventoryItem.CurrentSlot.UpdateUiSlot();
        clickedUISlot.CleanUiSlotEvent();
        clickedUISlot.CleanSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
        clickedUISlot.UpdateUiSlot();
        MouseInventoryItem.CleanSlot();
        ResetDraggedItem();
    }
}
