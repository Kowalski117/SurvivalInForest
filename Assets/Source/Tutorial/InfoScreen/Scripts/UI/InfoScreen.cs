using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : ScreenUI
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _resumeButton;

    public event Action OnButtonResumed;

    protected override void OnEnable()
    {
        base.OnEnable();
        _resumeButton.onClick.AddListener(ResumeButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resumeButton.onClick.RemoveListener(ResumeButtonClick);
    }

    public void Init(string nameText, string descriptionText, Sprite sprite)
    {
        _nameText.text = nameText;
        _descriptionText.text = descriptionText;
        _image.sprite = sprite;
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Toggle();
    }

    private void ResumeButtonClick()
    {
        OnButtonResumed?.Invoke();
    }
}
