using UnityEngine;

public class InitialTutorial : MonoBehaviour
{
    [SerializeField] private TutorialDataObject _tutorialData;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private TutorialHandler _handler;

    private void OnEnable()
    {
        _loadPanel.OnDeactivated += Show;
    }

    private void OnDisable()
    {
        _loadPanel.OnDeactivated -= Show;
    }

    private void Show()
    {
        _handler.UpdateInfo(_tutorialData);
    }
}
