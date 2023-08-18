using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangedItemView : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemAmount;

    public void Init(ExchangedItem item)
    {
        _itemSprite.enabled = true;
        _itemSprite.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.DisplayName;
        UpdateAmount(item.Amount);
    }

    public void UpdateAmount(int amount)
    {
        if (_itemSprite.enabled == true)
            _itemAmount.text = amount.ToString();
    }

    public void Clear()
    {
        _itemSprite.enabled = false;
        _itemSprite.sprite = null;
        _itemName.text = "";
        _itemAmount.text = "";
    }
}

