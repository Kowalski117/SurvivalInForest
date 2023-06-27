using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private BuildTool _buildTool;

    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    [SerializeField] private ExchangeHandler _shopKeeperDisplay;
    [SerializeField] private CraftingHandler _craftingHandler;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;
    private bool _isShopOpen = false;
    private bool _isCraftPlayerOpen = false;
    private bool _isTurnOffWindows = false;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
        _playerBackpackPanel.gameObject.SetActive(false);

        _shopKeeperDisplay.gameObject.SetActive(false);
        _craftingHandler.CraftingWindow.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
        ExchangeKeeper.OnExchangeDisplayRequested += DisplayShopWindow;

        _inventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;
        _inventoryPlayerInput.OnCraftPlayerWindow += DisplayCraftPlayerWindow;

        _buildPlayerInput.OnDeleteBuilding += EnableWindows;

        _buildTool.OnCreateBuild += TurnOffWindows;
        _buildTool.OnCompletedBuild += EnableWindows;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        ExchangeKeeper.OnExchangeDisplayRequested -= DisplayShopWindow;

        _inventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;
        _inventoryPlayerInput.OnCraftPlayerWindow -= DisplayCraftPlayerWindow;

        _buildPlayerInput.OnDeleteBuilding -= EnableWindows;

        _buildTool.OnCreateBuild -= TurnOffWindows;
        _buildTool.OnCompletedBuild -= EnableWindows;
    }

    public void DisplayInventory(InventorySystem inventoryDisplay, int offset)
    {
        //if (!_isTurnOffWindows)
        {
            _isChestOpen = !_isChestOpen;

            if (_isChestOpen)
            {
                _inventoryPanel.gameObject.SetActive(true);
                _inventoryPanel.RefreshDynamicInventory(inventoryDisplay, offset);
            }
            else
            {
                CloseChest();
            }
        }
    }

    public void DisplayPlayerInventory(InventorySystem inventoryDisplay, int offset)
    {
        //if (!_isTurnOffWindows)
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
    }

    private void DisplayShopWindow(ExchangeKeeper exchangeKeeper)
    {
        //if (!_isTurnOffWindows)
        {
            _isShopOpen = !_isShopOpen;

            if (_isShopOpen)
            {
                _shopKeeperDisplay.gameObject.SetActive(true);
                _shopKeeperDisplay.CreateSlots();
            }
            else
            {
                _shopKeeperDisplay.gameObject.SetActive(false);
            }
        }
    }

    private void DisplayCraftPlayerWindow(Crafting—ategory craftingCategory)
    {
        //if (!_isTurnOffWindows)
        {
            _isCraftPlayerOpen = !_isCraftPlayerOpen;

            if (_isCraftPlayerOpen)
            {
                _craftingHandler.CraftingWindow.gameObject.SetActive(true);
                _craftingHandler.UpdateSlot();
            }
            else
            {
                _craftingHandler.CraftingWindow.gameObject.SetActive(false);
            }
        }
    }

    private void TurnOffWindows()
    {
        _isTurnOffWindows = !_isTurnOffWindows;

        if(_isTurnOffWindows)
        {
            CloseWindow(_isCraftPlayerOpen, _craftingHandler.CraftingWindow.gameObject);
            CloseWindow(_isShopOpen, _shopKeeperDisplay.gameObject);
            CloseWindow(_isInventoryOpen, _playerBackpackPanel.gameObject);
            CloseWindow(_isChestOpen, _inventoryPanel.gameObject);
            _cursorController.SetCursorVisible(false);
            _buildPlayerInput.enabled = true;
            _inventoryPlayerInput.enabled = false;
            _buildTool.SetDeleteModeEnabled(false);
        }
    }

    private void EnableWindows()
    {
        if (_isTurnOffWindows)
        {
            OpenWindow(_isCraftPlayerOpen, _craftingHandler.CraftingWindow.gameObject);
            OpenWindow(_isShopOpen, _shopKeeperDisplay.gameObject);
            OpenWindow(_isInventoryOpen, _playerBackpackPanel.gameObject);
            OpenWindow(_isChestOpen, _inventoryPanel.gameObject);
            _cursorController.SetCursorVisible(true);
            _isTurnOffWindows = false;
            _inventoryPlayerInput.enabled = true;
            _buildPlayerInput.enabled = false;
        }
    }

    private void CloseChest()
    {
        if (!_isChestOpen)
        {
            _inventoryPanel.gameObject.SetActive(false);
        }
    }

    private void CloseWindow(bool isActive, GameObject window)
    {
        if (isActive)
        {
            window.gameObject.SetActive(false);
        }
    }

    private void OpenWindow(bool isActive, GameObject window)
    {
        if (isActive)
        {
            window.gameObject.SetActive(true);
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
