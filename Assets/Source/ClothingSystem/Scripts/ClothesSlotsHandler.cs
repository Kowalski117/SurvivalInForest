using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClothesSlotsHandler : MonoBehaviour
{
    [SerializeField] private MouseItemData _mouseItemData;
    [SerializeField] private ProtectionValue _protectionValue;
    [SerializeField] private StaminaAttribute _stiminaAttribute;
    [SerializeField] private InventorySlotUI[] _slots;

    private List<ClothesItemData> _clothesItems = new List<ClothesItemData>();

    private void OnEnable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdate += AddClothesItems;
            slot.OnItemClear += RemoveClothesItems;
        }
    }

    private void OnDisable()
    {
        foreach (var slot in _slots)
        {
            slot.OnItemUpdate -= AddClothesItems;
            slot.OnItemClear -= RemoveClothesItems;
        }
    }

    public void AddClothesItems(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.AssignedInventorySlot.ItemData != null && inventorySlotUI.AssignedInventorySlot.ItemData is ClothesItemData clothesItemData)
        {
            bool isClothesAlreadyAdded = _clothesItems.Any(clothes => clothes.ClothingType == clothesItemData.ClothingType);

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
        }
    }


    public void RemoveClothesItems(InventorySlotUI inventorySlotUI)
    {
        if (inventorySlotUI.AssignedInventorySlot.ItemData != null && inventorySlotUI.AssignedInventorySlot.ItemData is ClothesItemData clothesItemData)
        {
            if (_clothesItems.Contains(clothesItemData))
            {
                _clothesItems.Remove(clothesItemData);
                _protectionValue.UpdateProtectionValue(-clothesItemData.Protection);
                _stiminaAttribute.AddMaxValueStamina(-clothesItemData.Boost);
            }
        }
    }
}
