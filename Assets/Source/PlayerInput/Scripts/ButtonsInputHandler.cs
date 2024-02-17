using UnityEngine;

public class ButtonsInputHandler : MonoBehaviour
{
    [SerializeField] protected HotbarDisplay HotbarDisplay;
    [SerializeField] protected BuildTool BuildTool;
    [SerializeField] protected ManualWorkbench ManualWorkbench;
    [SerializeField] protected ExchangeHandler ExchangeHandler;
    [SerializeField] protected ChestHandler ChestHandler;
    [SerializeField] protected Interactor Interactor;

    [SerializeField] private AnimationUI _interactedConstructionButton;
    [SerializeField] private AnimationUI _addFireButton;
    [SerializeField] private AnimationUI _toggleIntaractableButton;
    [SerializeField] private AnimationUI _start—onversationButton;
    [SerializeField] private AnimationUI _useItemButton;

    protected InventorySlotUI CurrentSlot;
    protected bool IsBuilding = false;

    protected virtual void Awake()
    {
        ClearAll();
    }

    private void OnEnable()
    {
        HotbarDisplay.OnItemSwitched += ToggleButtons;
        BuildTool.OnCreateBuilding += EnableConstructionMode;
        BuildTool.OnCompletedBuilding += TurnOffConstructionMode;
        BuildTool.OnDestroyBuilding += TurnOffConstructionMode;
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
        BuildTool.OnCreateBuilding -= EnableConstructionMode;
        BuildTool.OnCompletedBuilding -= TurnOffConstructionMode;
        BuildTool.OnDestroyBuilding -= TurnOffConstructionMode;
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

    protected virtual void ClearAll()
    {
        _start—onversationButton.CloseAnimation();
        _toggleIntaractableButton.CloseAnimation();
        _interactedConstructionButton.CloseAnimation();
        _addFireButton.CloseAnimation();
        _useItemButton.CloseAnimation();
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
            _interactedConstructionButton.CloseAnimation();
            _toggleIntaractableButton.CloseAnimation();
            _start—onversationButton.CloseAnimation();
            _addFireButton.CloseAnimation();
            _useItemButton.CloseAnimation();
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

        _toggleIntaractableButton.OpenAnimation();
    }

    private void TurnOffIntractableButton()
    {
        if (IsBuilding)
            return;

        _toggleIntaractableButton.CloseAnimation();
    }

    private void EnableIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.OpenAnimation();
    }

    private void TurnOffIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.CloseAnimation();
    }

    private void EnableIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        if (Interactor.CurrentFire == null && Interactor.CurrentGardenBed == null)
            _interactedConstructionButton.OpenAnimation();

        ToggleAddFireButton(CurrentSlot);
        ToggleUseItemButton(CurrentSlot);
    }

    private void TurnOffIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        _interactedConstructionButton.CloseAnimation();
        _addFireButton.CloseAnimation();
        _useItemButton.CloseAnimation();
    }

    private void ToggleUseItemButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentGardenBed)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData is SeedItemData seedItemData)
            _useItemButton.OpenAnimation();
        else
            _useItemButton.CloseAnimation();
    }

    private void ToggleAddFireButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentFire)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData && inventorySlotUI.AssignedInventorySlot.ItemData.GorenjeTime > 0)
            _addFireButton.OpenAnimation();
        else
            _addFireButton.CloseAnimation();
    }

    protected virtual void ToggleDestroyBuildingMode(bool isActive)
    {
        if (isActive)
        {
            _interactedConstructionButton.CloseAnimation();
            _toggleIntaractableButton.CloseAnimation();
            _start—onversationButton.CloseAnimation();
        }
        else
        {
            ToggleButtons(CurrentSlot);
        }
    }
}