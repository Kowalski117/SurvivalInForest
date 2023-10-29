using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreativeModeWindow : MonoBehaviour
{ 
    [SerializeField] private PlayerInventoryHolder _inventoryHolder;
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private List<InventoryItemData> _itemDataBase;
    [SerializeField] private ItemButton _itemButtonPrefab;
    [SerializeField] private Transform _window;
    [SerializeField] private Transform _container;

    private bool _isCreativeModeOpen = false;
    private PlayerInput _playerInput;

    public PlayerInventoryHolder PlayerInventoryHolder => _inventoryHolder;

    private void Start()
    {
        CreateButtons();
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Player.CreativeMode.performed += ctx => EnableWindow();
    }

    private void OnDisable()
    {
        _playerInput.Player.CreativeMode.performed -= ctx => EnableWindow();
        _playerInput.Disable();
    }

    public void CreateButtons()
    {
        foreach (var item in _itemDataBase)
        {
            ItemButton itemButton = Instantiate(_itemButtonPrefab, _container);
            itemButton.Init(_inventoryHolder, item);
        }
    }

    public void EnableWindow()
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
}
