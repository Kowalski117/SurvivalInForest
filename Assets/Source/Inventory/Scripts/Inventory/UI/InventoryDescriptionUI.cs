using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescriptionUI : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _descriptionText;

    private void Awake()
    {
        ResetDescription();
    }

    private void OnEnable()
    {
        //InventorySlotUI.OnItemClicked += SetDescription;
    }

    private void OnDisable()
    {
        //InventorySlotUI.OnItemClicked -= SetDescription;
    }

    public void ResetDescription()
    {
        _iconImage.gameObject.SetActive(false);
        _titleText.text = "";
        _descriptionText.text = "";
    }

    public void SetDescription(InventorySlotUI inventorySlotUI)
    {
        if(inventorySlotUI.AssignedInventorySlot.ItemData != null)
        {
            _iconImage.gameObject.SetActive(true);
            _iconImage.sprite = inventorySlotUI.AssignedInventorySlot.ItemData.Icon;
            _titleText.text = inventorySlotUI.AssignedInventorySlot.ItemData.DisplayName;
            _descriptionText.text = inventorySlotUI.AssignedInventorySlot.ItemData.Description;
        }
        else
        {
            ResetDescription();
        }

    }
}
