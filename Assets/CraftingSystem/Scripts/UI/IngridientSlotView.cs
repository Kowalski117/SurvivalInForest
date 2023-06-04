using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngridientSlotView : MonoBehaviour
{
    [SerializeField] private Image _imageSprite;
    [SerializeField] private TMP_Text _itemCount;

    public void Init(InventoryItemData data, int amount)
    {
        _imageSprite.sprite = data.Icon;
        _itemCount.text= amount.ToString();
    }
}
