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

    private string _speed = "Speed";
    private string _pickUp = "PickUp";
    private string _build = "Build";
    private string[] _hits = { "Hit", "Hit1"};
    private string _aim = "Aim";
    private string _pullItem = "PullItem";

    private InventoryItemData _currentItemData;
    private InventoryItemData _previousItemData;

    private void Update()
    {
        if(_handAnimator != null)
        {
            if (_starterAssets.move != Vector2.zero)
                _handAnimator.HandAnimator.SetFloat(_speed, (_firstPersonController.Speed / 10));
            else
                _handAnimator.HandAnimator.SetFloat(_speed, 0);
        }

        Init(_hotbarDisplay.GetInventorySlotUI().AssignedInventorySlot.ItemData);
    }

    public void Init(InventoryItemData itemData)
    {
        _previousItemData = _currentItemData;
        
        if(itemData != null)
        {
            _currentItemData = itemData;
        }
        else
        {
            _currentItemData = _defoultItem.ItemData;
        }

        if (_currentItemData != _previousItemData)
        {
            PullItemAnimation();
        }
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

    public void Hit()
    {
        _handAnimator.HandAnimator.SetTrigger(_hits[Random.Range(0, _hits.Length)]);
    }

    public void Aim(bool value)
    {
        _handAnimator.HandAnimator.SetBool(_aim, value);
    }

    public void PullItemAnimation()
    {
        if(!_itemsAnimator.Any(item => _currentItemData == item.ItemData) || _currentItemData == null)
        {
            if (_handAnimator != _defoultItem)
            {
                _handAnimator.ToggleItem(false);
                _handAnimator = _defoultItem;
                _handAnimator.ToggleItem(true);
                _handAnimator.HandAnimator.SetTrigger(_pullItem);
            }

            return;
        }

        foreach (var item in _itemsAnimator)
        {
            if (_currentItemData == item.ItemData)
            {
                item.ToggleItem(true);

                if(_handAnimator != item)
                {
                    _handAnimator = item;
                    _handAnimator.HandAnimator.SetTrigger(_pullItem);
                }
            }
            else
            {
                item.ToggleItem(false);
            }
        }
    }

    public void TurnOffAnimations()
    {
        _handAnimator.HandAnimator.SetBool(_build, false);
    }
}
