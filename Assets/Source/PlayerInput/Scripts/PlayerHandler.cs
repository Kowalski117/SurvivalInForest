using StarterAssets;
using System;
using System.Collections;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private const float Delay = 1;

    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private InventoryPlayerInput _inventoryPlayerInput;
    [SerializeField] private InteractionPlayerInput _interactionPlayerInput;
    [SerializeField] private BuildPlayerInput _buildPlayerInput;
    [SerializeField] private ScreenPlayerInput _screenPlayerInput;

    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private SurvivalHandler _survivalHandler;
    [SerializeField] private ScreenAnimation _inventoryPanels;

    private WaitForSeconds _startWait = new WaitForSeconds(Delay);

    private bool _isControllerActive;
    private bool _isControllerActivePrevious;

    private bool _isCursorEnable;
    private bool _isCursorEnablePrevious;

    private bool _isAllParametrsEnable;
    private bool _isAllParametrsEnablePrevious;

    private bool _isColliderActive;
    private bool _isColliderActivePrevious;

    private bool _isCameraActive;
    private bool _isCameraActivePrevious;

    public FirstPersonController FirstPersonController => _firstPersonController;
    public HotbarDisplay HotbarDisplay => _hotbarDisplay;
    public InventoryPlayerInput InventoryPlayerInput => _inventoryPlayerInput;
    public InteractionPlayerInput InteractionPlayerInput => _interactionPlayerInput;
    public BuildPlayerInput BuildPlayerInput => _buildPlayerInput;
    public ScreenPlayerInput ScreenPlayerInput => _screenPlayerInput;
    public SurvivalHandler SurvivalHandler => _survivalHandler;
    public BuildTool BuildTool => _buildTool;

    private IEnumerator Start()
    {
        yield return _startWait;

        ToggleAllParametrs(false);
        ToggleAllInput(false);
        TogglePersonController(false);
        SetCursorVisible(true);
    }

    public void SetActiveCollider(bool isActive)
    {
        ToggleActive(isActive, ref _isColliderActive, ref _isColliderActivePrevious, () => _characterController.enabled = _isColliderActive);
    }

    public void SetCursorVisible(bool visible)
    {
        ToggleActive(visible, ref _isCursorEnable, ref _isCursorEnablePrevious, () => ToggleCursor(_isCursorEnable));
    }

    public void TogglePersonController(bool visible)
    {
        ToggleActive(visible, ref _isControllerActive, ref _isControllerActivePrevious, () => _firstPersonController.TogglePersonController(_isControllerActive));
    }

    public void ToggleHotbarDisplay(bool visible)
    {
        _hotbarDisplay.Toggle(visible);
    }

    public void ToggleInventoryInput(bool visible)
    {
        _inventoryPlayerInput.enabled = visible;
    } 
    
    public void ToggleScreenPlayerInput(bool visible)
    {
        _screenPlayerInput.enabled = visible;
    }

    public void ToggleInteractionInput(bool visible)
    {
        _interactionPlayerInput.SetEnable(visible);
    }

    public void ToggleBuildPlayerInput(bool visible)
    {
        _buildPlayerInput.enabled = visible;
    }

    public void ToggleInventoryPanels(bool visible)
    {
         if(visible)
            _inventoryPanels.Open();
         else
            _inventoryPanels.Close();
    }

    public void ToggleAllInput(bool visible)
    {
        ToggleInventoryInput(visible);
        ToggleInteractionInput(visible);
        ToggleBuildPlayerInput(visible);
    }

    public void ToggleAllParametrs(bool visible)
    {
        _isAllParametrsEnable = visible;
        _survivalHandler.SetEnable(visible);
        _survivalHandler.TimeHandler.ToggleEnable(visible);
    }

    public void ToggleCamera(bool visible)
    {
        ToggleActive(visible, ref _isCameraActive, ref _isCameraActivePrevious, () => _firstPersonController.ToggleCamera(_isCameraActive));
    }

    private void ToggleActive(bool visible, ref bool isActive, ref bool isActivePrevious, Action OnToggled)
    {
        if (isActivePrevious && !visible)
        {
            isActivePrevious = false;
            return;
        }

        isActivePrevious = isActive;
        isActive = visible;

        OnToggled?.Invoke();
    }

    private void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleCamera(!visible);
    }

    private void OnApplicationFocus(bool focus)
    {
        ToggleActive(focus, ref _isAllParametrsEnable, ref _isAllParametrsEnablePrevious, () => ToggleAllParametrs(_isAllParametrsEnable));
    }
}
