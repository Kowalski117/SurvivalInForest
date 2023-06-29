using StarterAssets;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private SelectionPlayerInput _selectionPlayerInput;
    [SerializeField] private InteractionPlayerInput _interactionPlayerInput;
    [SerializeField] private InteractionConstructionPlayerInput _interactionConstructionPlayerInput;
    [SerializeField] private HitPlayerInput _hitPlayerInput;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private Transform _inventoryPanels;

    private void Start()
    {
        SetCursorVisible(false);
    }

    public void SetCursorVisible(bool visible)
    {
        _firstPersonController.enabled = !visible;
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ToggleInventoryInput(bool visible)
    {
        _inventoryPlayerInput.enabled = visible;
    }

    public void ToggleSelectionInput(bool visible)
    {
        _selectionPlayerInput.enabled = visible;
    }

    public void ToggleInteractionInput(bool visible)
    {
        _interactionPlayerInput.enabled = visible;
    }

    public void ToggleInteractionConstructionInput(bool visible)
    {
        _interactionConstructionPlayerInput.enabled = visible;
    }

    public void ToggleHitPlayerInput(bool visible)
    {
        _hitPlayerInput.enabled = visible;
    }

    public void ToggleBuildPlayerInput(bool visible)
    {
        _buildPlayerInput.enabled = visible;
    }

    public void ToggleInventoryPanels(bool visible)
    {
         _inventoryPanels.gameObject.SetActive(visible);
    }
}
