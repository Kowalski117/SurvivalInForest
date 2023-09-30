using System;
using TMPro;
using UnityEngine;

public class ExchangeHandler : MonoBehaviour
{
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private ExchangerItemList _exchangerItemList;
    [SerializeField] private ExchangerSlotView[] _exchangerSlots;
    [SerializeField] private TMP_Text _nameText;

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
        if (!_inventoryHandler.IsInventoryOpen)
        {
            _isShopOpen = !_isShopOpen;
            exchangeKeeper.DistanceHandler.SetActive(_isShopOpen);

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
        else
        {
            _isShopOpen = false;
            exchangeKeeper.DistanceHandler.SetActive(_isShopOpen);
            _panel.gameObject.SetActive(false);
        }
    }

    private void CreateSlots()
    {
        _nameText.text = _exchangerItemList.Name;

        foreach (Transform child in _containerForSlot)
        {
            child.gameObject.SetActive(false);
        }

        var itemsHeld = _inventoryHolder.InventorySystem.GetAllItemsHeld();

        foreach (var exchangerItem in _exchangerItemList.Items)
        {
            bool canExchange = true;

            foreach (var itemToExchange in exchangerItem.ItemsToExchange)
            {
                if (!itemsHeld.TryGetValue(itemToExchange.ItemData, out int amountHeld) || amountHeld < itemToExchange.Amount)
                {
                    canExchange = false;
                    break;
                }
            }

            if (canExchange)
            {
                ExchangerSlotView exchangerSlot = Array.Find(_exchangerSlots, slot => !slot.gameObject.activeSelf);

                if (exchangerSlot != null)
                {
                    exchangerSlot.Init(exchangerItem, _inventoryHolder);
                    exchangerSlot.gameObject.SetActive(true);
                }
            }
        }
    }
}


