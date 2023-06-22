using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;

    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private ExchangerItemList _exchangerItemList;
    [SerializeField] private ExchangerSlotView _exchangerSlotPrefab;

    [SerializeField] private Dictionary<InventoryItemData, int> _itemsHeld;

    public void CreateSlots(/*ExchangeKeeper exchangeKeeper*/)
    {
        foreach (Transform child in _containerForSlot)
        {
            Destroy(child.gameObject);
        }

        _itemsHeld = _inventoryHolder.InventorySystem.GetAllItemsHeld();

        foreach (var item in _itemsHeld)
        {
            ExchangerSlotView exchangerSlot = Instantiate(_exchangerSlotPrefab, _containerForSlot);
            foreach (var itemList in _exchangerItemList.Items)
            {
                if (itemList.ItemData1 == item.Key)
                    exchangerSlot.Init(itemList, _inventoryHolder);
            }
        }
    }
}
