using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewInventoryNotifier : MonoBehaviour
{
    [SerializeField] private Image _frame;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Color _addItemColor;
    [SerializeField] private Color _removeItemColor;

    public void Init(InventoryItemData itemData, int amount)
    {
        _icon.sprite = itemData.Icon;
        _nameText.text = itemData.DisplayName;

        if(amount > 0)
        {
            _frame.color = _addItemColor;
            _amountText.text = "+" + amount.ToString();
        }
        else
        {
            _frame.color = _removeItemColor;
            _amountText.text = amount.ToString();
        }
    }

    public void UpdateAmount(int amount)
    {
        _amountText.text = amount.ToString();
    }
}
