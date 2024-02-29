using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScreen : MenuScreen
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _startButton;

    [SerializeField] private LoadPanel _loadPanel;

    private int _nextSceneIndex = 1;

    public event Action OnResumeGame;
    public event Action OnStartNewGame;

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

    private void ResumeButtonClick()
    {
        OnResumeGame?.Invoke();
        _loadPanel.StartLoadLastSave();
    }

    private void StartButtonClick()
    {
        OnStartNewGame?.Invoke();
        SavingGame.Delete();
        _loadPanel.StartLoad(SceneManager.GetActiveScene().buildIndex + _nextSceneIndex);
    }
}
