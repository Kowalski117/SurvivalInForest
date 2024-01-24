using TMPro;
using UnityEngine;

public class NoteScreen : ScreenUI
{
    [SerializeField] private TMP_Text _letterText;

    public void UpdateLetterText(string letterText)
    {
        OpenWindow();
        _letterText.text = letterText;
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        ToggleScreen();
        PlayerInputHandler.ToggleScreenPlayerInput(true);
    }
}
