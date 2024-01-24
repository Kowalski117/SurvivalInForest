using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionBetweenScenesWindow : ScreenUI
{
    [SerializeField] private TMP_Text _nameSceneText;
    [SerializeField] private Button _transitionButton;

    public event UnityAction OnTransitionButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        _transitionButton.onClick.AddListener(TransitionButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _transitionButton.onClick.RemoveListener(TransitionButtonClick);
    }

    public void SetNameScene(string sceneName)
    {
        _nameSceneText.text = sceneName;
    }

    private void TransitionButtonClick()
    {
        OnTransitionButton?.Invoke();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        //ToggleScreen();
    }
}
