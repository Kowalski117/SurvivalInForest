using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemAmount;
    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;
    [SerializeField] private bool _isReceiveSlot;

    private ExchangedItem _exchangedItem;

    public ExchangedItem ExchangedItem => _exchangedItem;

    public void Init(ExchangedItem item, PlayerInventoryHolder playerInventoryHolder)
    {
        if (item.ItemData)
        {
            _exchangedItem = item;
            _itemSprite.enabled = true;
            _itemSprite.sprite = _exchangedItem.ItemData.Icon;
            UpdateAmount(playerInventoryHolder, _exchangedItem.Amount);
        }
        else
        {
            Clear();
        }
    }

    public void UpdateAmount(PlayerInventoryHolder playerInventory, int amount)
    {
        if(_isReceiveSlot)
        {
            _itemAmount.text = amount.ToString();
        }
        else
        {
            _itemAmount.text = $"{playerInventory.InventorySystem.GetItemCount(_exchangedItem.ItemData)}/{amount}";

            if (playerInventory.InventorySystem.GetItemCount(_exchangedItem.ItemData) < amount)
                _itemAmount.color = _negativeColor;
            else
                _itemAmount.color = _positiveColor;
        }
    }

    public void Clear()
    {
        _itemSprite.enabled = false;
        _itemSprite.sprite = null;
        _itemAmount.text = "";
    }
}

