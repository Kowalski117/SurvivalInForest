using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MenuScreen
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _startButton;

    [SerializeField] private LoadPanel _loadPanel;

    public event UnityAction OnResumeGame;
    public event UnityAction OnStartNewGame;

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
        OnResumeGame?.Invoke();
        _loadPanel.StartLoadLastSave();
    }

    public void StartButtonClick()
    {
        OnStartNewGame?.Invoke();
        SaveGame.Delete();
        _loadPanel.StartLoad(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
