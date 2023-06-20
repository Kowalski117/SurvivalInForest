public class HotbarDisplay : StaticInventoryDisplay
{
    private int _maxIndexSize = 6;
    private int _currentIndex = 0;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    protected override void Start()
    {
        base.Start();

        _currentIndex = 0;
        _maxIndexSize = Slots.Length - 1;

        Slots[_currentIndex].ToggleHighlight();
    }

    private void Update()
    {
        if (_playerInput.Inventory.MouseWheel.ReadValue<float>() > 0.1f)
            ChangeIndex(1);

        if (_playerInput.Inventory.MouseWheel.ReadValue<float>() < -0.1f)
            ChangeIndex(-1);
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
    }

    private void ChangeIndex(int direction)
    {
        Slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize)
            _currentIndex = 0;

        if (_currentIndex < 0)
            _currentIndex = _maxIndexSize;

        Slots[_currentIndex].ToggleHighlight();
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
    }



    private void UseItem()
    {
        //if (Slots[_currentIndex].AssignedInventorySlot.ItemData != null)
        //    Slots[_currentIndex].AssignedInventorySlot.ItemData.UseItem();
    }

    private void Hotbar(int index)
    {
        SetIndex(index);
    }
}
