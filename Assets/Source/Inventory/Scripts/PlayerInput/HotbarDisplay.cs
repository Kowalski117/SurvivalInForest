using System;
using UnityEngine;
using UnityEngine.Events;

public class HotbarDisplay : StaticInventoryDisplay
{
    [SerializeField] private SurvivalHandler _survivalHandler;

    private int _maxIndexSize = 6;
    private int _currentIndex = 0;

    private bool _isActive = true;

    private PlayerInput _playerInput;

    public event UnityAction<InventorySlot> OnItemClicked;
    public event UnityAction<InventorySlotUI> OnItemSwitched;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Start()
    {
        _currentIndex = 0;
        _maxIndexSize = Slots.Length - 1;

        Slots[_currentIndex].ToggleHighlight();
        OnItemSwitched?.Invoke(Slots[_currentIndex]);
    }

    private void Update()
    {
        if (_playerInput.Inventory.MouseWheel.ReadValue<float>() > 0.1f)
            ChangeIndex(-1);

        if (_playerInput.Inventory.MouseWheel.ReadValue<float>() < -0.1f)
            ChangeIndex(1);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerInput.Enable();
        _playerInput.Inventory.Hotbar1.performed += ctx => Hotbar(0);
        _playerInput.Inventory.Hotbar2.performed += ctx => Hotbar(1);
        _playerInput.Inventory.Hotbar3.performed += ctx => Hotbar(2);
        _playerInput.Inventory.Hotbar4.performed += ctx => Hotbar(3);
        _playerInput.Inventory.Hotbar5.performed += ctx => Hotbar(4);
        _playerInput.Inventory.Hotbar6.performed += ctx => Hotbar(5);
        _playerInput.Inventory.UseItem.performed += ctx => UseItem();
        MouseInventoryItem.OnUpdatedSlots += UpdateSlot;

        foreach (var slot in Slots)
        {
            slot.OnItemClicked += HandleSlotSelected;
            slot.OnItemUpdate += UpdateSlot;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerInput.Inventory.Hotbar1.performed -= ctx => Hotbar(0);
        _playerInput.Inventory.Hotbar2.performed -= ctx => Hotbar(1);
        _playerInput.Inventory.Hotbar3.performed -= ctx => Hotbar(2);
        _playerInput.Inventory.Hotbar4.performed -= ctx => Hotbar(3);
        _playerInput.Inventory.Hotbar5.performed -= ctx => Hotbar(4);
        _playerInput.Inventory.Hotbar6.performed -= ctx => Hotbar(5);
        _playerInput.Inventory.UseItem.performed -= ctx => UseItem();
        _playerInput.Disable();
        MouseInventoryItem.OnUpdatedSlots += UpdateSlot;

        foreach (var slot in Slots)
        {
            slot.OnItemClicked -= HandleSlotSelected;
            slot.OnItemUpdate -= UpdateSlot;
        }
    }

    private void UseItem()
    {
        if (_isActive)
        {
            OnItemClicked?.Invoke(Slots[_currentIndex].AssignedInventorySlot);
        }
    }

    public InventorySlotUI GetInventorySlotUI()
    {
        return Slots[_currentIndex];
    }

    public void ToggleHotbarDisplay(bool isActive)
    {
        Slots[_currentIndex].ToggleHighlight();
        _isActive = isActive;
    }

    private void ChangeIndex(int direction)
    {
        if (_isActive)
        {
            Slots[_currentIndex].TurnOffHighlight();
            _currentIndex += direction;

            if (_currentIndex > _maxIndexSize)
                _currentIndex = 0;

            if (_currentIndex < 0)
                _currentIndex = _maxIndexSize;

            Slots[_currentIndex].ToggleHighlight();
            OnItemSwitched?.Invoke(Slots[_currentIndex]);
        }
    }

    private void SetIndex(int newIndex)
    {
        Slots[_currentIndex].ToggleHighlight();

        if (newIndex < 0)
            newIndex = _maxIndexSize;

        if (newIndex > _maxIndexSize)
            newIndex = 0;

        _currentIndex = newIndex;

        Slots[_currentIndex].ToggleHighlight();
        OnItemSwitched?.Invoke(Slots[_currentIndex]);
    }

    private void Hotbar(int index)
    {
        if(_isActive)
            SetIndex(index);
    }

    private void HandleSlotSelected(InventorySlotUI clickedSlot)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i] == clickedSlot && clickedSlot.AssignedInventorySlot.ItemData != null)
            {
                _currentIndex = i;
                UpdateSlot(clickedSlot);
            }
        }
    }

    private void UpdateSlot(InventorySlotUI slot)
    {
        UpdateSlot();
    }

    private void UpdateSlot()
    {
        OnItemSwitched?.Invoke(Slots[_currentIndex]);
    }
}
