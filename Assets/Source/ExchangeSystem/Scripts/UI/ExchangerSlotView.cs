using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangerSlotView : MonoBehaviour
{
    [SerializeField] private Image _itemSprite1;
    [SerializeField] private Image _itemSprite2;
    [SerializeField] private TMP_Text _itemName1;
    [SerializeField] private TMP_Text _itemName2;
    [SerializeField] private TMP_Text _itemAmount1;
    [SerializeField] private TMP_Text _itemAmount2;
    [SerializeField] private Button _exchangeButton;
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;

    [SerializeField] private ShopInventoryItem _shopSlot;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;

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

    public void Init(ShopInventoryItem shopSlot, PlayerInventoryHolder inventoryHolder)
    {
        _shopSlot = shopSlot;
        _inventoryHolder = inventoryHolder;

        _itemSprite1.sprite = shopSlot.ItemData1.Icon;
        _itemSprite2.sprite = shopSlot.ItemData2.Icon;

        _itemName1.text = shopSlot.ItemData1.DisplayName;
        _itemName2.text = shopSlot.ItemData2.DisplayName;

        UpdateAmount();
    }

    private void OnExchangeButtonClicked()
    {
        if (_inventoryHolder.RemoveInventory(_shopSlot.ItemData1, _shopSlot.Amount1 * _amount))
        {
            _inventoryHolder.RemoveInventory(_shopSlot.ItemData2, _shopSlot.Amount2 * _amount);
            Destroy(gameObject);
            _amount = 1;
        }
    }

    private void AddAmount()
    {
        _amount++;
        UpdateAmount();
    }

    private void RemoveAmount()
    {
        if(_amount > 1)
            _amount--;
        UpdateAmount();
    }

    private void UpdateAmount()
    {
        _itemAmount1.text = (_amount * _shopSlot.Amount1).ToString();
        _itemAmount2.text = (_amount * _shopSlot.Amount2).ToString();
    }
}
