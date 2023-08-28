using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _payButton;
    [SerializeField] private ProductSlotView[] _slots;

    private StoreSlotData _slotData;

    public event UnityAction<StoreSlotData> OnPayButton;

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

        _titleText.text = _slotData.Title;
        _descriptionText.text = _slotData.Description;
        _image.sprite = _slotData.Sprite;
        CreateProducts(_slotData);
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
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    public void PayButtonClick()
    {
        OnPayButton?.Invoke(_slotData);
    }
}
