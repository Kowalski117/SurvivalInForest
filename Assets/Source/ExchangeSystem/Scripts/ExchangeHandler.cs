using System;
using TMPro;
using UnityEngine;

public class ExchangeHandler : Raycast
{
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private ExchangerItemList _exchangerItemList;
    [SerializeField] private ExchangerSlotView[] _exchangerSlots;
    [SerializeField] private TradingRating _tradingRating;
    [SerializeField] private TMP_Text _nameText;

    private bool _isShopOpen = false;
    private ExchangeKeeper _exchangeKeeper;

    public event Action OnInteractionStarted;
    public event Action OnInteractionFinished;

    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inventoryHandler.OnInventoryClosed += CloseWindow;
    }

    private void OnDisable()
    {
        _inventoryHandler.OnInventoryClosed -= CloseWindow;
    }

    private void Update()
    {
        if (IsRayHittingSomething(LayerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ExchangeKeeper exchangeKeeper))
            {
                _exchangeKeeper = exchangeKeeper;
                if (_exchangeKeeper && !_isShopOpen)
                {
                    _panel.gameObject.SetActive(true);
                    CreateSlots();
                    _isShopOpen = true;
                    OnInteractionStarted?.Invoke();
                }
            }
        }
        else
        {
            Close();
        }
    }

    private void Close()
    {
        if (_exchangeKeeper != null && _isShopOpen)
        {
            _exchangeKeeper = null;
            CloseWindow();
            OnInteractionFinished?.Invoke();
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
                    exchangerSlot.Init(exchangerItem, _inventoryHolder, _tradingRating);
                    exchangerSlot.gameObject.SetActive(true);
                }
            }
        }
    }

    private void CloseWindow()
    {
        _panel.gameObject.SetActive(false);
        _isShopOpen = false;
    }
}


