using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionUI : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _descriptionText;

    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;

    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private SurvivalHandler _survivalHandler;

    private InventorySlotUI _currentSlot;
    private InventorySlotUI _previousSlot;

    private void Awake()
    {
        ResetDescription();
    }

    private void OnEnable()
    {
        _useButton.onClick.AddListener(UseItemButtonClick);
        _discardButton.onClick.AddListener(DiscardButtonClick);
    }

    private void OnDisable()
    {
        _useButton.onClick.RemoveListener(UseItemButtonClick);
        _discardButton.onClick.RemoveListener(DiscardButtonClick);
    }

    public void ResetDescription()
    {
        _iconImage.gameObject.SetActive(false);
        _titleText.text = "";
        _descriptionText.text = "";
        _useButton.gameObject.SetActive(false);
        _discardButton.gameObject.SetActive(false);
    }

    public void SetDescription(InventorySlotUI inventorySlotUI)
    {

        if(_currentSlot != null && _currentSlot.AssignedInventorySlot.ItemData != null && _currentSlot != inventorySlotUI)
        {
            _currentSlot.ToggleHighlight();
        }

        if (inventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            _previousSlot = _currentSlot;

            if(_previousSlot != null)
                _previousSlot.TurnOffHighlight();

            _currentSlot = inventorySlotUI;

            if(_currentSlot != null)
                _currentSlot.ToggleHighlight();

            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = inventorySlotUI.AssignedInventorySlot.ItemData.Icon;
            _titleText.text = inventorySlotUI.AssignedInventorySlot.ItemData.DisplayName;
            _descriptionText.text = inventorySlotUI.AssignedInventorySlot.ItemData.Description;

            if (inventorySlotUI.AssignedInventorySlot.ItemData.Type == ItemType.Food)
            {
                _useButton.gameObject.SetActive(true);
            }
            else
            {
                _useButton.gameObject.SetActive(false);
            }
            _discardButton.gameObject.SetActive(true);
        }
        else
        {
            ResetDescription();
        }
    }

    private void UseItemButtonClick()
    {
        _survivalHandler.Eat(_currentSlot.AssignedInventorySlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            ResetDescription();
    }

    private void DiscardButtonClick()
    {
        _inventoryOperator.RemoveItem(_currentSlot);

        if (_currentSlot.AssignedInventorySlot.ItemData == null)
            ResetDescription();
    }
}
