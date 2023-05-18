using StarterAssets;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    private bool _isOpenInventary = false;

    private void Start()
    {
        _panel.SetActive(false);
    }

    private void OnEnable()
    {
        _inventoryPlayerInput.SwitchInventory += ToggleInventory;
    }

    private void OnDisable()
    {
        _inventoryPlayerInput.SwitchInventory -= ToggleInventory;
    }

    private void ToggleInventory()
    {
        _isOpenInventary = !_isOpenInventary;
        _panel.SetActive(_isOpenInventary);
        _firstPersonController.enabled = !_isOpenInventary;
        _starterAssetsInputs.SetCursorState(!_isOpenInventary);
    }
}
