using UnityEngine;
using UnityEngine.Events;

public class ExchangeHandler : Raycast
{
    [SerializeField] private LayerMask LayerMask;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private Transform _containerForSlot;
    [SerializeField] private ExchangerItemList _exchangerItemList;
    [SerializeField] private ExchangerSlotView[] _exchangerSlots;
    [SerializeField] private TradingRating _tradingRating;
    [SerializeField] private TimeHandler _timeHandler;
    [SerializeField] private ExchangeWindow _exchangeWindow;

    private ExchangeKeeper _exchangeKeeper;

    public event UnityAction OnInteractionStarted;
    public event UnityAction OnInteractionFinished;
    public event UnityAction OnExchangedItem;

    private void Start()
    {
        InitSlots();
    }

    private void OnEnable()
    {
        foreach (var slotView in _exchangerSlots)
        {
            slotView.OnExchanged += ExchangeEvent;
        }

        _timeHandler.OnDayUpdate += SwapSlots;
    }

    private void OnDisable()
    {
        foreach (var slotView in _exchangerSlots)
        {
            slotView.OnExchanged -= ExchangeEvent;
        }

        _timeHandler.OnDayUpdate -= SwapSlots;
    }

    private void Update()
    {
        if (IsRayHittingSomething(LayerMask, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out ExchangeKeeper exchangeKeeper))
            {
                _exchangeKeeper = exchangeKeeper;
                if (_exchangeKeeper && !_exchangeWindow.IsShopOpen)
                {
                    _exchangeWindow.Open();

                    UpdateSlots();
                    OnInteractionStarted?.Invoke();
                }
            }
        }
        else
        {
            Clear();
        }
    }

    private void Clear()
    {
        if (_exchangeKeeper != null && _exchangeWindow.IsShopOpen)
        {
            _exchangeKeeper = null;
            _exchangeWindow.Close();
            OnInteractionFinished?.Invoke();
        }
    }

    private void InitSlots()
    {
        _exchangeWindow.SetTitleText(_exchangerItemList.Name);

        foreach (Transform child in _containerForSlot)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < _exchangerItemList.Items.Count; i++)
        {
            if (i < _exchangerSlots.Length)
            {
                _exchangerSlots[i].gameObject.SetActive(true);
                _exchangerSlots[i].Init(_exchangerItemList.Items[i], _inventoryHolder, _tradingRating);
            }
        }
    }

    private void UpdateSlots()
    {
        foreach (var slot in _exchangerSlots)
        {
            if(!slot.IsEmpty)
                slot.UpdateSlot();
        }
    }

    private void ExchangeEvent()
    {
        OnExchangedItem?.Invoke();
    }

    private void SwapSlots(int day)
    {
        InitSlots();
    }
}


