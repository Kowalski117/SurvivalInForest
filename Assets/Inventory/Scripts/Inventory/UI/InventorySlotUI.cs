using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image _imageSprite;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private InventorySlot _assignedInventorySlot;

    private Button _button;

    public InventoryDisplay ParentDisplay { get; private set; }
    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        _button = GetComponent<Button>();
        CleanSlot();
        _imageSprite.preserveAspect = true;
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnUISlotClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnUISlotClick);
    }

    private void Update()
    {
        UpdateUiSlot();
    }

    public void Init(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            _imageSprite.sprite = slot.ItemData.Icon;
            _imageSprite.color = Color.white;
            if(slot.Size >= 1)
                _itemCount.text = slot.Size.ToString();
        }
        else
        {
            CleanSlot();
        }
    }

    public void UpdateUiSlot()
    {
        if (_assignedInventorySlot != null)
            UpdateUISlot(_assignedInventorySlot);

    }

    public void CleanSlot()
    {
        _assignedInventorySlot?.ClearSlot();
        _imageSprite.sprite = null;
        _imageSprite.color = Color.clear;
        _itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}
