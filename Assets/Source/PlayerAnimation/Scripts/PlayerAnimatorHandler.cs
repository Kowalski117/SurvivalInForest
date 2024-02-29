using StarterAssets;
using System.Linq;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssets;
    [SerializeField] private HotbarDisplay _hotbarDisplay;
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;

    [SerializeField] private ItemAnimator[] _items;
    [SerializeField] private ItemAnimator _defoultItem;

    private ItemAnimator _hand;

    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;

    public ItemAnimator CurrentItemAnimation => _hand;

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
        if(_hand != null)
        {
            if (_starterAssets.Move != Vector2.zero) 
            {
                _hand.HandAnimator.SetFloat(PlayerAnimationConstants.Speed,(_firstPersonController.Speed / 
                    _firstPersonController.SprintSpeed));
            }
            else
                _hand.HandAnimator.SetFloat(PlayerAnimationConstants.Speed, 0);
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
            PullItem();

    }

    public void PickUp()
    {
        _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.PickUp);
        _playerAudioHandler.PlayPickUpClip();
    }

    public void Build()
    {
        TurnOffAnimations();
        _hand.HandAnimator.SetBool(PlayerAnimationConstants.Build, true);
    }

    public void ThrowFishingRod()
    {
        _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.ThrowFishingRod);
    }

    public void SwingFishingRod()
    {
        _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.SwingFishingRod);
    }

    public void Hit(bool isActive)
    {
        if (isActive)
        {
            _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.Hit);
        }
        else
        {
            _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.HitInAir);
        }
    }

    public void Aim(bool value)
    {
        _hand.HandAnimator.SetBool(PlayerAnimationConstants.Aim, value);
    }

    public void PullItem()
    {
        if (_currentItemData == _defoultItem.ItemData || !_items.Any(item => _currentItemData == item.ItemData))
            SwitchToDefaultItem();
        else 
            SwitchToItem(_currentItemData);
    }

    public void TurnOffAnimations()
    {
        _hand.HandAnimator.SetBool(PlayerAnimationConstants.Build, false);
    }

    private void SwitchToDefaultItem()
    {
        if (_hand != _defoultItem)
        {
            if(_hand != null ) 
                _hand.ToggleLayer(false);

            _hand = _defoultItem;
            _hand.ToggleLayer(true);
            _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.PullItem);
        }
        return;
    }

    private void SwitchToItem(InventoryItemData itemData)
    {
        foreach (var item in _items)
        {
            item.ToggleLayer(false);
        }

        foreach (var item in _items)
        {
            if (itemData == item.ItemData)
            {
                item.ToggleLayer(true);

                if (_hand != item)
                {
                    _hand = item;
                    _hand.HandAnimator.SetTrigger(PlayerAnimationConstants.PullItem);
                }
            }
            else
            {
                if(_hand != null && _hand != _defoultItem) 
                {
                    if (_hand.IndexLayer == item.IndexLayer || _hand.HandAnimator == item.HandAnimator)
                        item.ToggleItem(false);
                    else
                        item.Toggle(false);
                }
            }
        }
    }
}
