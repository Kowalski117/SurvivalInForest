using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractPanel : MonoBehaviour
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private SurvivalHandler _survivalHandler;

    private InventorySlot _currentSlot;

    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventorySlotUI.OnInteract += Init;

        _useButton.onClick.AddListener(UseItemButtonClick);
        _exitButton.onClick.AddListener(ExitBttonClick);
    }

    private void OnDisable()
    {
        InventorySlotUI.OnInteract -= Init;

        _useButton.onClick.RemoveListener(UseItemButtonClick);
        _exitButton.onClick.RemoveListener(ExitBttonClick);
    }

    public void Init(InventorySlot inventorySlot)
    {
        _panel.gameObject.SetActive(true);

        _currentSlot = inventorySlot;
        _icon.sprite = inventorySlot.ItemData.Icon;
        _nameText.text = inventorySlot.ItemData.DisplayName;
        _descriptionText.text = inventorySlot.ItemData.Description;
        _amountText.text = _inventoryHolder.InventorySystem.GetItemCount(inventorySlot.ItemData).ToString();

        if (inventorySlot.ItemData.Type == ItemType.Food)
        {
            _useButton.gameObject.SetActive(true);
        }
        else
        {
            _useButton.gameObject.SetActive(false);
        }
    }

    private void UseItemButtonClick()
    {
        if (_inventoryHolder.InventorySystem.RemoveItemsInventory(_currentSlot.ItemData, 1))
        {
            if (_currentSlot.ItemData is FoodItemData foodItemData)
            {
                _survivalHandler.Hunger.ReplenishValue(foodItemData.AmountSatiety);
                _survivalHandler.Thirst.ReplenishValue(foodItemData.AmountWater);
            }

            _amountText.text = _inventoryHolder.InventorySystem.GetItemCount(_currentSlot.ItemData).ToString();

            if (_inventoryHolder.InventorySystem.GetItemCount(_currentSlot.ItemData) == 0)
                _panel.gameObject.SetActive(false);
        }
    }

    private void ExitBttonClick()
    {
        _panel.gameObject.SetActive(false);
    }
}
