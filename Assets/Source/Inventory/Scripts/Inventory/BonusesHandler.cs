using System.Collections.Generic;
using UnityEngine;

public class BonusesHandler : MonoBehaviour
{
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private PlayerHandler _playerHandler;
    [SerializeField] private ChestHandler _chestHandler;
    [SerializeField] private ChestInventory _chestInventory;
    [SerializeField] private DynamicInventoryDisplay _chestDisplay;

    [SerializeField] private StoreHandler _storeHandler;
    [SerializeField] private RouletteScrollHandler _rouletteScrollHandler;
    [SerializeField] private RewardHandler _rewardHandler;
         
    [SerializeField] private PauseScreen _pauseScreen;

    private bool _isActive = false;

    private void OnEnable()
    {
        _inventoryHandler.OnInventoryClosed += CloseChest;

        _storeHandler.OnBonusShown += ShowItems;
        _rouletteScrollHandler.OnBonusShown += ShowItems;
        _rewardHandler.OnBonusShown += ShowItems;
    }

    private void OnDisable()
    {
        _inventoryHandler.OnInventoryClosed -= CloseChest;

        _storeHandler.OnBonusShown -= ShowItems;
        _rouletteScrollHandler.OnBonusShown -= ShowItems;
        _rewardHandler.OnBonusShown -= ShowItems;
    }

    public void OpenChest()
    {
        _isActive = true;
        _chestHandler.OpenChest(_chestInventory);
        _pauseScreen.ToggleAllScreen();
        _chestDisplay.SetAddItem(false);

        if (!_inventoryHandler.IsInventoryOpen)
            _playerHandler.InventoryPlayerInput.ToggleInventory();
    }

    private void ShowItems(Dictionary<InventoryItemData, int> items)
    {
        foreach (var item in items)
        {
            _chestInventory.AddInventoryItem(item.Key, item.Value);
        }

        OpenChest();
    }

    private void CloseChest()
    {
        if(_isActive)
        {
            _isActive = false;
            _chestHandler.CloseChest();
            _chestDisplay.SetAddItem(true);
            //_chestInventory.RemoveAllItems();
        }
    }
}
