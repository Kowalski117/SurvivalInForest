using UnityEngine;

public class BuildingRemoveModeView : MonoBehaviour
{
    [SerializeField] private BuildTool _tool;
    [SerializeField] private Transform _panel;

    private void OnEnable()
    {
        _tool.OnDeleteModeChanged += TogglePanel;
    }

    private void OnDisable()
    {
        _tool.OnDeleteModeChanged -= TogglePanel;
    }

    private void TogglePanel(bool toggle)
    {
        if(toggle)
            _panel.gameObject.SetActive(true);
        else
             _panel.gameObject.SetActive(false);
    }
}
