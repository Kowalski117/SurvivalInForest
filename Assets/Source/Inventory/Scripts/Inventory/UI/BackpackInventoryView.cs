using UnityEngine;

public class BackpackInventoryView : MonoBehaviour
{
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private DynamicInventoryDisplay _miniBackpackDisplay;
    [SerializeField] private BackpackInventory _backpackInventory;

    private bool _isOpenBackpack = false;

    private void Start()
    {
        _miniBackpackDisplay.Close();
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
            _miniBackpackDisplay.Open();
            _miniBackpackDisplay.Refresh(inventoryDisplay, offset);
        }
        else
        {
            _miniBackpackDisplay.Close();
        }
    }
}
