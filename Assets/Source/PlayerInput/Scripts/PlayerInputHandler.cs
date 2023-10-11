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
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private Transform _inventoryPanels;

    public FirstPersonController FirstPersonController => _firstPersonController;
    public InventoryPlayerInput InventoryPlayerInput => _inventoryPlayerInput;
    public InteractionPlayerInput InteractionPlayerInput => _interactionPlayerInput;
    public BuildPlayerInput BuildPlayerInput => _buildPlayerInput;
    public UIScreenPlayerInput ScreenPlayerInput => _screenPlayerInput;
    public SurvivalHandler SurvivalHandler => _survivalHandler;
    public BuildTool BuildTool => _buildTool;
    public Transform InventoryPanels => _inventoryPanels;

    private void Start()
    {
        SetCursorVisible(true);
        ToggleAllParametrs(false);
    }

    public void SetCursorVisible(bool visible)
    {
        _firstPersonController.ToggleCamera(!visible);

        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePersonController(bool visible)
    {
        _firstPersonController.ToggleCamera(visible);
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
        //ToggleInteractionConstructionInput(visible);
        ToggleBuildPlayerInput(visible);
    }

    public void ToggleAllParametrs(bool visible)
    {
        SetCursorVisible(!visible);
        TogglePersonController(visible);
        _survivalHandler.SetEnable(visible);
        _survivalHandler.TimeHandler.SetEnable(visible);
    }
}
