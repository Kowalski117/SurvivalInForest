using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothesSlotsHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private MouseItemData _mouseItemData;
    [SerializeField] private Protection _protectionValue;
    [SerializeField] private StaminaAttribute _stiminaAttribute;
    [SerializeField] private InventorySlotUI[] _slots;

    private List<ClothesItemData> _clothesItems = new List<ClothesItemData>();

    public event Action OnBackpackInteractioned;
    public event Action<ClothesItemData> OnClothesAdded;
    public event Action<ClothesItemData> OnClothesRemoved;
    public event Action OnBackpackRemoved;

    private void OnEnable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdated += AddItems;
            slot.OnItemCleared += RemoveSlot;
        }

        _inventoryHolder.OnItemSlotCleared += UpdateItems;
    }

    private void OnDisable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdated -= AddItems;
            slot.OnItemCleared -= RemoveSlot;
        }

        _inventoryHolder.OnItemSlotCleared -= UpdateItems;
    }

    public void AddItems(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.AssignedInventorySlot.ItemData != null && inventorySlotUI.AssignedInventorySlot.ItemData is ClothesItemData clothesItemData)
        {
            bool isClothesAlreadyAdded = _clothesItems.Any(clothes => clothes.ClothingType == clothesItemData.ClothingType);

            foreach (var item in _clothesItems)
            {
                if (item == clothesItemData)
                    return;
            }

            if (isClothesAlreadyAdded)
            {
                _mouseItemData.CurrentSlot.AssignedInventorySlot.AssignItem(inventorySlotUI.AssignedInventorySlot);
                _mouseItemData.CurrentSlot.UpdateItem();
                inventorySlotUI.Clear();
            }
            else
            {
                _clothesItems.Add(clothesItemData);
                _protectionValue.UpdateValue(clothesItemData.Protection);
                _stiminaAttribute.AddMaxValue(clothesItemData.Boost);
                OnClothesAdded?.Invoke(clothesItemData);
            }

            if(clothesItemData.ClothingType == ClothingType.Backpack)
                OnBackpackInteractioned?.Invoke();
        }
    }

    public void RemoveSlot(InventorySlot inventorySlotUI)
    {
        if(inventorySlotUI.ItemData != null)
            RemoveItem(inventorySlotUI.ItemData);
    }

    private void UpdateItems(InventorySlot inventorySlot, InventoryItemData itemData)
    {
        foreach (var slotUI in _slots)
        {
            if(inventorySlot == slotUI.AssignedInventorySlot)
                RemoveItem(itemData);
        }
    }

    private void RemoveItem(InventoryItemData itemData)
    {
        if (itemData != null && itemData is ClothesItemData clothesItemData)
        {
            if (_clothesItems.Contains(clothesItemData))
            {
                _clothesItems.Remove(clothesItemData);
                _protectionValue.UpdateValue(-clothesItemData.Protection);
                _stiminaAttribute.AddMaxValue(-clothesItemData.Boost);

                if (clothesItemData.ClothingType == ClothingType.Backpack)
                    OnBackpackRemoved?.Invoke();

                OnClothesRemoved?.Invoke(clothesItemData);
            }
        }
    }
}
