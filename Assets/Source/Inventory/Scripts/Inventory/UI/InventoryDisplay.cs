using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] protected MouseItemData MouseInventoryItem;
    [SerializeField] protected InventoryDescriptionUI InventoryDescription;
    [SerializeField] protected InventorySlotUI[] Slots;

    private int _currentlyDraggedItemIndex = -1;
    private InventorySlotUI _currentSlot;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public InventorySlotUI[] SlotsUI => Slots;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
    }

    public abstract void AssingSlot(InventorySystem inventoryToDisplay, int offSet);

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
        if(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null && _currentSlot != null)
        {
            _currentSlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
            _currentSlot.UpdateUiSlot();
            MouseInventoryItem.CleanSlot();
            ResetDraggedItem();
        }
    }

    protected void HandleSwap(InventorySlotUI inventorySlotUI)
    {
        int index = Array.IndexOf(Slots, inventorySlotUI);


        if (index == -1)
        {
            return;
        }
;
        SlotClicked(inventorySlotUI);
        HandleItemSelection(inventorySlotUI);
    }

    protected void HandleBeginDrag(InventorySlotUI inventorySlotUI)
    {
        int index = Array.IndexOf(Slots, inventorySlotUI);

        if (index == -1)
            return;

        _currentlyDraggedItemIndex = index;
        HandleItemSelection(inventorySlotUI);
        if (Slots[index].AssignedInventorySlot.ItemData != null)
        {
            CreateDraggedItem(Slots[index]);
        }
    }

    private void CreateDraggedItem(InventorySlotUI inventorySlotUI)
    {
        //int index = Array.IndexOf(Slots, inventorySlotUI);
        //MouseInventoryItem.CleanSlot();
        //MouseInventoryItem.UpdateMouseSlot(inventorySlotUI.AssignedInventorySlot);

        //_currentSlot = Slots[index];
        //Slots[index].CleanSlot();

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
            Slots[index].Select();
            InventoryDescription.SetDescription(Slots[index]);
        }
    }

    public void ResetSelection()
    {
        DeselectAllItems();
        InventoryDescription.ResetDescription();
    }

    private void DeselectAllItems()
    {
        foreach (var slotUI in Slots)
        {
            slotUI.Deselect();
        }
    }

    private void ResetDraggedItem()
    {
        _currentlyDraggedItemIndex = -1;
        MouseInventoryItem.Toggle(false);
        MouseInventoryItem.CleanSlot();
    }

    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
            clickedUISlot.UpdateUiSlot();
            ResetDraggedItem();
            return;
        }

        if (clickedUISlot.AssignedInventorySlot.ItemData != null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
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
                if (leftInStack < 1)
                {
                    SwapSlots(clickedUISlot);
                }
                else
                {
                    int remainingOnMouse = MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.Size - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUiSlot();

                    var newItem = new InventorySlot(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData, remainingOnMouse, clickedUISlot.AssignedInventorySlot.Durability);
                    MouseInventoryItem.CleanSlot();
                    MouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            else if (!isSameItem)
            {
                Debug.Log("1");
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        //int clickedIndex = Array.IndexOf(Slots, clickedUISlot);
        //if (clickedIndex == -1)
        //    return;

        //if (_currentlyDraggedItemIndex == clickedIndex)
        //{
        //    clickedUISlot.ToggleHighlight();
        //    return;
        //}

        MouseInventoryItem.CurrentSlot.AssignedInventorySlot.AssignItem(clickedUISlot.AssignedInventorySlot);
        MouseInventoryItem.CurrentSlot.UpdateUiSlot();

        clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
        clickedUISlot.UpdateUiSlot();
        MouseInventoryItem.CleanSlot();
        ResetDraggedItem();
    }
}
