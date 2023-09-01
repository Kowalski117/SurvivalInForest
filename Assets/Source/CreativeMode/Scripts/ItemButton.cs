using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    [SerializeField] private Image _icon;

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
        _inventoryHolder.AddToInventory(_inventoryItemData, _amount, _inventoryItemData.Durability);
    }
}
