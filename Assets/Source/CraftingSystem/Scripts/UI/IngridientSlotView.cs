using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngridientSlotView : MonoBehaviour
{
    [SerializeField] private Image _imageSprite;
    [SerializeField] private TMP_Text _itemCount;

    private InventoryItemData _data;
    private int _amount;


    public void Init(PlayerInventoryHolder playerInventory, InventoryItemData data, int amount)
    {
        _data = data;
        _amount = amount;

        _imageSprite.sprite = data.Icon;

        UpdateAmount(playerInventory);
    }

    public void UpdateAmount(PlayerInventoryHolder playerInventory)
    {
        _itemCount.text = $"{playerInventory.InventorySystem.GetItemCount(_data)} / {_amount}";
    }
}
