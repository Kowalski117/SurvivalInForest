using UnityEngine;
using UnityEngine.Events;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] protected PlayerInputHandler PlayerInputHandler;
    [SerializeField] private CanvasGroup _panel;

    private bool _isOpenScreen = false;

    public event UnityAction OnOpenScreen;
    public event UnityAction OnCloseScreen;

    public virtual void OnEnable()
    {
        PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied += Close;
    }

    public virtual void OnDisable()
    {
        PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied -= Close;
    }

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
            PlayerInputHandler.BuildTool.DeleteBuilding();
            PlayerInputHandler.SetCursorVisible(true);
            PlayerInputHandler.ToggleAllInput(false);
            OnOpenScreen?.Invoke();
        }
        else
        {
            CloseScreen();
            PlayerInputHandler.SetCursorVisible(false);
            PlayerInputHandler.ToggleAllInput(true);
            OnCloseScreen?.Invoke();
        }
    }

    public void Close()
    {
        _isOpenScreen = false;
        CloseScreen();
        PlayerInputHandler.SetCursorVisible(false);
        PlayerInputHandler.ToggleAllInput(true);
        OnCloseScreen?.Invoke();
    }
}
