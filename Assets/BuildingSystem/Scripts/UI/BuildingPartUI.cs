using UnityEngine;
using UnityEngine.UI;

public class BuildingPartUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;

    private BuildingData _assignedData;
    private BuildingPanelUI _parentDisplay;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void Init(BuildingData assignedData, BuildingPanelUI parentDisplay)
    {
        _assignedData = assignedData;
        _icon.sprite = _assignedData.Icon;
        _parentDisplay = parentDisplay;
    }

    private void OnButtonClick()
    {
        _parentDisplay.OnClick(_assignedData);
    }
}
