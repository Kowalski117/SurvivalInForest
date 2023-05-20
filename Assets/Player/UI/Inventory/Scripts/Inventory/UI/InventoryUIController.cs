using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;

    private void Awake()
    {
        _inventoryPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested += DisplayInventory;
        _inventoryPlayerInput.SwitchInventory += OpenDisplay;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDispleyRequested -= DisplayInventory;
        _inventoryPlayerInput.SwitchInventory -= OpenDisplay;
    }

    public void DisplayInventory(InventorySystem inventoryDislay)
    {
        _inventoryPanel.RefreshDynamicInventory(inventoryDislay);
    }

    public void OpenDisplay()
    {
        DisplayInventory(new InventorySystem(10));
        if(_inventoryPanel.gameObject.activeInHierarchy)
        {
            _inventoryPanel.gameObject.SetActive(false);
            _cursorController.SetCursorVisible(false);
            return;
        }
        else
        {
            _inventoryPanel.gameObject.SetActive(true);
            _cursorController.SetCursorVisible(true);
            return;
        }
    }
}
