using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] protected PlayerInputHandler PlayerInputHandler;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private Button _exitButton;

    private bool _isOpenScreen = false;

    public event UnityAction OnOpenScreen;
    public event UnityAction OnCloseScreen;
    public event UnityAction OnExitButton;

    public bool IsOpenScreen => _isOpenScreen;

    protected virtual void OnEnable()
    {
        if (PlayerInputHandler)
            PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied += Close;

        if(_exitButton)
            _exitButton.onClick.AddListener(ExitButtonClick);
    }

    protected virtual void OnDisable()
    {
        if (PlayerInputHandler)
            PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied -= Close;

        if (_exitButton)
            _exitButton.onClick.RemoveListener(ExitButtonClick);
    }

    public void OpenScreen()
    {
        _isOpenScreen = true;
        OnOpenScreen?.Invoke();
        _panel.blocksRaycasts = true;
        _panel.alpha = Mathf.Lerp(0, 1, 1);
    }

    public void CloseScreen()
    {
        _isOpenScreen = false;
        OnCloseScreen?.Invoke();
        _panel.blocksRaycasts = false;
        _panel.alpha = _panel.alpha = Mathf.Lerp(1, 0, 1);
    }

    public void ToggleScreen()
    {
        _isOpenScreen = !_isOpenScreen;

        if (_isOpenScreen)
        {
            OpenScreen();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.BuildTool.DeleteBuilding();
                PlayerInputHandler.ToggleAllInput(false);

                if (_inventoryHandler && !_inventoryHandler.IsInventoryOpen)
                    PlayerInputHandler.SetCursorVisible(true);
            }
        }
        else
        {
            CloseScreen();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.ToggleAllInput(true);

                if(_inventoryHandler && !_inventoryHandler.IsInventoryOpen)
                    PlayerInputHandler.SetCursorVisible(false);
            }
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

    protected virtual void ExitButtonClick()
    {
        OnExitButton?.Invoke();
    }
}
