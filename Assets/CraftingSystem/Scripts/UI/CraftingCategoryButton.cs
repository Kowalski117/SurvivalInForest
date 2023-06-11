using UnityEngine;
using UnityEngine.UI;

public class CraftingCategoryButton : MonoBehaviour
{
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private ItemType _itemType;

    private Button _button;

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

    private void OpenCategory()
    {
        _craftingHandler.SwitchCraftingCategory(_itemType);
    }
}
