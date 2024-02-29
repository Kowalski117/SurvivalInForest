using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private int _amountMax = 50;

    private Button _button;
    private InventoryItemData _inventoryItemData;
    private PlayerInventoryHolder _inventoryHolder;

    private int _amount = 1;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(AddItem);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(AddItem);
    }

    public void Init(PlayerInventoryHolder playerInventoryHolder, InventoryItemData inventoryItemData)
    {
        _inventoryHolder = playerInventoryHolder;
        _inventoryItemData = inventoryItemData;
        _icon.sprite = _inventoryItemData.Icon;
    }

    private void AddItem()
    {
        if(Keyboard.current.leftShiftKey.isPressed)
            _inventoryHolder.AddItem(_inventoryItemData, _amountMax, _inventoryItemData.Durability);
        else
            _inventoryHolder.AddItem(_inventoryItemData, _amount, _inventoryItemData.Durability);
    }
}
