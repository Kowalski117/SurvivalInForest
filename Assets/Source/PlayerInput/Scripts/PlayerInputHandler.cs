using BehaviorDesigner.Runtime.Tasks.Unity.UnityRenderer;
using StarterAssets;
using System.Collections;
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
    [SerializeField] private PlayerHealth _playerHealth;

    private bool _isCursorEnable;
    private bool _isPreviusCursorEnable;

    public FirstPersonController FirstPersonController => _firstPersonController;
    public InventoryPlayerInput InventoryPlayerInput => _inventoryPlayerInput;
    public InteractionPlayerInput InteractionPlayerInput => _interactionPlayerInput;
    public BuildPlayerInput BuildPlayerInput => _buildPlayerInput;
    public UIScreenPlayerInput ScreenPlayerInput => _screenPlayerInput;
    public SurvivalHandler SurvivalHandler => _survivalHandler;
    public BuildTool BuildTool => _buildTool;
    public Transform InventoryPanels => _inventoryPanels;
    public PlayerHealth PlayerHealth => _playerHealth;

    private void Start()
    {
        //ToggleAllParametrs(false);
        //SetCursorVisible(true);
        StartCoroutine(WaitForLoad(1f));
    }

    private void Update()
    {
        if (_isCursorEnable != _isPreviusCursorEnable)
            ToggleCursor(_isCursorEnable);
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleAllParametrs(false);
    }

    public void SetCursorVisible(bool visible)
    {
        _isPreviusCursorEnable = _isCursorEnable;
        _isCursorEnable = visible;
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
         _inventoryPanels.gameObject.SetActive(visible);
    }

    public void ToggleAllInput(bool visible)
    {
        TogglePersonController(visible);
        ToggleInventoryInput(visible);
        ToggleInteractionInput(visible);
        ToggleBuildPlayerInput(visible);
    }

    public void ToggleAllParametrs(bool visible)
    {
        SetCursorVisible(!visible);
        TogglePersonController(visible);
        _survivalHandler.SetEnable(visible);
        _survivalHandler.TimeHandler.ToggleEnable(visible);
    }

    private void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        _firstPersonController.ToggleCamera(!visible);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        ToggleCursor(_isCursorEnable);
    }
}
