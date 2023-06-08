using UnityEngine;
using UnityEngine.UI;

public class CraftingCategoryButton : MonoBehaviour
{
    [SerializeField] private Transform _containerForSlots;

    private Button _button;

    public Transform ContainerForSlots => _containerForSlots;

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
        _containerForSlots.gameObject.SetActive(true);
    }
}
