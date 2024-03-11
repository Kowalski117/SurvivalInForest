using System;
using UnityEngine;

[RequireComponent(typeof(StoreSlotView))]
public class StoreSlot : MonoBehaviour
{
    [SerializeField] private ProductSlotView[] _slots;
    [SerializeField] private GameObject _adsPoint;
    [SerializeField] private GameObject _janPoint;

    private StoreSlotView _view;
    private StoreSlotData _slotData;

    public event Action<StoreSlot> OnPayedButton;

    public StoreSlotData StoreSlotData => _slotData;

    private void Awake()
    {
        _view = GetComponent<StoreSlotView>();
    }

    private void OnEnable()
    {
        _view.OnPayedButton += Pay;
    }

    private void OnDisable()
    {
        _view.OnPayedButton -= Pay;
    }

    public void Init(StoreSlotData storeSlot)
    {
        _slotData = storeSlot;
        _view.Init(_slotData);

        CreateProducts(_slotData);

        if (_slotData.IsOpenAds)
        {
            _adsPoint.gameObject.SetActive(true);
            _janPoint.gameObject.SetActive(false);
        }
        else
        {
            _adsPoint.gameObject.SetActive(false);
            _janPoint.gameObject.SetActive(true);
        }
    }

    public void CreateProducts(StoreSlotData storeSlot)
    {
        foreach (var product in storeSlot.Products)
        {
            foreach (var slot in _slots)
            {
                if (!slot.IsBusy) 
                {
                    slot.Init(product.ItemData, product.Amount);
                    break;
                }
            }
        }

        foreach (var slot in _slots)
        {
            if (!slot.IsBusy)
                slot.gameObject.SetActive(false);
        }
    }

    public void UpdateLanguage()
    {
        _view.Init(_slotData);
    }

    private void Pay()
    {
        OnPayedButton?.Invoke(this);
    }
}
