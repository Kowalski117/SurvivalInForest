using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoreSlotView : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _payButton;
    [SerializeField] private TMP_Text _priceText;

    public event UnityAction OnPayedButton;

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
        _titleText.text = storeSlot.Name;
        _descriptionText.text = storeSlot.Description;
        _image.sprite = storeSlot.Sprite;
        _priceText.text = storeSlot.Price.ToString();
    }

    private void PayButtonClick()
    {
        OnPayedButton?.Invoke();
    }
}
