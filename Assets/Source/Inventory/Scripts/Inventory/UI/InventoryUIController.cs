using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    //[SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    //[SerializeField] private CursorController _cursorController;

    //[SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    //[SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    //[SerializeField] private ExchangeHandler _shopKeeperDisplay;
    //[SerializeField] private ExchangeKeeper _shopKeeper;
    //[SerializeField] private CraftingHandler _craftingHandler;

    //private bool _isInventoryOpen = false;
    //private bool _isChestOpen = false;
    //private bool _isShopOpen = false;
    //private bool _isCraftOpen = false;

    //private void Awake()
    //{
    //    _inventoryPanel.gameObject.SetActive(false);
    //    _playerBackpackPanel.gameObject.SetActive(false);

    //    _shopKeeperDisplay.gameObject.SetActive(false);
    //    _craftingHandler.gameObject.SetActive(false);
    //}

    //private void OnEnable()
    //{
    //    InventoryHolder.OnDinamicInventoryDispleyRequested += DisplayInventory;
    //    _inventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;

    //    CraftBench.OnCraftingDisplayRequested += DisplayCraftWindow;
    //    ExchangeKeeper.OnExchangeDisplayRequested += DisplayShopWindow;
    //}

    //private void OnDisable()
    //{
    //    InventoryHolder.OnDinamicInventoryDispleyRequested -= DisplayInventory;
    //    _inventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;

    //    CraftBench.OnCraftingDisplayRequested -= DisplayCraftWindow;
    //    ExchangeKeeper.OnExchangeDisplayRequested -= DisplayShopWindow;
    //}

    //public void DisplayInventory(InventorySystem inventoryDislay, int offSet)
    //{
    //    _isChestOpen = !_isChestOpen;

    //    if (_isChestOpen)
    //    {
    //        _inventoryPanel.gameObject.SetActive(true);
    //        _cursorController.SetCursorVisible(true);
    //        _inventoryPanel.RefreshDynamicInventory(inventoryDislay, offSet);
    //    }
    //    else
    //    {
    //        CloseChest();
    //    }
    //}

    //public void DisplayPlayerInventory(InventorySystem inventoryDislay, int offSet)
    //{
    //    _isInventoryOpen = !_isInventoryOpen;

    //    if (_isInventoryOpen)
    //    {
    //        _playerBackpackPanel.gameObject.SetActive(true);
    //        _cursorController.SetCursorVisible(true);
    //        _playerBackpackPanel.RefreshDynamicInventory(inventoryDislay, offSet);
    //    }
    //    else
    //    {
    //        CloseInventory();
    //    }
    //}

    //private void DisplayShopWindow(ExchangeKeeper exchangeKeeper)
    //{
    //    _isShopOpen = !_isShopOpen;

    //    if (_isShopOpen)
    //    {
    //        _shopKeeperDisplay.gameObject.SetActive(true);
    //        _shopKeeperDisplay.CreateSlots();
    //    }
    //    else
    //    {
    //        _shopKeeperDisplay.gameObject.SetActive(false);
    //    }
    //}

    //private void DisplayCraftWindow(CraftBench craftBench)
    //{
    //    _isCraftOpen = !_isCraftOpen;

    //    if (_isCraftOpen)
    //    {
    //        _craftingHandler.gameObject.SetActive(true);
    //        _craftingHandler.DisplayCraftingWindow(craftBench.Crafting—ategory);
    //    }
    //    else
    //    {
    //        _craftingHandler.gameObject.SetActive(false);
    //    }
    //}

    //private void CloseChest()
    //{
    //    _inventoryPanel.gameObject.SetActive(false);
    //    _cursorController.SetCursorVisible(false);
    //}

    //private void CloseInventory()
    //{
    //    _playerBackpackPanel.gameObject.SetActive(false);
    //    _cursorController.SetCursorVisible(false);
    //}
}
