using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _payButton;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private ProductSlotView[] _slots;
    [SerializeField] private GameObject _adsPoint;
    [SerializeField] private GameObject _janPoint;

    private StoreSlotData _slotData;

    public event Action<StoreSlot> OnPayedButton;

    public StoreSlotData StoreSlotData => _slotData;

    private void OnEnable()
    {
        _payButton.onClick.AddListener(PayButtonClick);
    }

    private void OnDisable()
    {
        _payButton.onClick.RemoveListener(PayButtonClick);
    }

    public void Init(StoreSlotData storeSlot)
    {
        _slotData = storeSlot;

        _titleText.text = _slotData.Name;
        _descriptionText.text = _slotData.Description;
        _image.sprite = _slotData.Sprite;
        _priceText.text = _slotData.Price.ToString();
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

    public void PayButtonClick()
    {
        OnPayedButton?.Invoke(this);
    }
}
