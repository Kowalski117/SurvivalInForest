using StarterAssets;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssets;
    [SerializeField] private HotbarDisplay _hotbarDisplay;

    [SerializeField] private ItemAnimator[] _itemsAnimator;
    [SerializeField] private ItemAnimator _defoultItem;

    private ItemAnimator _handAnimator;

    private string _speed = "Speed";
    private string _pickUp = "PickUp";
    private string _build = "Build";
    private string[] _hits = { "Hit", "Hit1"};
    private string _aim = "Aim";
    private string _pullItem = "PullItem";

    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;

    public ItemAnimator CurrentItemAnimation => _handAnimator;

    private void OnEnable()
    {
        _hotbarDisplay.OnItemSwitched += Init;
    }

    private void OnDisable()
    {
        _hotbarDisplay.OnItemSwitched -= Init;
    }

    private void Update()
    {
        if(_handAnimator != null)
        {
            if (_starterAssets.move != Vector2.zero)
                _handAnimator.HandAnimator.SetFloat(_speed, (_firstPersonController.Speed / 10));
            else
                _handAnimator.HandAnimator.SetFloat(_speed, 0);
        }
    }

    public void Init(InventorySlotUI slotUI)
    {
        _previousItemData = _currentItemData;

        if (slotUI.AssignedInventorySlot.ItemData != null)
            _currentItemData = slotUI.AssignedInventorySlot.ItemData;
        else
            _currentItemData = _defoultItem.ItemData;

        if (_currentItemData != _previousItemData)
            PullItemAnimation();
    }

    public void PickUp()
    {
        _handAnimator.HandAnimator.SetTrigger(_pickUp);
    }

    public void Build()
    {
        TurnOffAnimations();
        _handAnimator.HandAnimator.SetBool(_build, true);
    }

    public void Hit(bool isActive)
    {
        if(isActive)
            _handAnimator.HandAnimator.SetTrigger(_hits[0]);
        else
            _handAnimator.HandAnimator.SetTrigger(_hits[1]);
    }

    public void Aim(bool value)
    {
        _handAnimator.HandAnimator.SetBool(_aim, value);
    }

    public void PullItemAnimation()
    {
        if (_currentItemData == null || !_itemsAnimator.Any(item => _currentItemData == item.ItemData))
            SwitchToDefaultItem();
        else
            SwitchToItem(_currentItemData);
    }

    private void SwitchToDefaultItem()
    {
        if (_handAnimator != _defoultItem)
        {
            _handAnimator.ToggleLayer(false);
            _handAnimator = _defoultItem;
            _handAnimator.ToggleLayer(true);
            _handAnimator.HandAnimator.SetTrigger(_pullItem);
        }

        return;
    }

    private void SwitchToItem(InventoryItemData itemData)
    {
        foreach (var item in _itemsAnimator)
        {
            item.ToggleLayer(false);
        }

        foreach (var item in _itemsAnimator)
        {
            if (itemData == item.ItemData)
            {
                item.ToggleLayer(true);

                if (_handAnimator != item)
                {
                    _handAnimator = item;
                    _handAnimator.HandAnimator.SetTrigger(_pullItem);
                }
            }
            else
            {
                if(_handAnimator != null) 
                {
                    if (_handAnimator.IndexLayer == item.IndexLayer || _handAnimator.HandAnimator == item.HandAnimator)
                        item.ToggleItem(false);
                    else
                        item.ToggleAnimator(false);
                }
            }
        }
    }

    public void TurnOffAnimations()
    {
        _handAnimator.HandAnimator.SetBool(_build, false);
    }
}
