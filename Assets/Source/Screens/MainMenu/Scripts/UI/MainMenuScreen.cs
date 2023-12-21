using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MenuScreen
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _startButton;

    [SerializeField] private LoadPanel _loadPanel;

    protected override void OnEnable()
    {
        base.OnEnable();

        _resumeButton.onClick.AddListener(ResumeButtonClick);
        _startButton.onClick.AddListener(StartButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _resumeButton.onClick.RemoveListener(ResumeButtonClick);
        _startButton.onClick.RemoveListener(StartButtonClick);
    }

    public void ResumeButtonClick()
    {
        _loadPanel.StartLoadLastSave();
    }

    public void StartButtonClick()
    {
        _loadPanel.StartLoad(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
