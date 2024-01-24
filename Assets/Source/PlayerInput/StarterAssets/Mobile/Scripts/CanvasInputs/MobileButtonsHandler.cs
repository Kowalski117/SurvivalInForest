using UnityEngine;

public class MobileButtonsHandler : ButtonsInputHandler
{
    [SerializeField] private Transform _attackButton;
    [SerializeField] private Transform _aimButton;
    [SerializeField] private Transform _sprintButton;
    [SerializeField] private Transform _stealthButton;
    [SerializeField] private Transform _toggleInventoryButton;
    [SerializeField] private Transform _rotateBuildButton;
    [SerializeField] private Transform _destroyBuildButton;
    [SerializeField] private Transform _putBuildButton;
    [SerializeField] private Transform _toggleBuildModeButton;
    [SerializeField] private Transform _removeBuildModeButton;

    protected override void Awake()
    {
        base.Awake();

        _aimButton.gameObject.SetActive(false);
        _putBuildButton.gameObject.SetActive(false);
        _destroyBuildButton.gameObject.SetActive(false);
        _rotateBuildButton.gameObject.SetActive(false);
        _removeBuildModeButton.gameObject.SetActive(false);
    }

    protected override void ToggleButtons(InventorySlotUI inventorySlotUI)
    {
        base.ToggleButtons(inventorySlotUI);

        _aimButton.gameObject.SetActive(false);

        if (inventorySlotUI.AssignedInventorySlot.ItemData is WeaponItemData weaponItemData && weaponItemData.WeaponType == WeaponType.RangedWeapon)
            _aimButton.gameObject.SetActive(true);
    }

    protected override void EnableConstructionMode()
    {
        base.EnableConstructionMode();

        if (IsBuilding)
        {
            _rotateBuildButton.gameObject.SetActive(true);
            _destroyBuildButton.gameObject.SetActive(true);
            _putBuildButton.gameObject.SetActive(true);

            _attackButton.gameObject.SetActive(false);
            _aimButton.gameObject.SetActive(false);
            _sprintButton.gameObject.SetActive(false);
            _stealthButton.gameObject.SetActive(false);
            _removeBuildModeButton.gameObject.SetActive(false);
            _toggleBuildModeButton.gameObject.SetActive(false);
        }
    }

    protected override void TurnOffConstructionMode()
    {
        base.TurnOffConstructionMode();

        _rotateBuildButton.gameObject.SetActive(false);
        _destroyBuildButton.gameObject.SetActive(false);
        _putBuildButton.gameObject.SetActive(false);

        _attackButton.gameObject.SetActive(true);
        _sprintButton.gameObject.SetActive(true);
        _stealthButton.gameObject.SetActive(true);
        _toggleBuildModeButton.gameObject.SetActive(true);
    }

    protected override void ToggleDestroyBuildingMode(bool isActive)
    {
        base.ToggleDestroyBuildingMode(isActive);

        if (isActive)
        {
            _removeBuildModeButton.gameObject.SetActive(true);

            _rotateBuildButton.gameObject.SetActive(false);
            _destroyBuildButton.gameObject.SetActive(false);
            _putBuildButton.gameObject.SetActive(false);
            _attackButton.gameObject.SetActive(false);
            _aimButton.gameObject.SetActive(false);
            _sprintButton.gameObject.SetActive(false);
            _stealthButton.gameObject.SetActive(false);
        }
        else
        {
            _removeBuildModeButton.gameObject.SetActive(false);
            _attackButton.gameObject.SetActive(true);
            _sprintButton.gameObject.SetActive(true);
            _stealthButton.gameObject.SetActive(true);
        }
    }
}
