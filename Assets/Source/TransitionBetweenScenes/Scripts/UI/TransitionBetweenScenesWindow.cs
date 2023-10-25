using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionBetweenScenesWindow : ScreenUI
{
    [SerializeField] private TMP_Text _nameSceneText;
    [SerializeField] private Button _transitionButton;
    [SerializeField] private Button _exitButton;

    public event UnityAction OnTransitionButton;
    public event UnityAction OnExitButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        _transitionButton.onClick.AddListener(TransitionButtonClick);
        _exitButton.onClick.AddListener(ExitButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _transitionButton.onClick.RemoveListener(TransitionButtonClick);
        _exitButton.onClick.RemoveListener(ExitButtonClick);
    }

    public void SetNameScene(string sceneName)
    {
        _nameSceneText.text = sceneName;
    }

    private void TransitionButtonClick()
    {
        OnTransitionButton?.Invoke();
    }

    private void ExitButtonClick()
    {
        OnExitButton?.Invoke();
    }
}
