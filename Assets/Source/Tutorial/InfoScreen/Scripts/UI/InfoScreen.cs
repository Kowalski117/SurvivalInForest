using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InfoScreen : ScreenUI
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _image;
    [SerializeField] private Button _exitButton;

    public event UnityAction OnExitButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        _exitButton.onClick.AddListener(ExitButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _exitButton.onClick.RemoveListener(ExitButtonClick);
    }

    public void Init(string nameText, string descriptionText, Sprite sprite)
    {
        _nameText.text = nameText;
        _descriptionText.text = descriptionText;
        _image.sprite = sprite;
    }

    public void ExitButtonClick()
    {
        OnExitButton?.Invoke();
        ToggleScreen();
    }
}
