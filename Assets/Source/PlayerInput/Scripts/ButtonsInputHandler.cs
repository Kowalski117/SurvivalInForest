using UnityEngine;

public class ButtonsInputHandler : MonoBehaviour
{
    [SerializeField] protected HotbarDisplay HotbarDisplay;
    [SerializeField] protected BuildTool BuildTool;
    [SerializeField] protected ManualWorkbench ManualWorkbench;
    [SerializeField] protected ExchangeHandler ExchangeHandler;
    [SerializeField] protected ChestHandler ChestHandler;
    [SerializeField] protected InteractorConstruction Interactor;

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
        HotbarDisplay.OnItemSwitched += Toggle;
        BuildTool.OnBuildingCreated += EnableConstructionMode;
        BuildTool.OnBuildingCompleted += TurnOffConstructionMode;
        BuildTool.OnBuildingDestroyed += TurnOffConstructionMode;
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
        HotbarDisplay.OnItemSwitched -= Toggle;
        BuildTool.OnBuildingCreated -= EnableConstructionMode;
        BuildTool.OnBuildingCompleted -= TurnOffConstructionMode;
        BuildTool.OnBuildingDestroyed -= TurnOffConstructionMode;
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
        _start—onversationButton.Close();
        _toggleIntaractableButton.Close();
        _interactedConstructionButton.Close();
        _addFireButton.Close();
        _useItemButton.Close();
    }

    protected virtual void Toggle(InventorySlotUI inventorySlotUI)
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
            _interactedConstructionButton.Close();
            _toggleIntaractableButton.Close();
            _start—onversationButton.Close();
            _addFireButton.Close();
            _useItemButton.Close();
        }
    }

    protected virtual void TurnOffConstructionMode()
    {
        IsBuilding = false;
        Toggle(CurrentSlot);
    }

    private void EnableIntractableButton()
    {
        if (IsBuilding)
            return;

        _toggleIntaractableButton.Open();
    }

    private void TurnOffIntractableButton()
    {
        if (IsBuilding)
            return;

        _toggleIntaractableButton.Close();
    }

    private void EnableIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.Open();
    }

    private void TurnOffIntractableNPSButton()
    {
        if (IsBuilding)
            return;

        _start—onversationButton.Close();
    }

    private void EnableIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        if (Interactor.CurrentFire == null && Interactor.CurrentGardenBed == null)
            _interactedConstructionButton.Open();

        ToggleAddFireButton(CurrentSlot);
        ToggleUseItemButton(CurrentSlot);
    }

    private void TurnOffIntractableConstructionButton()
    {
        if (IsBuilding)
            return;

        _interactedConstructionButton.Close();
        _addFireButton.Close();
        _useItemButton.Close();
    }

    private void ToggleUseItemButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentGardenBed)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData is SeedItemData seedItemData)
            _useItemButton.Open();
        else
            _useItemButton.Close();
    }

    private void ToggleAddFireButton(InventorySlotUI inventorySlotUI)
    {
        if (IsBuilding || !Interactor.CurrentFire)
            return;

        if (inventorySlotUI.AssignedInventorySlot.ItemData && inventorySlotUI.AssignedInventorySlot.ItemData.GorenjeTime > 0)
            _addFireButton.Open();
        else
            _addFireButton.Close();
    }

    protected virtual void ToggleDestroyBuildingMode(bool isActive)
    {
        if (isActive)
        {
            _interactedConstructionButton.Close();
            _toggleIntaractableButton.Close();
            _start—onversationButton.Close();
        }
        else
            Toggle(CurrentSlot);
    }
}