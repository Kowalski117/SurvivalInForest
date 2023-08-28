using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductSlotView : MonoBehaviour
{
    [SerializeField] private Image _imageSprite;
    [SerializeField] private TMP_Text _itemCount;

    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public void Init(InventoryItemData data, int amount)
    {
        _imageSprite.sprite = data.Icon;
        _itemCount.text = amount.ToString();

        _isBusy = true;
    }
}
