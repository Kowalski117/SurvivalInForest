using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;
    [SerializeField] private StaticInventoryDisplay _playerHotbarInventory;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
        _playerBackpackPanel.CanvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
        _playerInputHandler.InventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        _playerInputHandler.InventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;
    }

    public void DisplayInventory(InventorySystem inventoryDisplay, int offset)
    {
        _isChestOpen = !_isChestOpen;

        if (_isChestOpen)
        {
            _inventoryPanel.gameObject.SetActive(true);
            _inventoryPanel.RefreshDynamicInventory(inventoryDisplay, offset);
        }
        else
        {
            _inventoryPanel.gameObject.SetActive(false);
        }
    }

    public void DisplayPlayerInventory(InventorySystem inventoryDisplay, int offset)
    {
        _isInventoryOpen = !_isInventoryOpen;

        if (_isInventoryOpen)
        {
            _playerInputHandler.ToggleBuildPlayerInput(false);
            _playerInputHandler.ToggleInteractionConstructionInput(false);
            _playerBackpackPanel.CanvasGroup.alpha = 1;
            _playerInputHandler.SetCursorVisible(true);
            _playerBackpackPanel.RefreshDynamicInventory(inventoryDisplay, offset);
            _playerInputHandler.ToggleHotbarDisplay(false);
        }
        else
        {
            _playerBackpackPanel.ResetSelection();
            _playerHotbarInventory.ResetSelection();
            _playerBackpackPanel.CanvasGroup.alpha = 0;
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleBuildPlayerInput(true);
            _playerInputHandler.ToggleInteractionConstructionInput(true);
            _playerInputHandler.ToggleHotbarDisplay(true);
        }
    }
}
