using UnityEngine;

public class UIInventoryHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;
    [SerializeField] private StaticInventoryDisplay _playerHotbarInventory;
    [SerializeField] private ClothesInventory _clothesInventory;
    [SerializeField] private DynamicInventoryDisplay _playerClothesPanel;
    [SerializeField] private MouseItemData _mouseItemData;

    private bool _isInventoryOpen = false;
    private bool _isChestOpen = false;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        _playerBackpackPanel.CanvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
        _playerInputHandler.InventoryPlayerInput.SwitchInventory += DisplayPlayerInventory;

        _playerHealth.OnDied += TurnOffDisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        _playerInputHandler.InventoryPlayerInput.SwitchInventory -= DisplayPlayerInventory;

        _playerHealth.OnDied -= TurnOffDisplayInventory;
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
            _playerBackpackPanel.CanvasGroup.blocksRaycasts = true;
            _playerInputHandler.SetCursorVisible(true);
            _playerBackpackPanel.RefreshDynamicInventory(inventoryDisplay, offset);
            _playerInputHandler.ToggleHotbarDisplay(false);

            _playerClothesPanel.RefreshDynamicInventory(_clothesInventory.InventorySystem, 0);
        }
        else
        {
            _mouseItemData.ReturnCurrentSlot();
            _playerBackpackPanel.ResetSelection();
            _playerHotbarInventory.ResetSelection();
            _playerBackpackPanel.CanvasGroup.alpha = 0;
            _playerBackpackPanel.CanvasGroup.blocksRaycasts = false;
            _playerInputHandler.SetCursorVisible(false);
            _playerInputHandler.ToggleBuildPlayerInput(true);
            _playerInputHandler.ToggleInteractionConstructionInput(true);
            _playerInputHandler.ToggleHotbarDisplay(true);
        }
    }

    private void TurnOffDisplayInventory()
    {
        if(_isInventoryOpen)
            _playerInputHandler.InventoryPlayerInput.ToggleInventory();
    }
}
