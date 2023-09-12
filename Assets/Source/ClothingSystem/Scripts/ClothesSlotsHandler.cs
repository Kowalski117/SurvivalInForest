using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ClothesSlotsHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private MouseItemData _mouseItemData;
    [SerializeField] private ProtectionValue _protectionValue;
    [SerializeField] private StaminaAttribute _stiminaAttribute;
    [SerializeField] private InventorySlotUI[] _slots;

    private List<ClothesItemData> _clothesItems = new List<ClothesItemData>();

    public event UnityAction OnInteractionBackpack;
    public event UnityAction OnRemoveBackpack;

    private void OnEnable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdate += AddClothesItems;
            slot.OnItemClear += RemoveClothesItems;
        }

        _inventoryHolder.OnClearItemSlot += UpdateClothesItems;
    }

    private void OnDisable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdate -= AddClothesItems;
            slot.OnItemClear -= RemoveClothesItems;
        }

        _inventoryHolder.OnClearItemSlot += UpdateClothesItems;
    }

    public void AddClothesItems(InventorySlotUI inventorySlotUI)
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
                _mouseItemData.CurrentSlot.UpdateUiSlot();
                inventorySlotUI.CleanSlot();
            }
            else
            {
                _clothesItems.Add(clothesItemData);
                _protectionValue.UpdateProtectionValue(clothesItemData.Protection);
                _stiminaAttribute.AddMaxValueStamina(clothesItemData.Boost);
            }

            if(clothesItemData.ClothingType == ClothingType.Backpack)
            {
                OnInteractionBackpack?.Invoke();
            }
        }
    }

    public void RemoveClothesItems(InventorySlot inventorySlotUI)
    {
        if(inventorySlotUI.ItemData != null)
        {
            RemoveItem(inventorySlotUI.ItemData);
        }
    }

    private void UpdateClothesItems(InventorySlot inventorySlot, InventoryItemData itemData)
    {
        foreach (var slotUI in _slots)
        {
            if(inventorySlot == slotUI.AssignedInventorySlot)
            {
                RemoveItem(itemData);
            }
        }
    }

    private void RemoveItem(InventoryItemData itemData)
    {
        if (itemData != null && itemData is ClothesItemData clothesItemData)
        {
            if (_clothesItems.Contains(clothesItemData))
            {
                _clothesItems.Remove(clothesItemData);
                _protectionValue.UpdateProtectionValue(-clothesItemData.Protection);
                _stiminaAttribute.AddMaxValueStamina(-clothesItemData.Boost);

                if (clothesItemData.ClothingType == ClothingType.Backpack)
                {
                    OnRemoveBackpack?.Invoke();
                }
            }
        }
    }
}
