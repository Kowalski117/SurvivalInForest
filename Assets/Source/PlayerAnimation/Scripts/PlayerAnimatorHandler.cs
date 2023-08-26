using StarterAssets;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    [SerializeField] private ItemAnimator _handAnimator;
    [SerializeField] private Animator _currentAnimator;
    [SerializeField] private FirstPersonController _firstPersonController;
    [SerializeField] private StarterAssetsInputs _starterAssets;
    [SerializeField] private HotbarDisplay _hotbarDisplay;

    [SerializeField] private ItemAnimator[] _itemsAnimator;
    [SerializeField] private InventoryItemData _defoultItem;

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

    private void Start()
    {
        PullItemAnimation();
    }

    private void Update()
    {
        if (_starterAssets.move != Vector2.zero)
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
        
        if(itemData != null)
        {
            _currentItemData = itemData;
        }
        else
        {
            _currentItemData = _defoultItem;
        }

        if (_currentItemData != _previousItemData)
        {
            PullItemAnimation();
            Debug.Log("È");
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

    public void Hit()
    {
        if(_currentAnimator != null)
        {
            Debug.Log(_currentAnimator);
            _currentAnimator.SetTrigger(_hit);
        }
        else
        {
            Debug.Log(_currentAnimator);
        }
    }

    public void PullItemAnimation()
    {
        foreach (var item in _itemsAnimator)
        {
            if (_currentItemData == item.ItemData)
            {
                item.ToggleItem(true);
                _handAnimator = item;
                _currentAnimator = item.HandAnimator;
                Debug.Log("1");
                _currentAnimator.SetTrigger(_pullItem);
            }
            else
            {
                item.ToggleItem(false);
            }
        }
    }

    //public void RemoveItemAnimationEvent()
    //{
    //    if (_currentItemData == null)
    //        RemoveItemsAnimationEvent();
    //}

    //public void RemoveItemsAnimationEvent()
    //{
    //    foreach (var item in _itemsAnimator)
    //    {
    //        item.ToggleItem(false);
    //    }
    //}

    public void TurnOffAnimations()
    {
        _handAnimator.SetBool(_build, false);
    }
}
