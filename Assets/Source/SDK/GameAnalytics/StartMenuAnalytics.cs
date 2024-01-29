using UnityEngine;

public class StartMenuAnalytics : Analytics
{
    [SerializeField] private MainMenuScreen _mainMenuScreen;

    private void OnEnable()
    {
        _mainMenuScreen.OnStartNewGame += StartNewGame;
        _mainMenuScreen.OnResumeGame += ResumeGame;
    }

    private void OnDisable()
    {
        _mainMenuScreen.OnStartNewGame -= StartNewGame;
        _mainMenuScreen.OnResumeGame -= ResumeGame;
    }

    private void ResumeGame()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.ResumeGame);
    }

    private void StartNewGame()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.StartNewGame);
    }
}
