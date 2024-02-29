using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorItem : Raycast
{
    [SerializeField] private LayerMask _interactionItemLayer;

    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private PlayerHandler _playerInputHandler;
    [SerializeField] private PlayerAnimatorHandler _playerAnimation;
    [SerializeField] private InventoryOperator _inventoryOperator;
    [SerializeField] private UIInventoryHandler _inventoryHandler;

    [SerializeField] private bool _isKeyPickUp = true;
    [SerializeField] private bool _isLargeLiftingArea = false;
    [SerializeField] private float _liftingDelay = 2f;

    private Coroutine _coroutine;
    private WaitForSeconds _liftingWait;

    private ItemPickUp _currentItemPickUp;
    private BeingLiftedObject _currentObjectPickUp;

    private List<ItemPickUp> _itemsPickUp = new List<ItemPickUp>();
    private bool _isRemoveItem = false;
    private bool _isIconFilled = false;
    private bool _isInventoryFull = false;
    private bool _isStartingPick = true;
    private float _lookTimer = 0;
    private int _addAmount = 1;

    public event Action<float> OnTimeUpdated;

    public float LookTimerPracent => _lookTimer / _liftingDelay;

    protected override void Awake()
    {
        base.Awake();
        _liftingWait = new WaitForSeconds(_liftingDelay);
    }

    private void Update()
    {
        HandleInteraction();
        HandleInventoryFull();
    }

    private void OnEnable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickedUp += PickUp;
    }

    private void OnDisable()
    {
        _playerInputHandler.InteractionPlayerInput.OnPickedUp -= PickUp;
    }

    public void UpdateKeyPickUp(bool isKeyPickUp)
    {
        _isKeyPickUp = !isKeyPickUp;
    }

    public void SetLiftingArea(bool isLargeLiftingArea)
    {
        _isLargeLiftingArea = isLargeLiftingArea;
    }

    private void PickUp()
    {
        if ((_currentItemPickUp || _currentObjectPickUp) && _isKeyPickUp)
        {
            _playerAnimation.PickUp();
            StartCoroutine();
        }
    }

    private void HandleInteraction()
    {
        if ((IsRayHittingSomething(_interactionItemLayer, out RaycastHit hitInfo) || _itemsPickUp.Count > 0 && _isLargeLiftingArea) && !_inventoryHandler.IsInventoryOpen)
        {
            if (_isStartingPick && !_isInventoryFull)
            {
                _lookTimer += Time.deltaTime;

                OnTimeUpdated?.Invoke(LookTimerPracent);
                _isRemoveItem = false;

                if (_lookTimer >= _liftingDelay && !_isIconFilled)
                {
                    _isIconFilled = true;

                    if (_itemsPickUp.Count > 0 && _isLargeLiftingArea && !_isRemoveItem)
                        _currentItemPickUp = _itemsPickUp[0];
                    else if (hitInfo.collider != null)
                    {
                        if (hitInfo.collider.TryGetComponent(out ItemPickUp itemPickUp))
                            _currentItemPickUp = itemPickUp;
                        else if (hitInfo.collider.TryGetComponent(out BeingLiftedObject objectPickUp))
                            _currentObjectPickUp = objectPickUp;
                    }

                    if (!_isKeyPickUp)
                        StartCoroutine();
                }
                else
                    _isIconFilled = false;
            }
            else
                ResetLookTimer();
        }
        else
            ResetLookTimer();
    }

    private void HandleInventoryFull()
    {
        if (_isInventoryFull)
        {
            ResetLookTimer();
        }
    }

    private void ResetLookTimer()
    {
        if (_lookTimer >= _liftingDelay || _itemsPickUp.Count == 0)
        {
            _lookTimer = 0;
            OnTimeUpdated?.Invoke(0);
            _isIconFilled = false;
            _isInventoryFull = false;
        }
    }

    private void StartCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(AddInventory());
    }

    private IEnumerator AddInventory()
    {
        _isIconFilled = false;
        _isStartingPick = true;

        if (_currentItemPickUp != null)
        {
            if (!_playerInventoryHolder.AddItem(_currentItemPickUp.ItemData, _addAmount, _currentItemPickUp.Durability))
            {
                _inventoryOperator.StartCreateItem(_currentItemPickUp.ItemData, _currentItemPickUp.ItemData.Durability, _addAmount);
                _isInventoryFull = true;
            }

            if (_itemsPickUp.Contains(_currentItemPickUp))
            {
                _itemsPickUp.Remove(_currentItemPickUp);
                _isRemoveItem = true;
            }

            _currentItemPickUp.PickUp();
            _currentItemPickUp = null;

            _lookTimer = 0;
            OnTimeUpdated?.Invoke(0);
            _isIconFilled = false;
            _isInventoryFull = false;
        }
        else if (_currentObjectPickUp != null)
        {
            foreach (var inventoryData in _currentObjectPickUp.ObjectItemsData.LootRandomItems.Items)
            {
                for (var i = 0; i < inventoryData.Amount; i++)
                {
                    if (!_playerInventoryHolder.AddItem(inventoryData.ItemData, _addAmount, inventoryData.ItemData.Durability))
                        _inventoryOperator.StartCreateItem(inventoryData.ItemData, inventoryData.ItemData.Durability, _addAmount);
                }
            }

            _currentObjectPickUp.PickUp();
            _currentObjectPickUp = null;
        }

        _playerAnimation.PickUp();

        yield return _liftingWait;

        _coroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemPickUp itemPickUp) && itemPickUp.enabled && _isLargeLiftingArea)
        {
            if (!_itemsPickUp.Contains(itemPickUp))
                _itemsPickUp.Add(itemPickUp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ItemPickUp itemPickUp) && _isLargeLiftingArea)
        {
            if (_itemsPickUp.Contains(itemPickUp))
                _itemsPickUp.Remove(itemPickUp);
        }
    }
}
