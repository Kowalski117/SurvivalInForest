using StarterAssets;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private InteractionPlayerInput _interactionPlayerInput;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private UIScreenPlayerInput _screenPlayerInput;
    [SerializeField] private Transform _inventoryPanels;

    public FirstPersonController FirstPersonController => _firstPersonController;
    public InventoryPlayerInput InventoryPlayerInput => _inventoryPlayerInput;
    public InteractionPlayerInput InteractionPlayerInput => _interactionPlayerInput;
    public BuildPlayerInput BuildPlayerInput => _buildPlayerInput;
    public UIScreenPlayerInput ScreenPlayerInput => _screenPlayerInput;
    public Transform InventoryPanels => _inventoryPanels;

    private void Start()
    {
        SetCursorVisible(false);
    }

    public void SetCursorVisible(bool visible)
    {
        _firstPersonController.TogglePersonController(!visible);

        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePersonController(bool visible)
    {
        _firstPersonController.TogglePersonController(visible);
    }

    public void ToggleHotbarDisplay(bool visible)
    {
        _hotbarDisplay.ToggleHotbarDisplay(visible);
    }

    public void ToggleInventoryInput(bool visible)
    {
        _inventoryPlayerInput.enabled = visible;
    } 
    
    public void ToggleScreenPlayerInput(bool visible)
    {
        _screenPlayerInput.enabled = visible;
    }

    public void ToggleInteractionConstructionInput(bool visible)
    {
        _interactionPlayerInput.TurnOff();
        _interactionPlayerInput.enabled = visible;
    }
    public void ToggleBuildPlayerInput(bool visible)
    {
        _buildPlayerInput.enabled = visible;
    }

    public void ToggleInventoryPanels(bool visible)
    {
         _inventoryPanels.gameObject.SetActive(visible);
    }

    public void ToggleAllInput(bool visible)
    {
        TogglePersonController(visible);
        ToggleInventoryInput(visible);
        ToggleInteractionConstructionInput(visible);
        ToggleBuildPlayerInput(visible);
    }
}
