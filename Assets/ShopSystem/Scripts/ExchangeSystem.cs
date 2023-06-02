using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ExchangeSystem
{
    [SerializeField] private List<ExchangerSlot> _shopInventory;
    [SerializeField] private int _availableGold;
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;

    public List<ExchangerSlot> ShopInventory => _shopInventory;
    public int AvailableGold => _availableGold;
    public float BuyMarkUp => _buyMarkUp;
    public float SellMarkUp => _sellMarkUp;

    public ExchangeSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ExchangerSlot>(size);

        for (int i = 0; i < _shopInventory.Count; i++)
        {
            _shopInventory.Add(new ExchangerSlot());
        }
    }

    public void AddToShop(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out ExchangerSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ExchangerSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ExchangerSlot();
            _shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out ExchangerSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if (!ContainsItem(data, out ExchangerSlot slot))
            return;

        slot.RemoveFromStack(amount);
    }

    public void GainGold(int basketTotal)
    {
        _availableGold += basketTotal;
    }
}
