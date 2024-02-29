using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionView : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _descriptionText;

    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;

    public event Action OnUsedButton;
    public event Action OnDiscardedButton;

    private void OnEnable()
    {
        _useButton.onClick.AddListener(UseButtonClick);
        _discardButton.onClick.AddListener(DiscardButtonClick);
    }

    private void OnDisable()
    {
        _useButton.onClick.RemoveListener(UseButtonClick);
        _discardButton.onClick.RemoveListener(DiscardButtonClick);
    }

    public void Init(Sprite icon, string title, string description)
    {
        _iconImage.sprite = icon;
        _titleText.text = title;
        _descriptionText.text = description;
    }

    public void Clear()
    {
        _iconImage.sprite = null;
        _titleText.text = "";
        _descriptionText.text = "";
    }

    private void UseButtonClick()
    {
        OnUsedButton?.Invoke();
    }

    private void DiscardButtonClick()
    {
        OnDiscardedButton?.Invoke();
    }
}
