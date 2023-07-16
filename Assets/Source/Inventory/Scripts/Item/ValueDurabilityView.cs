using UnityEngine;
using UnityEngine.UI;

public class ValueDurabilityView : MonoBehaviour
{
    [SerializeField] private InventorySlotUI _itemSlot;
    [SerializeField] private Image _image;

    private void Update()
    {
        if(_itemSlot.AssignedInventorySlot.Durability > 0)
        {
            if (_itemSlot.AssignedInventorySlot.DurabilityPercent > 0)
            {
                _image.enabled = true;
            }
            else
            {
                _image.enabled = false;
            }

            _image.fillAmount = _itemSlot.AssignedInventorySlot.DurabilityPercent;
        }
        else
        {
            _image.enabled = false;
        }
    }
}
