using UnityEngine;
using UnityEngine.Events;

public class UIInventoryHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private DynamicInventoryDisplay _chestInventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;
    [SerializeField] private StaticInventoryDisplay _playerHotbarInventory;
    [SerializeField] private ClothesInventory _clothesInventory;
    [SerializeField] private DynamicInventoryDisplay _playerClothesPanel;
    [SerializeField] private MouseItemData _mouseItemData;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;

    public event UnityAction OnInventoryClosed;

    public bool IsInventoryOpen => _isInventoryOpen;
    public bool IsChestOpen => _isChestOpen;

    private void Awake()
    {
        _chestInventoryPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        _playerBackpackPanel.CanvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        _playerInputHandler.InventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;
        _playerHealth.OnDied += TurnOffDisplayInventory;
    }

    private void OnDisable()
    {
        _playerInputHandler.InventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;
        _playerHealth.OnDied -= TurnOffDisplayInventory;
    }

    public void DisplayChestInventory(ChestInventory chestInventory, int offset)
    {
        _isChestOpen = !_isChestOpen;

        if (_isChestOpen)
        {
            _chestInventoryPanel.gameObject.SetActive(true);
            _chestInventoryPanel.RefreshDynamicInventory(chestInventory.InventorySystem, offset);
        }
        else
        {
            _chestInventoryPanel.gameObject.SetActive(false);
        }
    }

    public void DisplayPlayerInventory(InventorySystem inventoryDisplay, int offset)
    {
        _isInventoryOpen = !_isInventoryOpen;

        if (_isInventoryOpen)
        {
            _playerBackpackPanel.CanvasGroup.alpha = 1;
            _playerBackpackPanel.CanvasGroup.blocksRaycasts = true;
            _playerInputHandler.SetCursorVisible(true);
            _playerBackpackPanel.RefreshDynamicInventory(inventoryDisplay, offset);
            _playerInputHandler.ToggleHotbarDisplay(false);
            _playerInputHandler.ToggleInteractionInput(false);

            _playerClothesPanel.RefreshDynamicInventory(_clothesInventory.InventorySystem, 0);
        }
        else
        {
            OnInventoryClosed?.Invoke();
            _mouseItemData.ReturnCurrentSlot();
            _playerBackpackPanel.ResetSelection();
            _playerHotbarInventory.ResetSelection();
            _playerBackpackPanel.CanvasGroup.alpha = 0;
            _playerBackpackPanel.CanvasGroup.blocksRaycasts = false;
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleInteractionInput(true);
            _playerInputHandler.ToggleHotbarDisplay(true);
        }
    }

    public void TurnOffDisplayInventory()
    {
        if(_isInventoryOpen)
            _playerInputHandler.InventoryPlayerInput.ToggleInventory();
    }
}
