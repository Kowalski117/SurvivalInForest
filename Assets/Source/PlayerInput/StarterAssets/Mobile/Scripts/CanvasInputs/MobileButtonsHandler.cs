using UnityEngine;
using UnityEngine.UI;

public class MobileButtonsHandler : MonoBehaviour
{
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private ManualWorkbench _manualWorkbench;
    [SerializeField] private ExchangeHandler _exchangeHandler;
    [SerializeField] private ChestHandler _chestHandler;
    [SerializeField] private Interactor _interactor;

    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _aimButton;
    [SerializeField] private Button _sprintButton;
    [SerializeField] private Button _stealthButton;
    [SerializeField] private Button _toggleInventoryButton;
    [SerializeField] private Button _interactedConstructionButton;
    [SerializeField] private Button _addFireButton;
    [SerializeField] private Button _toggleIntaractableButton;
    [SerializeField] private Button _useItemButton;
    [SerializeField] private Button _removeItemButton;
    [SerializeField] private Button _rotateBuildButton;
    [SerializeField] private Button _destroyBuildButton;
    [SerializeField] private Button _putBuildButton;

    private InventorySlotUI _currentSlot;
    private bool _isBuilding = false;

    private void Awake()
    {
        _aimButton.gameObject.SetActive(false);
        _putBuildButton.gameObject.SetActive(false);
        _destroyBuildButton.gameObject.SetActive(false);
        _rotateBuildButton.gameObject.SetActive(false);
        _useItemButton.gameObject.SetActive(false);
        _toggleIntaractableButton.gameObject.SetActive(false);
        _interactedConstructionButton.gameObject.SetActive(false);
        _addFireButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _hotbarDisplay.OnItemSwitched += ToggleButtons;
        _buildTool.OnCreateBuild += EnableConstructionMode;
        _buildTool.OnCompletedBuild += TurnOffConstructionMode;
        _buildTool.OnDestroyBuild += TurnOffConstructionMode;

        _manualWorkbench.OnInteractionStarted += EnableIntractableButton;
        _manualWorkbench.OnInteractionFinished += TurnOffIntractableButton;
        _exchangeHandler.OnInteractionStarted += EnableIntractableButton;
        _exchangeHandler.OnInteractionFinished += TurnOffIntractableButton;
        _chestHandler.OnInteractionStarted += EnableIntractableButton;
        _chestHandler.OnInteractionFinished += TurnOffIntractableButton;
        _interactor.OnInteractionStarted += EnableIntractableConstructionButton;
        _interactor.OnInteractionFinished += TurnOffIntractableConstructionButton;
    }

    private void OnDisable()
    {
        _hotbarDisplay.OnItemSwitched -= ToggleButtons;
        _buildTool.OnCreateBuild -= EnableConstructionMode;
        _buildTool.OnCompletedBuild -= TurnOffConstructionMode;
        _buildTool.OnDestroyBuild -= TurnOffConstructionMode;

        _manualWorkbench.OnInteractionStarted -= EnableIntractableButton;
        _manualWorkbench.OnInteractionFinished -= TurnOffIntractableButton;
        _exchangeHandler.OnInteractionStarted -= EnableIntractableButton;
        _exchangeHandler.OnInteractionFinished -= TurnOffIntractableButton;
        _chestHandler.OnInteractionStarted -= EnableIntractableButton;
        _chestHandler.OnInteractionFinished -= TurnOffIntractableButton;
        _interactor.OnInteractionStarted -= EnableIntractableConstructionButton;
        _interactor.OnInteractionFinished -= TurnOffIntractableConstructionButton;
    }

    private void ToggleButtons(InventorySlotUI inventorySlotUI)
    {
        _currentSlot = inventorySlotUI;

        if (_isBuilding)
            return;

        _aimButton.gameObject.SetActive(false);
        _useItemButton.gameObject.SetActive(false);
        _addFireButton.gameObject.SetActive(false);


        if (inventorySlotUI.AssignedInventorySlot.ItemData is FoodItemData foodItemData || inventorySlotUI.AssignedInventorySlot.ItemData is SeedItemData seedItemData)
        {
            _useItemButton.gameObject.SetActive(true);

        }
        else if (inventorySlotUI.AssignedInventorySlot.ItemData is WeaponItemData weaponItemData && weaponItemData.WeaponType == WeaponType.RangedWeapon)
        {
            _aimButton.gameObject.SetActive(true);
        }
        //else
        //{
        //    _useItemButton.gameObject.SetActive(false);
        //    _aimButton.gameObject.SetActive(false);
        //}

        ToggleAddFireButton(inventorySlotUI);
    }

    private void EnableConstructionMode()
    {
        _isBuilding = true;

        if (_isBuilding)
        {
            _rotateBuildButton.gameObject.SetActive(true);
            _destroyBuildButton.gameObject.SetActive(true);
            _putBuildButton.gameObject.SetActive(true);

            _attackButton.gameObject.SetActive(false);
            _aimButton.gameObject.SetActive(false);
            _sprintButton.gameObject.SetActive(false);
            _stealthButton.gameObject.SetActive(false);
            _interactedConstructionButton.gameObject.SetActive(false);
            _toggleIntaractableButton.gameObject.SetActive(false);
            _useItemButton.gameObject.SetActive(false);
            _removeItemButton.gameObject.SetActive(false);
        }
    }

    private void TurnOffConstructionMode()
    {
        _isBuilding = false;

        _rotateBuildButton.gameObject.SetActive(false);
        _destroyBuildButton.gameObject.SetActive(false);
        _putBuildButton.gameObject.SetActive(false);

        _attackButton.gameObject.SetActive(true);
        _sprintButton.gameObject.SetActive(true);
        _stealthButton.gameObject.SetActive(true);
        _removeItemButton.gameObject.SetActive(true);

        ToggleButtons(_currentSlot);
    }

    private void EnableIntractableButton()
    {
        if (_isBuilding)
            return;

        _toggleIntaractableButton.gameObject.SetActive(true);
    }

    private void TurnOffIntractableButton()
    {
        if (_isBuilding)
            return;

        _toggleIntaractableButton.gameObject.SetActive(false);
    }

    private void EnableIntractableConstructionButton()
    {
        if (_isBuilding)
            return;

        if(_interactor.CurrentFire == null)
            _interactedConstructionButton.gameObject.SetActive(true);
        ToggleAddFireButton(_currentSlot);
    }

    private void TurnOffIntractableConstructionButton()
    {
        if (_isBuilding)
            return;

        _interactedConstructionButton.gameObject.SetActive(false);
        _addFireButton.gameObject.SetActive(false);
    }

    private void ToggleAddFireButton(InventorySlotUI inventorySlotUI)
    {
        if (_isBuilding || !_interactor.CurrentFire)
            return;

        if( inventorySlotUI.AssignedInventorySlot.ItemData && inventorySlotUI.AssignedInventorySlot.ItemData.GorenjeTime > 0)
            _addFireButton.gameObject.SetActive(true);
        else
            _addFireButton.gameObject.SetActive(false);       
    }
}
