using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Items _itemStats;
    [SerializeField] private int _amount;

    public Items ItemStats => _itemStats;
    public int Amount => _amount;

    public void AddAmount(int amount)
    {
        _amount += amount;
    }

    public void SetAmount( int amount)
    {
        _amount = amount;
    }
}
