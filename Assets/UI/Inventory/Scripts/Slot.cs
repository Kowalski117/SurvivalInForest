using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Items _itemInSlot;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _amountInSlotText;
    [SerializeField] private int _amountInSlot;

    public Image Icon => _icon;
    public TMP_Text AmountInSlotText => _amountInSlotText;
    public Items ItemInSlot => _itemInSlot;
    public int AmountInSlot => _amountInSlot;

    public void SetStats()
    {
        bool hasItem = _itemInSlot != null && _amountInSlot > 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(hasItem);
        }

        if (hasItem)
        {
            _icon.sprite = _itemInSlot.Icon;
            _amountInSlotText.text = _amountInSlot.ToString();
            _icon.raycastTarget = true;
        }
        else
        {
            _itemInSlot = null;
            _icon.sprite = null;
            _icon.raycastTarget = false;
            _amountInSlotText.text = string.Empty;
        }
    }

    public void SetItemInSlot(Items items)
    {
        _itemInSlot = items;
    }

    public void AddAmount(int amount)
    {
        _amountInSlot += amount;
    }

    public void SetAmount(int amount)
    {
        _amountInSlot = amount;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Slot dropperSlot = eventData.pointerPress.GetComponent<Slot>();

        if (dropperSlot == this || dropperSlot.AmountInSlot == 0 && dropperSlot.ItemInSlot == null)
            return;

        if (dropperSlot != null )
        {
            if (dropperSlot.ItemInSlot == _itemInSlot && dropperSlot.AmountInSlot < dropperSlot.ItemInSlot.MaxStack)
            {
                int availableSpace = dropperSlot.ItemInSlot.MaxStack - dropperSlot.AmountInSlot;
                int amountToAdd = Mathf.Min(availableSpace, _amountInSlot);
                AddAmount(-amountToAdd);
                dropperSlot.AddAmount(amountToAdd);

                dropperSlot.SetStats();
                SetStats();
            }
            else
            {
                Items tempItem = _itemInSlot;
                int tempAmount = _amountInSlot;

                SetItemInSlot(dropperSlot.ItemInSlot);
                SetAmount(dropperSlot.AmountInSlot);

                dropperSlot.SetItemInSlot(tempItem);
                dropperSlot.SetAmount(tempAmount);

                dropperSlot.SetStats();
                SetStats();
            }
        }
    }
}
