using StarterAssets;
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

    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;

    public ItemAnimator CurrentItemAnimation => _handAnimator;

    private void Awake()
    {
        SwitchToItem(_currentItemData);
    }

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
            {
                _handAnimator.HandAnimator.SetFloat(PlayerAnimationConstants.Speed,(_firstPersonController.Speed / 
                    _firstPersonController.SprintSpeed));

            }
            else
                _handAnimator.HandAnimator.SetFloat(PlayerAnimationConstants.Speed, 0);
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
        _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.PickUp);
    }

    public void Build()
    {
        TurnOffAnimations();
        _handAnimator.HandAnimator.SetBool(PlayerAnimationConstants.Build, true);
    }

    public void ThrowFishingRod()
    {
        _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.ThrowFishingRod);
    }

    public void SwingFishingRod()
    {
        _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.SwingFishingRod);
    }

    public void Hit(bool isActive)
    {
        if (isActive)
        {
            _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.Hit);
        }
        else
        {
            _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.HitInAir);
        }
    }

    public void Aim(bool value)
    {
        _handAnimator.HandAnimator.SetBool(PlayerAnimationConstants.Aim, value);
    }

    public void PullItemAnimation()
    {
        if (_currentItemData == _defoultItem.ItemData || !_itemsAnimator.Any(item => _currentItemData == item.ItemData))
            SwitchToDefaultItem();
        else 
            SwitchToItem(_currentItemData);
    }

    private void SwitchToDefaultItem()
    {
        if (_handAnimator != _defoultItem)
        {
            if(_handAnimator != null ) 
                _handAnimator.ToggleLayer(false);

            _handAnimator = _defoultItem;
            _handAnimator.ToggleLayer(true);
            _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.PullItem);
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
                    _handAnimator.HandAnimator.SetTrigger(PlayerAnimationConstants.PullItem);
                }
            }
            else
            {
                if(_handAnimator != null && _handAnimator != _defoultItem) 
                {
                    if (_handAnimator.IndexLayer == item.IndexLayer || _handAnimator.HandAnimator == item.HandAnimator)
                    {
                        item.ToggleItem(false);
                    }
                    else
                    {
                        item.ToggleAnimator(false);
                    }

                }
            }
        }
    }

    public void TurnOffAnimations()
    {
        _handAnimator.HandAnimator.SetBool(PlayerAnimationConstants.Build, false);
    }
}
