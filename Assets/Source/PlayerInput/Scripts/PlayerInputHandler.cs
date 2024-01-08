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
    [SerializeField] private Interactor _interactor;
    [SerializeField] private LoadPanel _loadPanel;

    private bool _isCursorEnable;

    public FirstPersonController FirstPersonController => _firstPersonController;
    public HotbarDisplay HotbarDisplay => _hotbarDisplay;
    public InventoryPlayerInput InventoryPlayerInput => _inventoryPlayerInput;
    public InteractionPlayerInput InteractionPlayerInput => _interactionPlayerInput;
    public BuildPlayerInput BuildPlayerInput => _buildPlayerInput;
    public UIScreenPlayerInput ScreenPlayerInput => _screenPlayerInput;
    public SurvivalHandler SurvivalHandler => _survivalHandler;
    public BuildTool BuildTool => _buildTool;
    public Transform InventoryPanels => _inventoryPanels;
    public PlayerHealth PlayerHealth => _playerHealth;
    public Interactor Interactor => _interactor;
    public LoadPanel LoadPanel => _loadPanel;
    public bool IsCursorEnable => _isCursorEnable;

    private void Start()
    {
        StartCoroutine(WaitForLoad(1f));
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleAllParametrs(false);
    }

    public void SetCursorVisible(bool visible)
    {
        _isCursorEnable = visible;
        ToggleCursor(_isCursorEnable);
    }

    public void TogglePersonController(bool visible)
    {
        ToggleCameraPersonController(visible);
        _firstPersonController.TogglePersonController(visible);
    }

    public void ToggleCameraPersonController(bool visible)
    {
        _firstPersonController.ToggleCamera(visible);
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
        ToggleCameraPersonController(visible);
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
        ToggleCursor(hasFocus);
    }
}
