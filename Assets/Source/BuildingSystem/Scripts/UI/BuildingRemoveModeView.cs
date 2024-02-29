using UnityEngine;

public class BuildingRemoveModeView : MonoBehaviour
{
    [SerializeField] private BuildTool _tool;
    [SerializeField] private AnimationUI _animationUI;

    private void Awake()
    {
        _animationUI.Close();
    }

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
            _animationUI.Open();
        else
            _animationUI.Close();
    }
}
