using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private PlayerInputHandler _cursorController;
    [SerializeField] private BuildTool _buildTool;

    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    [SerializeField] private SleepPanel _sleepPanel;
    [SerializeField] private ExchangeHandler _shopKeeperDisplay;
    [SerializeField] private CraftingHandler _craftingHandler;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;

    private bool _isTurnOffWindows = false;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
        _playerBackpackPanel.gameObject.SetActive(false);
        _craftingHandler.CraftingWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
        _inventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;

        _buildPlayerInput.OnDeleteBuilding += EnableWindows;
        _buildTool.OnCreateBuild += TurnOffWindows;
        _buildTool.OnCompletedBuild += EnableWindows;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        _inventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;

        _buildPlayerInput.OnDeleteBuilding -= EnableWindows;
        _buildTool.OnCreateBuild -= TurnOffWindows;
        _buildTool.OnCompletedBuild -= EnableWindows;
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
            _buildPlayerInput.enabled = false;
            _playerBackpackPanel.gameObject.SetActive(true);
            _cursorController.SetCursorVisible(true);
            _playerBackpackPanel.RefreshDynamicInventory(inventoryDisplay, offset);
        }
        else
        {
            CloseInventory();
        }
    }

    private void TurnOffWindows()
    {
        _isTurnOffWindows = !_isTurnOffWindows;

        if (_isTurnOffWindows)
        {
            _cursorController.ToggleInventoryPanels(false);
            _cursorController.SetCursorVisible(false);
            _cursorController.ToggleBuildPlayerInput(true);
            _cursorController.ToggleInventoryInput(false);
            _buildTool.SetDeleteModeEnabled(false);
        }
    }

    private void EnableWindows()
    {
        if (_isTurnOffWindows)
        {
            _cursorController.SetCursorVisible(true);
            _cursorController.ToggleInventoryPanels(true);
            _cursorController.ToggleBuildPlayerInput(false);
            _cursorController.ToggleInventoryInput(true);
            _isTurnOffWindows = false;
        }
    }

    private void CloseInventory()
    {
        if (!_isInventoryOpen)
        {
            _playerBackpackPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
            _buildPlayerInput.enabled = true;
        }
    }
}
