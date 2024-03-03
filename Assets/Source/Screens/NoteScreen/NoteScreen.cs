using TMPro;
using UnityEngine;

public class NoteScreen : ScreenUI
{
    [SerializeField] private TMP_Text _letterText;
    [SerializeField] private float _textIndentPercentage;

    public void UpdateLetterText(string letterText)
    {
        OpenWindow();
        _letterText.text = "<line-indent="+_textIndentPercentage+"%>"+letterText;
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
        PlayerInputHandler.ToggleScreenPlayerInput(true);
    }
}
