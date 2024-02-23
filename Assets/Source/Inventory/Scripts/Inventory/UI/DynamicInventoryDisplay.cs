using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private AnimationUI _animationUI;

    public event UnityAction<int> OnDisplayRefreshed;

    public AnimationUI AnimationUI => _animationUI;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }

    public void Open()
    {
        //_canvasGroup.alpha = 1;
        //_canvasGroup.blocksRaycasts = true;
        _animationUI.OpenAnimation();
    }

    public void Close()
    {
        //_canvasGroup.alpha = 0;
        //_canvasGroup.blocksRaycasts = false;
        _animationUI.CloseAnimation();
    }

    public void RefreshDynamicInventory(InventorySystem inventoryToSystem, int offSet)
    {
        ClearSlots();
        _inventorySystem = inventoryToSystem;

        if (_inventorySystem != null)
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;

        OnDisplayRefreshed?.Invoke(_inventorySystem.InventorySize);
        AssingSlot(inventoryToSystem, offSet);
    }

    public override void AssingSlot(InventorySystem inventoryToDisplay, int offSet)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < Slots.Length; i++)
        {
            if (i < inventoryToDisplay.InventorySize)
            {
                InventorySlotUI viewSlot = Slots[i];
                slotDictionary.Add(viewSlot, inventoryToDisplay.InventorySlots[i + offSet]);
                viewSlot.gameObject.SetActive(true);
                viewSlot.Init(inventoryToDisplay.InventorySlots[i + offSet]);
                viewSlot.UpdateUiSlot();

                Slots[i].OnItemClicked += HandleItemSelection;
                Slots[i].OnItemBeginDrag += HandleBeginDrag;
                Slots[i].OnItemDroppedOn += HandleSwap;
                Slots[i].OnItemEndDrag += HandleEndDrag;
            }
            else
            {
                Slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void ClearSlots()
    {
        foreach (var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }

        if (slotDictionary != null)
            slotDictionary.Clear();
    }

    public override void HandleSwap(InventorySlotUI inventorySlotUI)
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
}
