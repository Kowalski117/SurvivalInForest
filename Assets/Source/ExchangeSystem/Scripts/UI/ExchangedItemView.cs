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
        _itemSprite.sprite = item.ItemData.Icon;
        _itemName.text = item.ItemData.DisplayName;
        UpdateAmount(item.Amount);
    }

    public void UpdateAmount(int amount)
    {
        _itemAmount.text = amount.ToString();
    }
}

