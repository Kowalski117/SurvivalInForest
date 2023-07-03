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

    private InventoryItemData _inventoryItemData;

    public InventoryItemData ItemData => _inventoryItemData;

    public void Init(InventoryItemData itemData, int amount)
    {
        _inventoryItemData = itemData;
        _icon.sprite = itemData.Icon;
        _nameText.text = itemData.DisplayName;
        _amountText.text = amount.ToString();

        if(amount > 0 )
            _frame.color = _addItemColor;
        else
            _frame.color = _removeItemColor;
    }

    public void UpdateAmount(int amount)
    {
        _amountText.text = amount.ToString();
    }
}
