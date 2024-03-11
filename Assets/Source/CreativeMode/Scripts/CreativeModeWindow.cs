using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreativeModeWindow : MonoBehaviour
{ 
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private PlayerHandler _inputHandler;
    [SerializeField] private UIInventoryHandler _inventoryUI;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private List<InventoryItemData> _itemDataBase;
    [SerializeField] private ItemButton _itemButtonPrefab;
    [SerializeField] private Transform _window;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _minePoint;
    [SerializeField] private Transform _npcPoint;
    [SerializeField] private Transform _waterPoint;

    [SerializeField] private Toggle _speedToggle;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Toggle _godModeToggle;

    private bool _isCreativeModeOpen = false;
    private float _delay = 0.25f;
    private float _defoultSpeed;
    private PlayerInput _playerInput;

    public PlayerInventoryHolder PlayerInventoryHolder => _inventoryHolder;

    private void Start()
    {
        CreateButtons();
        _defoultSpeed = _inputHandler.FirstPersonController.SprintSpeed;
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.CreativeMode.performed += ctx => ToggleWindow();

        _speedToggle.onValueChanged.AddListener(ToggleSpeed);
        _godModeToggle.onValueChanged.AddListener(ToggleGodMode);
    }

    private void OnDisable()
    {
        _playerInput.Player.CreativeMode.performed -= ctx => ToggleWindow();
        _playerInput.Disable();

        _speedToggle.onValueChanged.RemoveListener(ToggleSpeed);
        _godModeToggle.onValueChanged.RemoveListener(ToggleGodMode);
    }

    public void CreateButtons()
    {
        foreach (var item in _itemDataBase)
        {
            ItemButton itemButton = Instantiate(_itemButtonPrefab, _container);
            itemButton.Init(_inventoryHolder, item);
        }
    }

    public void ToggleWindow()
    {
        _isCreativeModeOpen = !_isCreativeModeOpen;

        if (_isCreativeModeOpen)
        {
            _inputHandler.SetCursorVisible(true);
            _window.gameObject.SetActive(true);
        }
        else
        {
            _inputHandler.SetCursorVisible(false);
            _window.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Set Ids")]
    public void SetItemIds()
    {
        _itemDataBase = new List<InventoryItemData>();

        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").OrderBy(i => i.Id).ToList();

        var hasIdInRange = foundItems.Where(i => i.Id != -1 && i.Id < foundItems.Count).OrderBy(i => i.Id).ToList();
        var hasIdNotInRange = foundItems.Where(i => i.Id != -1 && i.Id >= foundItems.Count).OrderBy(i => i.Id).ToList();
        var noId = foundItems.Where(i => i.Id <= -1).ToList();

        var index = 0;

        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIdInRange.Find(d => d.Id == i);

            if (itemToAdd != null)
            {
                _itemDataBase.Add(itemToAdd);
            }
            else if (index < noId.Count)
            {
                noId[index].SetId(i);
                itemToAdd = noId[index];
                index++;
                _itemDataBase.Add(itemToAdd);
            }
        }

        foreach (var item in hasIdNotInRange)
        {
            _itemDataBase.Add(item);
        }
    }

    public void StartTeleportPlayer(Transform point)
    {
        StartCoroutine(TeleportPlayer(point));
    }

    private IEnumerator TeleportPlayer(Transform point)
    {
        _inputHandler.TogglePersonController(false);
        _inputHandler.gameObject.transform.position = point.position;
        yield return new WaitForSeconds(_delay);
        _inputHandler.TogglePersonController(true);
    }

    private void ToggleSpeed(bool value)
    {
        if (value)
            _inputHandler.FirstPersonController.SprintSpeed = _maxSpeed;
        else
            _inputHandler.FirstPersonController.SprintSpeed = _defoultSpeed;
    }

    private void ToggleGodMode(bool value)
    {
        _playerHealth.SetGodMode(value);
    }
}
