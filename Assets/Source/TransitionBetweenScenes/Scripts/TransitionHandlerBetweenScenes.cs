using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandlerBetweenScenes : MonoBehaviour
{
    [SerializeField] private TransitionBetweenScenesWindow _transitionWindow;
    [SerializeField] private SaveGame _saveGame;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private int _transitionSceneIndex;

    private void OnEnable()
    {
        _transitionWindow.OnExitButton += ExitScreen;
        _transitionWindow.OnTransitionButton += TransitionButton;
    }

    private void OnDisable()
    {
        _transitionWindow.OnExitButton -= ExitScreen;
        _transitionWindow.OnTransitionButton -= TransitionButton;
    }

    private void TransitionButton()
    {
        _saveGame.Save();
        ES3.Save(SaveLoadConstants.TransitionScene, true);
        ES3.Save(SaveLoadConstants.IsNewGame, false);
        _loadPanel.LoadScene(_transitionSceneIndex);
    }

    private void ExitScreen()
    {
        _transitionWindow.ToggleScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            _transitionWindow.ToggleScreen();
            _transitionWindow.SetNameScene(_loadPanel.GetNameSettingsScreen(_transitionSceneIndex));
        }
    }
}
