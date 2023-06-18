using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;

    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    [SerializeField] private ExchangeHandler _shopKeeperDisplay;
    [SerializeField] private CraftingHandler _craftingHandler;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;
    private bool _isShopOpen = false;
    private bool _isCraftPlayerOpen = false;

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
        _inventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;
        ExchangeKeeper.OnExchangeDisplayRequested += DisplayShopWindow;
        _inventoryPlayerInput.OnCraftPlayerWindow += DisplayCraftPlayerWindow;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        _inventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;
        ExchangeKeeper.OnExchangeDisplayRequested -= DisplayShopWindow;
        _inventoryPlayerInput.OnCraftPlayerWindow -= DisplayCraftPlayerWindow;
    }

    public void DisplayInventory(InventorySystem inventoryDisplay, int offset)
    {
        _isChestOpen = !_isChestOpen;

        if (_isChestOpen)
        {
            _inventoryPanel.gameObject.SetActive(true);
            _cursorController.SetCursorVisible(true);
            _inventoryPanel.RefreshDynamicInventory(inventoryDisplay, offset);
        }
        else
        {
            CloseChest();
        }
    }

    public void DisplayPlayerInventory(InventorySystem inventoryDisplay, int offset)
    {
        _isInventoryOpen = !_isInventoryOpen;

        if (_isInventoryOpen)
        {
            _playerBackpackPanel.gameObject.SetActive(true);
            _cursorController.SetCursorVisible(true);
            _playerBackpackPanel.RefreshDynamicInventory(inventoryDisplay, offset);
        }
        else
        {
            CloseInventory();
        }
    }

    private void DisplayShopWindow(ExchangeKeeper exchangeKeeper)
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

    private void DisplayCraftPlayerWindow(Crafting—ategory craftingCategory, bool playerInRange)
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

    private void CloseChest()
    {
        _inventoryPanel.gameObject.SetActive(false);
        _cursorController.SetCursorVisible(false);
    }

    private void CloseInventory()
    {
        _playerBackpackPanel.gameObject.SetActive(false);
        _cursorController.SetCursorVisible(false);
    }
}
