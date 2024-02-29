using System;
using UnityEngine;

public class PlayerEquipmentHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _inventory;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private ToolItemData _armItemData;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;
    [SerializeField] private MainClock _timeHandler;

    private WeaponItemData _currentWeapon;
    private ToolItemData _currentTool;
    private InventoryItemData _currentItemData;
    private InventorySlot _currentInventorySlot;

    private bool _isActiveGoal = false;
    private bool _isEnable;
    private float _hourInSeconds = 3600;
    private float _maxDelayFire = 10f;
    private float _nextFireDelay;

    public event Action<InventoryItemData> OnUpdateItemData;
    public event Action<WeaponItemData> OnUpdateWeaponItemData;
    public event Action<ToolItemData> OnUpdateToolItemData;

    public InventorySlot CurrentInventorySlot => _currentInventorySlot;
    public WeaponItemData CurrentWeapon => _currentWeapon;
    public ToolItemData CurrentTool => _currentTool;

    private void Awake()
    {
        _nextFireDelay = _maxDelayFire;
    }

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttacked += UseItem;
        _hotbarDisplay.OnItemSwitched += UpdateItemData;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnAttacked -= UseItem;
        _hotbarDisplay.OnItemSwitched -= UpdateItemData;
    }

    private void Update()
    {
        if (_nextFireDelay <= _maxDelayFire)
            _nextFireDelay += Time.deltaTime;

        if (_isEnable)
        {
            if (_currentWeapon != null && _currentWeapon.WeaponType == WeaponType.MeleeWeapon)
            {
                if (_nextFireDelay > _currentWeapon.Speed)
                {
                    _playerAnimation.Hit(_isActiveGoal);

                    _nextFireDelay = 0;
                }
            }
            else if (_currentTool != null && _currentWeapon == null)
            {
                if (_nextFireDelay > _currentTool.Speed)
                {
                    _playerAnimation.Hit(_isActiveGoal);

                    _nextFireDelay = 0;
                }
            }
        }
    }

    public void SetActiveGoal(bool isActive)
    {
        _isActiveGoal = isActive;
    }

    public bool IsShoot()
    {
        if (_nextFireDelay > _currentWeapon.Speed)
        {
            if (_inventory.RemoveItem(_currentWeapon.Bullet.ItemData, 1))
            {
                _playerAnimation.Hit(true);
                _playerAudioHandler.PlayOneShot(_currentWeapon.MuzzleSound);
                _nextFireDelay = 0;
                return true;
            }
        }

        return false;
    }

    public void UpdateDurabilityItem(InventorySlot inventorySlot)
    {
        if (_hotbarDisplay.CurrentSlot.AssignedInventorySlot.ItemData is FoodItemData foodItemData && inventorySlot == _hotbarDisplay.CurrentSlot.AssignedInventorySlot || inventorySlot.ItemData is ToolItemData toolItemData && toolItemData.ToolType == ToolType.Torch || inventorySlot.ItemData is ClothesItemData clothesItemData)
            return;

        if (inventorySlot.Durability > 0)
        {
            inventorySlot.LowerStrength(1);

            if (inventorySlot.Durability <= 0)
            {
                inventorySlot.UpdateDurabilityIfNeeded();
                _inventory.RemoveSlot(inventorySlot, 1);
            }
        }
    }

    public void UpdateDurabilityWithGameTime(InventorySlot inventorySlot)
    {
        if (inventorySlot.Durability > 0)
        {
            inventorySlot.LowerStrength(_timeHandler.TimeMultiplier / _hourInSeconds * Time.deltaTime);

            if (inventorySlot.Durability <= 0)
            {
                inventorySlot.UpdateDurabilityIfNeeded();

                if (inventorySlot.ItemData is ClothesItemData clothesItemData && clothesItemData.DischargedItem != null)
                    _inventory.AddItem(clothesItemData.DischargedItem, 1);

                _inventory.RemoveSlot(inventorySlot, 1);
            }
        }
    }

    private void UpdateItemData(InventorySlotUI slotUI)
    {
        _nextFireDelay = _maxDelayFire;
        _currentInventorySlot = slotUI.AssignedInventorySlot;
        _currentItemData = _currentInventorySlot.ItemData;
        OnUpdateItemData?.Invoke(_currentItemData);
        InitWeapon(_currentItemData);
        InitTool(_currentItemData);
    }

    private void InitWeapon(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is WeaponItemData weaponItemData)
                _currentWeapon = weaponItemData;
            else
                _currentWeapon = null;
        }
        else
            _currentWeapon = null;

        OnUpdateWeaponItemData?.Invoke(_currentWeapon);
    }

    private void InitTool(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            if (itemData is ToolItemData toolItemData)
                _currentTool = toolItemData;
            else
                _currentTool = _armItemData;
        }
        else
            _currentTool = _armItemData;

        OnUpdateToolItemData?.Invoke(_currentTool);
    }

    private void UseItem(bool isActive)
    {
        _isEnable = isActive;
    }
}
