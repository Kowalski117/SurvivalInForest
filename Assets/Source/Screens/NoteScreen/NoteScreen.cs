using TMPro;
using UnityEngine;

public class NoteScreen : ScreenUI
{
    [SerializeField] private TMP_Text _letterText;

    public void UpdateLetterText(string letterText)
    {
        Toggle();
        _letterText.text = letterText;
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
        Toggle();
        PlayerInputHandler.ToggleScreenPlayerInput(true);
    }
}
