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

        _aimButton.Close();
        _putBuildButton.Close();
        _destroyBuildButton.Close();
        _rotateBuildButton.Close();
        _removeBuildModeButton.Close();
    }

    protected override void Toggle(InventorySlotUI inventorySlotUI)
    {
        base.Toggle(inventorySlotUI);

        _aimButton.Close();

        if (inventorySlotUI.AssignedInventorySlot.ItemData is WeaponItemData weaponItemData && weaponItemData.WeaponType == WeaponType.RangedWeapon)
            _aimButton.Open();
    }

    protected override void EnableConstructionMode()
    {
        base.EnableConstructionMode();

        if (IsBuilding)
        {
            _rotateBuildButton.Open();
            _destroyBuildButton.Open();
            _putBuildButton.Open();

            _attackButton.Close();
            _aimButton.Close();
            _sprintButton.Close();
            _stealthButton.Close();
            _removeBuildModeButton.Close();
            _toggleBuildModeButton.Close();
            _toggleQuestJournalButton.Close();
        }
    }

    protected override void TurnOffConstructionMode()
    {
        base.TurnOffConstructionMode();

        _rotateBuildButton.Close();
        _destroyBuildButton.Close();
        _putBuildButton.Close();

        _attackButton.Open();
        _sprintButton.Open();
        _stealthButton.Open();
        _toggleBuildModeButton.Open();
        _toggleQuestJournalButton.Open();
    }

    protected override void ToggleDestroyBuildingMode(bool isActive)
    {
        base.ToggleDestroyBuildingMode(isActive);

        if (isActive)
        {
            _removeBuildModeButton.Open();

            _rotateBuildButton.Close();
            _destroyBuildButton.Close();
            _putBuildButton.Close();
            _attackButton.Close();
            _aimButton.Close();
            _sprintButton.Close();
            _stealthButton.Close();
            _toggleQuestJournalButton.Close();
        }
        else
        {
            _removeBuildModeButton.Close();
            _attackButton.Open();
            _sprintButton.Open();
            _stealthButton.Open();
            _toggleQuestJournalButton.Open();
        }
    }
}
