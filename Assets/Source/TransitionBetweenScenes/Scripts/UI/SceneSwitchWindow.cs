using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitchWindow : ScreenUI
{
    [SerializeField] private TMP_Text _nameSceneText;
    [SerializeField] private Button _transitionButton;

    public event Action OnTransitionedButton;

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
        OnTransitionedButton?.Invoke();
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
    }
}
