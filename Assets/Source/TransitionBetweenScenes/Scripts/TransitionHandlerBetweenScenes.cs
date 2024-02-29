using UnityEngine;

public class TransitionHandlerBetweenScenes : MonoBehaviour
{
    [SerializeField] private SceneSwitchWindow _transitionWindow;
    [SerializeField] private SavingGame _saveGame;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private int _transitionSceneIndex;

    private bool _isActive = true;

    private void OnEnable()
    {
        _transitionWindow.OnButtonExited += ExitScreen;
        _transitionWindow.OnTransitionedButton += TransitionButton;
    }

    private void OnDisable()
    {
        _transitionWindow.OnButtonExited -= ExitScreen;
        _transitionWindow.OnTransitionedButton -= TransitionButton;
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
        _transitionWindow.Toggle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() && _isActive)
        {
            _transitionWindow.Toggle();
            _transitionWindow.SetNameScene(_loadPanel.GetNameSettingsScreen(_transitionSceneIndex));
            _isActive = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>())
            _isActive = true;
    }
}
