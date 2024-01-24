using UnityEngine;

public class ButtonsInputHandler : MonoBehaviour
{
    [SerializeField] protected HotbarDisplay HotbarDisplay;
    [SerializeField] protected BuildTool BuildTool;
    [SerializeField] protected ManualWorkbench ManualWorkbench;
    [SerializeField] protected ExchangeHandler ExchangeHandler;
    [SerializeField] protected ChestHandler ChestHandler;
    [SerializeField] protected Interactor Interactor;

    [SerializeField] private Transform _interactedConstructionButton;
    [SerializeField] private Transform _addFireButton;
    [SerializeField] private Transform _toggleIntaractableButton;
    [SerializeField] private Transform _start—onversationButton;
    [SerializeField] private Transform _useItemButton;

    protected InventorySlotUI CurrentSlot;
    protected bool IsBuilding = false;

    protected virtual void Awake()
    {
        _start—onversationButton.gameObject.SetActive(false);
        _toggleIntaractableButton.gameObject.SetActive(false);
        _interactedConstructionButton.gameObject.SetActive(false);
        _addFireButton.gameObject.SetActive(false);
        _useItemButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        HotbarDisplay.OnItemSwitched += ToggleButtons;
        BuildTool.OnCreateBuild += EnableConstructionMode;
        BuildTool.OnCompletedBuild += TurnOffConstructionMode;
        BuildTool.OnDestroyBuild += TurnOffConstructionMode;
        BuildTool.OnDeleteModeChanged += ToggleDestroyBuildingMode;

        ManualWorkbench.OnInteractionStarted += EnableIntractableButton;
        ManualWorkbench.OnInteractionFinished += TurnOffIntractableButton;
        ExchangeHandler.OnInteractionStarted += EnableIntractableButton;
        ExchangeHandler.OnInteractionStarted += EnableIntractableNPSButton;
        ExchangeHandler.OnInteractionFinished += TurnOffIntractableButton;
        ExchangeHandler.OnInteractionFinished += TurnOffIntractableNPSButton;
        ChestHandler.OnInteractionStarted += EnableIntractableButton;
        ChestHandler.OnInteractionFinished += TurnOffIntractableButton;
        Interactor.OnInteractionStarted += EnableIntractableConstructionButton;
        Interactor.OnInteractionFinished += TurnOffIntractableConstructionButton;
    }

    private void OnDisable()
    {
        HotbarDisplay.OnItemSwitched -= ToggleButtons;
        BuildTool.OnCreateBuild -= EnableConstructionMode;
        BuildTool.OnCompletedBuild -= TurnOffConstructionMode;
        BuildTool.OnDestroyBuild -= TurnOffConstructionMode;
        BuildTool.OnDeleteModeChanged -= ToggleDestroyBuildingMode;

        ManualWorkbench.OnInteractionStarted -= EnableIntractableButton;
        ManualWorkbench.OnInteractionFinished -= TurnOffIntractableButton;
        ExchangeHandler.OnInteractionStarted -= EnableIntractableButton;
        ExchangeHandler.OnInteractionStarted -= EnableIntractableNPSButton;
        ExchangeHandler.OnInteractionFinished -= TurnOffIntractableButton;
        ExchangeHandler.OnInteractionFinished -= TurnOffIntractableNPSButton;
        ChestHandler.OnInteractionStarted -= EnableIntractableButton;
        ChestHandler.OnInteractionFinished -= TurnOffIntractableButton;
        Interactor.OnInteractionStarted -= EnableIntractableConstructionButton;
        Interactor.OnInteractionFinished -= TurnOffIntractableConstructionButton;
    }

    protected virtual void ToggleButtons(InventorySlotUI inventorySlotUI)
    {
        CurrentSlot = inventorySlotUI;

        if (IsBuilding)
            return;

        ToggleAddFireButton(CurrentSlot);
        ToggleUseItemButton(CurrentSlot);
    }

    protected virtual void EnableConstructionMode()
    {
        IsBuilding = true;

        if (IsBuilding)
        {
            _interactedConstructionButton.gameObject.SetActive(false);
            _toggleIntaractableButton.gameObject.SetActive(false);
            _start—onversationButton.gameObject.SetActive(false);
            _addFireButton.gameObject.SetActive(false);
            _useItemButton.gameObject.SetActive(false);
        }
    }

    protected virtual void TurnOffConstructionMode()
    {
        IsBuilding = false;
        ToggleButtons(CurrentSlot);
    }

    private void EnableIntractableButton()
    {
        if (IsBuilding)
            return;

        _toggleIntaractableButton.gameObject.SetActive(true);
    }

    private void TurnOffIntractableButton()
    {
        if (IsBuilding)
            return;

        _toggleIntaractableButton.gameObject.SetActive(false);
    }

    private void EnableIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.gameObject.SetActive(true);
    }

    private void TurnOffIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.gameObject.SetActive(false);
    }

    private void EnableIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        if (Interactor.CurrentFire == null && Interactor.CurrentGardenBed == null)
            _interactedConstructionButton.gameObject.SetActive(true);

        ToggleAddFireButton(CurrentSlot);
        ToggleUseItemButton(CurrentSlot);
    }

    private void TurnOffIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        _interactedConstructionButton.gameObject.SetActive(false);
        _addFireButton.gameObject.SetActive(false);
        _useItemButton.gameObject.SetActive(false);
    }

    private void ToggleUseItemButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentGardenBed)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData is SeedItemData seedItemData)
            _useItemButton.gameObject.SetActive(true);
        else
            _useItemButton.gameObject.SetActive(false);
    }

    private void ToggleAddFireButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentFire)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData && inventorySlotUI.AssignedInventorySlot.ItemData.GorenjeTime > 0)
            _addFireButton.gameObject.SetActive(true);
        else
            _addFireButton.gameObject.SetActive(false);
    }

    protected virtual void ToggleDestroyBuildingMode(bool isActive)
    {
        if (isActive)
        {
            _interactedConstructionButton.gameObject.SetActive(false);
            _toggleIntaractableButton.gameObject.SetActive(false);
            _start—onversationButton.gameObject.SetActive(false);
        }
        else
        {
            ToggleButtons(CurrentSlot);
        }
    }
}
