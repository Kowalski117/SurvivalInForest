using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] protected PlayerHandler PlayerInputHandler;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private Button _exitButton;

    protected bool IsOpenScreen = false;

    public event UnityAction OnOpenScreen;
    public event UnityAction OnCloseScreen;
    public event UnityAction OnExitButton;

    public bool IsOpenPanel => IsOpenScreen;

    private void Awake()
    {
        CloseScreen();
    }

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
        IsOpenScreen = true;
        OnOpenScreen?.Invoke();
        _panel.blocksRaycasts = true;
        _panel.alpha = Mathf.Lerp(0, 1, 1);
    }

    public void OpenWindow()
    {
        ToggleScreen();
        PlayerInputHandler.ToggleScreenPlayerInput(false);
    }

    public void CloseScreen()
    {
        IsOpenScreen = false;
        OnCloseScreen?.Invoke();

        if (_panel)
        {
            _panel.blocksRaycasts = false;
            _panel.alpha = _panel.alpha = Mathf.Lerp(1, 0, 1);
        }
    }

    public virtual void ToggleScreen()
    {
        IsOpenScreen = !IsOpenScreen;

        if (IsOpenScreen)
        {
            OpenScreen();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.BuildTool.DeleteBuilding();
                PlayerInputHandler.ToggleAllInput(false);
                PlayerInputHandler.TogglePersonController(false);
                PlayerInputHandler.SetActiveCollider(false);
                //PlayerInputHandler.ToggleScreenPlayerInput(false);

                PlayerInputHandler.SetCursorVisible(true);
            }
        }
        else
        {
            CloseScreen();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.ToggleAllInput(true);
                //PlayerInputHandler.ToggleScreenPlayerInput(true);
                PlayerInputHandler.TogglePersonController(true);
                PlayerInputHandler.SetActiveCollider(true);
                PlayerInputHandler.SetCursorVisible(false);
            }
        }
    }

    public void Close()
    {
        IsOpenScreen = false;
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
