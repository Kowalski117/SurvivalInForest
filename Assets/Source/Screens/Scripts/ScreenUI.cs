using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] protected PlayerInputHandler PlayerInputHandler;
    [SerializeField] private CanvasGroup _panel;

    private bool _isOpenScreen = false;

    public void OpenScreen()
    {
        _panel.blocksRaycasts = true;
        _panel.alpha = Mathf.Lerp(0, 1, 1);
    }

    public void CloseScreen()
    {
        _panel.blocksRaycasts = false;
        _panel.alpha = _panel.alpha = Mathf.Lerp(1, 0, 1);
    }

    public void ToggleScreen()
    {
        _isOpenScreen = !_isOpenScreen;

        if (_isOpenScreen)
        {
            OpenScreen();
            PlayerInputHandler.SetCursorVisible(true);
            PlayerInputHandler.ToggleAllInput(false);
        }
        else
        {
            CloseScreen();
            PlayerInputHandler.SetCursorVisible(false);
            PlayerInputHandler.ToggleAllInput(true);
        }
    }
}
