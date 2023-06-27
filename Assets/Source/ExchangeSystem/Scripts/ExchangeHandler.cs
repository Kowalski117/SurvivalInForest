using System.Collections.Generic;
using UnityEngine;

public class ExchangeHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private ExchangerItemList _exchangerItemList;
    [SerializeField] private ExchangerSlotView _exchangerSlotPrefab;

    [SerializeField] private Dictionary<InventoryItemData, int> _itemsHeld;

    private bool _isShopOpen = false;

    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ExchangeKeeper.OnExchangeDisplayRequested += DisplayShopWindow;
    }

    private void OnDisable()
    {
        ExchangeKeeper.OnExchangeDisplayRequested -= DisplayShopWindow;
    }

    private void DisplayShopWindow(ExchangeKeeper exchangeKeeper)
    {
        _isShopOpen = !_isShopOpen;

        if (_isShopOpen)
        {
            _panel.gameObject.SetActive(true);
            CreateSlots();
        }
        else
        {
            _panel.gameObject.SetActive(false);
        }
    }

    private void CreateSlots()
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
