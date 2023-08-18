using UnityEngine;
using UnityEngine.UI;

public class ExchangerSlotView : MonoBehaviour
{
    [SerializeField] private ExchangedItemView[] _itemsToExchangeView;
    [SerializeField] private ExchangedItemView[] _itemsToReceiveView;
    [SerializeField] private Button _exchangeButton;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;

    private ExchangerInventoryItem _shopSlot;
    private PlayerInventoryHolder _inventoryHolder;
    private int _amount = 1;

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

    public void Init(ExchangerInventoryItem shopSlot, PlayerInventoryHolder inventoryHolder)
    {
        _shopSlot = shopSlot;
        _inventoryHolder = inventoryHolder;

        UpdateExchangedItems(_itemsToExchangeView, shopSlot.ItemsToExchange);
        UpdateExchangedItems(_itemsToReceiveView, shopSlot.ItemsToReceive);
        UpdateAmount();
    }

    private void OnExchangeButtonClicked()
    {
        foreach (var itemToExchange in _shopSlot.ItemsToExchange)
        {
            if (_inventoryHolder.RemoveInventory(itemToExchange.ItemData, itemToExchange.Amount * _amount))
            {
                foreach (var itemToReceive in _shopSlot.ItemsToReceive)
                {
                    _inventoryHolder.AddToInventory(itemToReceive.ItemData, itemToReceive.Amount * _amount, itemToReceive.ItemData.Durability);
                }

                gameObject.SetActive(false);
                _amount = 1;
                break;
            }
        }
    }

    private void AddAmount()
    {
        _amount++;
        UpdateAmount();
    }

    private void RemoveAmount()
    {
        if (_amount > 1)
            _amount--;
        UpdateAmount();
    }

    private void UpdateAmount()
    {
        foreach (var itemToExchangeView in _itemsToExchangeView)
        {
            itemToExchangeView.UpdateAmount(_amount);
        }

        foreach (var itemToReceiveView in _itemsToReceiveView)
        {
            itemToReceiveView.UpdateAmount(_amount);
        }
    }

    private void UpdateExchangedItems(ExchangedItemView[] itemViewArray, ExchangedItem[] itemArray)
    {
        for (int i = 0; i < itemViewArray.Length; i++)
        {
            if (i < itemArray.Length)
            {
                itemViewArray[i].gameObject.SetActive(true);
                itemViewArray[i].Init(itemArray[i]);
            }
            else
            {
                itemViewArray[i].gameObject.SetActive(true);
                itemViewArray[i].Clear();
            }
        }
    }
}
