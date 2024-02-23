using UnityEngine;

public class MobileButtonsHandler : ButtonsInputHandler
{
    [SerializeField] private AnimationUI _attackButton;
    [SerializeField] private AnimationUI _aimButton;
    [SerializeField] private AnimationUI _sprintButton;
    [SerializeField] private AnimationUI _stealthButton;
    [SerializeField] private AnimationUI _toggleInventoryButton;
    [SerializeField] private AnimationUI _toggleQuestJournalButton;
    [SerializeField] private AnimationUI _rotateBuildButton;
    [SerializeField] private AnimationUI _destroyBuildButton;
    [SerializeField] private AnimationUI _putBuildButton;
    [SerializeField] private AnimationUI _toggleBuildModeButton;
    [SerializeField] private AnimationUI _removeBuildModeButton;

    protected override void ClearAll()
    {
        base.ClearAll();

        _aimButton.CloseAnimation();
        _putBuildButton.CloseAnimation();
        _destroyBuildButton.CloseAnimation();
        _rotateBuildButton.CloseAnimation();
        _removeBuildModeButton.CloseAnimation();
    }

    protected override void ToggleButtons(InventorySlotUI inventorySlotUI)
    {
        base.ToggleButtons(inventorySlotUI);

        _aimButton.CloseAnimation();

        if (inventorySlotUI.AssignedInventorySlot.ItemData is WeaponItemData weaponItemData && weaponItemData.WeaponType == WeaponType.RangedWeapon)
            _aimButton.OpenAnimation();
    }

    protected override void EnableConstructionMode()
    {
        base.EnableConstructionMode();

        if (IsBuilding)
        {
            _rotateBuildButton.OpenAnimation();
            _destroyBuildButton.OpenAnimation();
            _putBuildButton.OpenAnimation();

            _attackButton.CloseAnimation();
            _aimButton.CloseAnimation();
            _sprintButton.CloseAnimation();
            _stealthButton.CloseAnimation();
            _removeBuildModeButton.CloseAnimation();
            _toggleBuildModeButton.CloseAnimation();
            _toggleQuestJournalButton.CloseAnimation();
        }
    }

    protected override void TurnOffConstructionMode()
    {
        base.TurnOffConstructionMode();

        _rotateBuildButton.CloseAnimation();
        _destroyBuildButton.CloseAnimation();
        _putBuildButton.CloseAnimation();

        _attackButton.OpenAnimation();
        _sprintButton.OpenAnimation();
        _stealthButton.OpenAnimation();
        _toggleBuildModeButton.OpenAnimation();
        _toggleQuestJournalButton.OpenAnimation();
    }

    protected override void ToggleDestroyBuildingMode(bool isActive)
    {
        base.ToggleDestroyBuildingMode(isActive);

        if (isActive)
        {
            _removeBuildModeButton.OpenAnimation();

            _rotateBuildButton.CloseAnimation();
            _destroyBuildButton.CloseAnimation();
            _putBuildButton.CloseAnimation();
            _attackButton.CloseAnimation();
            _aimButton.CloseAnimation();
            _sprintButton.CloseAnimation();
            _stealthButton.CloseAnimation();
            _toggleQuestJournalButton.CloseAnimation();
        }
        else
        {
            _removeBuildModeButton.CloseAnimation();
            _attackButton.OpenAnimation();
            _sprintButton.OpenAnimation();
            _stealthButton.OpenAnimation();
            _toggleQuestJournalButton.OpenAnimation();
        }
    }
}
