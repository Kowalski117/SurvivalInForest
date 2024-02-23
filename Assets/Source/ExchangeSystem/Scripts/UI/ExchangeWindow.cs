using TMPro;
using UnityEngine;

public class ExchangeWindow : MonoBehaviour
{
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private AnimationUI _panel;
    [SerializeField] private TMP_Text _nameText;

    private bool _isShopOpen = false;

    public bool IsShopOpen => _isShopOpen;

    private void Awake()
    {
        Close();
    }

    private void OnEnable()
    {
        _inventoryHandler.OnInventoryClosed += Close;
    }

    private void OnDisable()
    {
        _inventoryHandler.OnInventoryClosed -= Close;
    }

    public void Open()
    {
        _panel.SetActive(true);

        if (_inventoryHandler.IsInventoryOpen)
            _panel.OpenAnimation();

        _isShopOpen = true;
    }

    public void Close()
    {
        _panel.CloseAnimation();
        _panel.SetActive(false);
        _isShopOpen = false;
    }

    public void SetTitleText(string title)
    {
        _nameText.text = title;
    }
}
