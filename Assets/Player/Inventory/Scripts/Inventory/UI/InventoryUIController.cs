using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
        _playerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDispleyRequested += DisplayPlayerInventory;
        _inventoryPlayerInput.SwitchInventory += OpenDisplay;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDispleyRequested -= DisplayPlayerInventory;
        _inventoryPlayerInput.SwitchInventory -= OpenDisplay;
    }

    private void Update()
    {
        if (_inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // поменять
        {
            _inventoryPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }

        if (_playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) // поменять
        {
            _playerBackpackPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }

    }

    public void DisplayInventory(InventorySystem inventoryDislay, int offSet)
    {
        //_playerBackpackPanel.gameObject.SetActive(true);
        _inventoryPanel.gameObject.SetActive(true);
        _cursorController.SetCursorVisible(true);
        _inventoryPanel.RefreshDynamicInventory(inventoryDislay, offSet);
        //_playerBackpackPanel.RefreshDynamicInventory(inventoryDislay, offSet);
    }

    public void DisplayPlayerInventory(InventorySystem inventoryDislay, int offSet)
    {
        _playerBackpackPanel.gameObject.SetActive(true);
        //_inventoryPanel.gameObject.SetActive(true);
        _cursorController.SetCursorVisible(true);
        //_inventoryPanel.RefreshDynamicInventory(inventoryDislay, offSet);
        _playerBackpackPanel.RefreshDynamicInventory(inventoryDislay, offSet);
    }

    public void OpenDisplay()
    {
        if(_inventoryPanel.gameObject.activeInHierarchy)
        {
            _inventoryPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
        }
    }
}
