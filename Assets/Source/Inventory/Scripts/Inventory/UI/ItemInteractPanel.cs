using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractPanel : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _dropOffset;
    [SerializeField] private Transform _panel;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _discardButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private SurvivalHandler _survivalHandler;

    private InventorySlot _currentSlot;
    private int _addAmount = 1;
    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //InventorySlotUI.OnInteract += Init;
        _playerInputHandler.InventoryPlayerInput.OnToggleInventory += ExitBttonClick;
        _useButton.onClick.AddListener(UseItemButtonClick);
        _discardButton.onClick.AddListener(DiscardButtonClick);
        _exitButton.onClick.AddListener(ExitBttonClick);
    }

    private void OnDisable()
    {
        //InventorySlotUI.OnInteract -= Init;
        _playerInputHandler.InventoryPlayerInput.OnToggleInventory += ExitBttonClick;
        _useButton.onClick.RemoveListener(UseItemButtonClick);
        _discardButton.onClick.RemoveListener(DiscardButtonClick);
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
        if(_currentSlot.ItemData != null)
        {
            if (_currentSlot.ItemData is FoodItemData foodItemData)
            {
                _survivalHandler.Hunger.ReplenishValue(foodItemData.AmountSatiety);
                _survivalHandler.Thirst.ReplenishValue(foodItemData.AmountWater);

                if(foodItemData.EmptyDishes != null)
                    _inventoryHolder.AddToInventory(foodItemData.EmptyDishes, _addAmount);
            }
            _inventoryHolder.RemoveInventory(_currentSlot.ItemData, _addAmount);
        }

        UpdateState();
    }

    private void DiscardButtonClick()
    {

        if (_inventoryHolder.InventorySystem.GetItemCount(_currentSlot.ItemData) >= 0)
        {
            Instantiate(_currentSlot.ItemData.ItemPrefab, _playerTransform.position + _playerTransform.forward * _dropOffset, Quaternion.identity);
            _inventoryHolder.RemoveInventory(_currentSlot.ItemData, _addAmount);
        }
        UpdateState();
    }

    private void ExitBttonClick()
    {
        if(_panel.gameObject.activeInHierarchy)
            _panel.gameObject.SetActive(false);
    }

    private void UpdateState()
    {
        _amountText.text = _inventoryHolder.InventorySystem.GetItemCount(_currentSlot.ItemData).ToString();

        if (_inventoryHolder.InventorySystem.GetItemCount(_currentSlot.ItemData) == 0)
            _panel.gameObject.SetActive(false);
    }
}
