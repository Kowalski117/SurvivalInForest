using UnityEngine;
using UnityEngine.UI;

public class CraftingCategoryButton : MonoBehaviour
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private Image _image;
    [SerializeField] private Image _frameImage;

    private Button _button;
    private Color _defoutColor = Color.white;
    private Color _selectColor = Color.black;

    public ItemType ItemType => _itemType;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OpenCategory);
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OpenCategory);
    }

    public void ToggleButton(bool isActive)
    {
        if(isActive)
        {
            _frameImage.gameObject.SetActive(true);
            _image.color = _selectColor;
        }
        else
        {
            _frameImage.gameObject.SetActive(false);
            _image.color = _defoutColor;
        }
    }

    private void OpenCategory()
    {
        _craftingHandler.SwitchCraftingCategory(_itemType);
    }
}
