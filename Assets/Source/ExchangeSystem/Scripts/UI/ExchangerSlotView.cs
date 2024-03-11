using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangerSlotView : MonoBehaviour
{
    [SerializeField] private ItemView[] _itemsToExchangeView;
    [SerializeField] private ItemView _itemToReceiveView;
    [SerializeField] private Button _exchangeButton;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;
    [SerializeField] private TMP_Text _ratingText;
    [SerializeField] private GameObject _ratingPanel;

    private ExchangerItemData _itemSlot;
    private ItemsToExchange _currentItemToExchange;
    private ExchangedItem _currentItemToReceive;
    private PlayerInventoryHolder _inventoryHolder;
    private TradingRating _rating;
    private int _amount = 1;
    private int _index = 1;

    public event Action OnExchanged;

    public bool IsEmpty { get; private set; } = true;

    private void OnEnable()
    {
        _exchangeButton.onClick.AddListener(OnExchangeButtonClicked);
        _plusButton.onClick.AddListener(AddAmount);
        _minusButton.onClick.AddListener(RemoveAmount);
    }

    private void OnDisable()
    {
        _exchangeButton.onClick.RemoveListener(OnExchangeButtonClicked);
        _plusButton.onClick.RemoveListener(AddAmount);
        _minusButton.onClick.RemoveListener(RemoveAmount);
    }

    public void Init(ExchangerItemData shopSlot, PlayerInventoryHolder inventoryHolder, TradingRating tradingRating)
    {
        _itemSlot = shopSlot;
        _inventoryHolder = inventoryHolder;
        _rating = tradingRating;
        IsEmpty = false;
        InitItemsToExchange(_itemSlot);
        UpdateAll();
    }

    public void UpdateAll()
    {
        UpdateExchangedItems();
        UpdateAmount();
        UpdateRating();
    }

    private void OnExchangeButtonClicked()
    {
        foreach (var itemToExchange in _currentItemToExchange.Items)
        {
            if (_inventoryHolder.RemoveItem(itemToExchange.ItemData, itemToExchange.Amount * _amount))
            {
                _inventoryHolder.AddItem(_currentItemToReceive.ItemData, _currentItemToReceive.Amount * _amount, _currentItemToReceive.ItemData.Durability);
                _amount = _index;
                UpdateAmount();
                OnExchanged?.Invoke();
                break;
            }
        }
    }

    private void InitItemsToExchange(ExchangerItemData item)
    {
        _currentItemToExchange = item.ItemsToExchange[UnityEngine.Random.Range(0, item.ItemsToExchange.Length)];
        _currentItemToReceive = item.ItemsToReceive[UnityEngine.Random.Range(0, item.ItemsToReceive.Length)];
    }

    private void AddAmount()
    {
        _amount++;
        UpdateAmount();
    }

    private void RemoveAmount()
    {
        if (_amount > _index)
            _amount--;

        UpdateAmount();
    }

    private void UpdateAmount()
    {
        for (int i = 0; i < _itemsToExchangeView.Length; i++)
        {
            if (i < _currentItemToExchange.Items.Length)
                _itemsToExchangeView[i].UpdateAmount(_inventoryHolder, _currentItemToExchange.Items[i].Amount * _amount);
        }

        _itemToReceiveView.UpdateAmount(_inventoryHolder, _currentItemToReceive.Amount * _amount);
    }

    private void UpdateExchangedItems()
    {
        for (int i = 0; i < _itemsToExchangeView.Length; i++)
        {
            _itemsToExchangeView[i].gameObject.SetActive(true);

            if(i < _currentItemToExchange.Items.Length)
                _itemsToExchangeView[i].Init(_currentItemToExchange.Items[i], _inventoryHolder);
        }

        _itemToReceiveView.Init(_currentItemToReceive, _inventoryHolder);
    }

    private void UpdateRating()
    {
        _ratingText.text = _itemSlot.Rating.ToString();

        if(_itemSlot.Rating > _rating.CurrentRating)
            _ratingPanel.gameObject.SetActive(true);
        else
            _ratingPanel.gameObject.SetActive(false);
    }
}
