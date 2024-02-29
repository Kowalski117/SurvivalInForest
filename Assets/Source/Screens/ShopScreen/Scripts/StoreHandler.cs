using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private StoreSlot _prefabSlot;
    [SerializeField] private StoreSlotData[] _storeSlotsData;
    [SerializeField] private List<StoreSlot> _storeSlots = new List<StoreSlot>();
    [SerializeField] private Transform _containerSlots;
    [SerializeField] private YandexAds _yandexAds;

    public event Action<Dictionary<InventoryItemData, int>> OnBonusShown;
    public event Action OnProductBuyed;

    private void Awake()
    {
        InitSlots();
    }

    private void OnEnable()
    {
        foreach (var slot in _storeSlots)
        {
            slot.OnPayedButton += TrySellProduct;
        }
    }

    private void OnDisable()
    {
        foreach (var slot in _storeSlots)
        {
            slot.OnPayedButton -= TrySellProduct;
        }
    }

    public void InitSlots()
    {
        for (int i = 0; i < _storeSlots.Count; i++)
        {
            _storeSlots[i].Init(_storeSlotsData[i]);
        }
    }

    private void TrySellProduct(StoreSlot slot)
    {
        if (slot.StoreSlotData.IsOpenAds)
        {
            _yandexAds.ShowRewardAd(() => AddItem(slot.StoreSlotData));
        }
        else
        {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
            Billing.PurchaseProduct(slot.StoreSlotData.Id, (purchaseProduct) =>
            {
                OnProductBuyed?.Invoke();
                AddItem(slot.StoreSlotData);
            });
#endif
        }
    }

    private void AddItem(StoreSlotData data)
    {
        Dictionary<InventoryItemData, int> products = new Dictionary<InventoryItemData, int>();

        foreach (var product in data.Products)
        {
            products.Add(product.ItemData, product.Amount);
        }

        OnBonusShown?.Invoke(products);
    }
}
