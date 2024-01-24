using UnityEngine;

public class BackpackInventoryView : MonoBehaviour
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private DynamicInventoryDisplay _miniBackpackDisplay;
    [SerializeField] private BackpackInventory _backpackInventory;

    private bool _isOpenBackpack = false;

    private void Start()
    {
        _miniBackpackDisplay.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _backpackInventory.OnDinamicDisplayInventory += ToggleDisplay;
    }

    private void OnDisable()
    {
        _backpackInventory.OnDinamicDisplayInventory -= ToggleDisplay;
    }

    public void ToggleDisplay(InventorySystem inventoryDisplay, int offset)
    {
        _isOpenBackpack = !_isOpenBackpack;

        if (_isOpenBackpack)
        {
            _miniBackpackDisplay.gameObject.SetActive(true);
            _miniBackpackDisplay.RefreshDynamicInventory(inventoryDisplay, offset);
        }
        else
        {
            _miniBackpackDisplay.gameObject.SetActive(false);
        }
    }
}
