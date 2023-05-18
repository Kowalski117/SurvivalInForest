using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
    [SerializeField] private Slot _clickedSlot;
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _nameItem;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _exitButton;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(OnExitButton);
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(OnExitButton);
    }

    public void SetItemInfo(Slot clickedSlot)
    {
        _nameItem.text = clickedSlot.ItemInSlot.Name;  
        _description.text = clickedSlot.ItemInSlot.Description;
    }

    public void Open()
    {
        _panel.SetActive(true);
    }

    private void OnExitButton()
    {
        _panel.SetActive(false);
    }
}
