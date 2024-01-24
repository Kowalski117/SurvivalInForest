using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PlayerHandler : MonoBehaviour
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
    [SerializeField] private Compass _compass;

    private bool _isCursorEnable;
    private bool _isCursorEnablePrevious;
    private bool _isAllParametrsEnable;
    private bool _isAllParametrsEnablePrevious;
    private bool _isControllerActive;
    private bool _isControllerActivePrevious;

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
    public Compass Compass => _compass;
    public bool IsCursorEnable => _isCursorEnable;

    private void Start()
    {
        StartCoroutine(WaitForLoad(1f));
    }

    private IEnumerator WaitForLoad(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleAllParametrs(false);
        ToggleAllInput(false);
        TogglePersonController(false);
        SetCursorVisible(true);
    }

    public void SetCursorVisible(bool visible)
    {
        if (_isCursorEnablePrevious && !visible)
        {
            _isCursorEnablePrevious = false;
            return;
        }

        _isCursorEnablePrevious = _isCursorEnable;
        _isCursorEnable = visible;
        ToggleCursor(_isCursorEnable);
    }

    public void TogglePersonController(bool visible)
    {
        //if (_isControllerActivePrevious && !visible)
        //{
        //    _isControllerActivePrevious = false;
        //    return;
        //}
        //_isControllerActivePrevious = _isControllerActive;
        //_isControllerActive = visible;
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
        _firstPersonController.ToggleCamera(visible);
    }

    private void ToggleCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        ToggleCamera(!visible);
    }

    private void OnApplicationFocus(bool focus)
    {
        if(_isAllParametrsEnablePrevious && !focus)
        {
            _isAllParametrsEnablePrevious = false;
            return;
        }

        _isAllParametrsEnablePrevious = _isAllParametrsEnable;
        _isAllParametrsEnable = focus;
        ToggleAllParametrs(focus);
    }
}
