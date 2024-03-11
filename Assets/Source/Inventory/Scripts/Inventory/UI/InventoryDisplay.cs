using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] protected MouseItemData MouseInventoryItem;
    [SerializeField] protected InventoryDescription InventoryDescription;
    [SerializeField] protected InventoryOperator InventoryOperator;
    [SerializeField] protected InventorySlotUI[] Slots;
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private bool _isCanAddItem = true;

    protected InventorySystem _inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;

    public event UnityAction<InventorySlotUI> OnSlotSelected;

    public abstract void AssingSlots(InventorySystem inventoryToDisplay, int offSet);
    public abstract void HandleSwap(InventorySlotUI inventorySlotUI);

    protected virtual void OnEnable()
    {
        if (_craftingHandler != null)
            _craftingHandler.OnInventoryUpdated += UpdateSlots;

        foreach (var slot in Slots)
        {
            slot.OnItemClicked += HandleItemSelection;
            slot.OnItemBeginDrag += HandleBeginDrag;
            slot.OnItemDroppedOn += HandleSwap;
            slot.OnItemEndDrag += HandleEndDrag;
        }
    }

    protected virtual void OnDisable()
    {
        if (_craftingHandler != null)
            _craftingHandler.OnInventoryUpdated -= UpdateSlots;

        foreach (var slot in Slots)
        {
            slot.OnItemClicked -= HandleItemSelection;
            slot.OnItemBeginDrag -= HandleBeginDrag;
            slot.OnItemDroppedOn -= HandleSwap;
            slot.OnItemEndDrag -= HandleEndDrag;
        }
    }

    public void SetAddItem(bool isAdd)
    {
        _isCanAddItem = isAdd;
    }

    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        if (MouseInventoryItem.CurrentItemData != null && MouseInventoryItem.CurrentItemData &&
        (clickedUISlot.AllowedItemTypes == MouseInventoryItem.CurrentItemData.Type ||
         clickedUISlot.AllowedItemTypes == ItemType.None) && _isCanAddItem)
        {
            if (clickedUISlot.AssignedInventorySlot.ItemData == null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
                clickedUISlot.UpdateItem();
                ResetDraggedItem();
                return;
            }
            else if (clickedUISlot.AssignedInventorySlot.ItemData != null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData != null)
            {
                bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData;
                if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.Size))
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
                    clickedUISlot.UpdateItem();
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

    public void UpdateSlots()
    {
        foreach (var slot in Slots)
        {
            slot.UpdateItem();
        }
    }

    public void ResetSelection()
    {
        DeselectAllItems();
        InventoryDescription.Clear();
    }

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateItem();
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
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        MouseInventoryItem.Toggle(true);

        if (inventorySlotUI.AssignedInventorySlot.ItemData != null && MouseInventoryItem.InventorySlotUI.AssignedInventorySlot.ItemData == null)
        {
            if (isShiftPressed && inventorySlotUI.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                MouseInventoryItem.UpdateSlot(halfStackSlot);
                MouseInventoryItem.UpdateSlotUI(inventorySlotUI);
                inventorySlotUI.UpdateItem();
                return;
            }
            else
            {
                MouseInventoryItem.UpdateSlot(inventorySlotUI.AssignedInventorySlot);
                MouseInventoryItem.UpdateSlotUI(inventorySlotUI);
                inventorySlotUI.ClearItemEvent();
                inventorySlotUI.Clear();
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
            InventoryDescription.SetInfo(Slots[index]);
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
        MouseInventoryItem.Clear();
    }


    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        if (MouseInventoryItem.CurrentSlot.AssignedInventorySlot.ItemData == clickedUISlot.AssignedInventorySlot.ItemData || MouseInventoryItem.CurrentSlot.AssignedInventorySlot.ItemData != null)
            return;

        MouseInventoryItem.CurrentSlot.AssignedInventorySlot.AssignItem(clickedUISlot.AssignedInventorySlot);
        MouseInventoryItem.CurrentSlot.UpdateItem();
        clickedUISlot.ClearItemEvent();
        clickedUISlot.Clear();
        clickedUISlot.AssignedInventorySlot.AssignItem(MouseInventoryItem.InventorySlotUI.AssignedInventorySlot);
        clickedUISlot.UpdateItem();
        MouseInventoryItem.Clear();
        ResetDraggedItem();
    }
}
