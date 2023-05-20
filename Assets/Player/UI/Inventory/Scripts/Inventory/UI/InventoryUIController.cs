using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private DynamicInventoryDisplay _chestPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    private void Awake()
    {
        _chestPanel.gameObject.SetActive(false);
        _playerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackInventoryDispleyRequested += DisplayPlayerBackpack;
        _inventoryPlayerInput.SwitchInventory += OpenDisplay;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackInventoryDispleyRequested -= DisplayPlayerBackpack;
        _inventoryPlayerInput.SwitchInventory -= OpenDisplay;
    }

    private void Update()
    {
        if (_chestPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // поменять
        {
            _chestPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }

        if (_playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // поменять
        {
            _playerBackpackPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }

    }

    public void DisplayInventory(InventorySystem inventoryDislay)
    {
        _chestPanel.gameObject.SetActive(true);
        _cursorController.SetCursorVisible(true);
        _chestPanel.RefreshDynamicInventory(inventoryDislay);
    }

    public void DisplayPlayerBackpack(InventorySystem inventoryDislay)
    {
        _playerBackpackPanel.gameObject.SetActive(true);
        _cursorController.SetCursorVisible(true);
        _playerBackpackPanel.RefreshDynamicInventory(inventoryDislay);
    }

    public void OpenDisplay()
    {
        if(_chestPanel.gameObject.activeInHierarchy)
        {
            _chestPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }
    }
}
