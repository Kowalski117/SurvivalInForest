using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<Slot> _slots = new List<Slot>();

    private void Awake()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (!_slots[i].ItemInSlot)
            {
                for (int j = 0; j < _slots[i].transform.childCount; j++)
                {
                    _slots[i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    public void PickUpItem(ItemObject itemObject)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].ItemInSlot != null && _slots[i].ItemInSlot.Id == itemObject.ItemStats.Id && _slots[i].AmountInSlot < _slots[i].ItemInSlot.MaxStack)
            {
                int remainingAmount = itemObject.Amount;

                int spaceAvailable = _slots[i].ItemInSlot.MaxStack - _slots[i].AmountInSlot;
                int amountToAdd = Mathf.Min(spaceAvailable, remainingAmount);
                _slots[i].AddAmount(amountToAdd);
                remainingAmount -= amountToAdd;

                itemObject.SetAmount(remainingAmount);
                _slots[i].SetStats();
                if (remainingAmount <= 0)
                {
                    Destroy(itemObject.gameObject);
                    return;
                }
            }
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].ItemInSlot == null)
            {
                _slots[i].SetItemInSlot(itemObject.ItemStats);
                _slots[i].AddAmount(itemObject.Amount);
                Destroy(itemObject.gameObject);
                _slots[i].SetStats();
                return;
            }
        }
    }
}
