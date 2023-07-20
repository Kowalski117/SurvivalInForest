using StarterAssets;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private HotbarDisplay _hotbarDisplay;

    [SerializeField] private ItemAnimation[] _items;

    private string _speed = "Speed";
    private string _pickUp = "PickUp";
    private string _build = "Build";
    private string _hit = "Hit";

    private string _pullItem = "GetItem";
    private string _removeItem = "RemoveItem";

    private string _axeBlow = "AxeBlow";
    private string _blowPickaxe = "BlowPickaxe";

    private string _pullBow = "PullBow";

    private bool _isItemInHand = false;
    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;

    private void Update()
    {
        if (_firstPersonController._isEnable)
            _handAnimator.SetFloat(_speed, (_firstPersonController.Speed / 10));
        else
            _handAnimator.SetFloat(_speed, 0);

        Init(_hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData);
    }

    //private void OnEnable()
    //{
    //    _hotbarDisplay.ItemClicked += Init;
    //}

    //private void OnDisable()
    //{
    //    _hotbarDisplay.ItemClicked -= Init;
    //}

    public void Init(InventoryItemData itemData)
    {
        _previousItemData = _currentItemData; 
        _currentItemData = itemData;

        if (_currentItemData != _previousItemData)
        {
            GetItem(_currentItemData);
        }
    }

    public void PickUp()
    {
        _handAnimator.SetTrigger(_pickUp);
    }

    public void Build()
    {
        TurnOffAnimations();
        _handAnimator.SetBool(_build, true);
    }

    public void Hit(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            foreach (var item in _items)
            {
                if (itemData == item.ItemData)
                {
                    if (itemData is ToolItemData toolItemData)
                    {
                        if (toolItemData.ToolType == ToolType.Axe)
                        {
                            _handAnimator.SetTrigger(_axeBlow);
                        }
                        else if(toolItemData.ToolType == ToolType.Pickaxe)
                        {
                            _handAnimator.SetTrigger(_blowPickaxe);
                        }
                        else if (toolItemData.ToolType == ToolType.Arm)
                        {
                            _handAnimator.SetTrigger(_hit);
                        }
                    }
                    else if(itemData is WeaponItemData weapon)
                    {
                        _handAnimator.SetTrigger(_hit);
                    }
                }
            }
        }
        else
        {
            _handAnimator.SetTrigger(_hit);
        }
    }

    public void GetItem(InventoryItemData itemData)
    {
        if (itemData != null)
        {
            bool itemFound = false;
            _currentItemData = itemData;
            foreach (var item in _items)
            {
                if (itemData == item.ItemData)
                {
                    if (itemData is WeaponItemData weaponItemData)
                    {
                        if (weaponItemData.WeaponType == WeaponType.RangedWeapon)
                        {
                            _handAnimator.SetTrigger(_pullBow);
                        }
                    }
                    else
                    {
                        _handAnimator.SetTrigger(_pullItem);
                    }
                    _isItemInHand = true;
                    itemFound = true;
                }
            }

            if (!itemFound && _isItemInHand)
            {
                _handAnimator.SetTrigger(_removeItem);
                _isItemInHand = false;
            }
        }
        else
        {
            if (_isItemInHand)
            {
                _handAnimator.SetTrigger(_removeItem);
                _isItemInHand = false;
            }

        }
    }

    public void PullItemAnimationEvent()
    {
        foreach (var item in _items)
        {
            if (_currentItemData == item.ItemData)
            {
                item.ToggleItem(true);
            }
            else
            {
                item.ToggleItem(false);
            }
        }
    }

    public void RemoveItemAnimationEvent()
    {
        if (_currentItemData == null)
            RemoveItemsAnimationEvent();
    }

    public void RemoveItemsAnimationEvent()
    {
        foreach (var item in _items)
        {
            item.ToggleItem(false);
        }
    }

    public void TurnOffAnimations()
    {
        _handAnimator.SetBool(_build, false);
    }
}

[System.Serializable]
public class ItemAnimation
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private Transform _item;

    public InventoryItemData ItemData => _itemData;

    public void ToggleItem(bool isActive)
    {
        if(_item != null)
            _item.gameObject.SetActive(isActive);
    }
}
