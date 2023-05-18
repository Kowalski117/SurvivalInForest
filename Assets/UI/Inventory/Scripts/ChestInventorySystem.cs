using System.Collections.Generic;
using UnityEngine;

public class ChestInventorySystem : MonoBehaviour
{
    [SerializeField] private List<Slot> _slots = new List<Slot>();
    [SerializeField] private int _countSlots;
    [SerializeField] private int _maxSlots;
    [SerializeField] private Transform _slotsContainer;
    [SerializeField] private Slot _slotPrefab;

    private Slot _currentSlot;

    private void Awake()
    {
        for (int i = 0; i < _maxSlots; i++)
        {
            _currentSlot = Instantiate(_slotPrefab, _slotsContainer);
            _slots.Add(_currentSlot);
        }

        for (int i = 1; i < _maxSlots; i++)
        {
            if(_countSlots < i)
            {
                _slots[i].gameObject.SetActive(false);
            }
        }

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
}
