using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] protected PlayerHandler PlayerInputHandler;
    [SerializeField] private bool _isUnplugScreenInput;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private AnimationUI _animationUI;
    [SerializeField] private Button _exitButton;

    protected bool IsOpenScreen = false;

    public event Action OnScreenOpened;
    public event Action OnScreenClosed;
    public event Action OnButtonExited;

    public bool IsOpenPanel => IsOpenScreen;

    protected virtual void Awake()
    {
        Close();
    }

    protected virtual void OnEnable()
    {
        if (PlayerInputHandler)
            PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied += CloseDied;

        if(_exitButton)
            _exitButton.onClick.AddListener(ExitButtonClick);
    }

    protected virtual void OnDisable()
    {
        if (PlayerInputHandler)
            PlayerInputHandler.SurvivalHandler.PlayerHealth.OnDied -= CloseDied;

        if (_exitButton)
            _exitButton.onClick.RemoveListener(ExitButtonClick);
    }

    public void Open()
    {
        IsOpenScreen = true;
        OnScreenOpened?.Invoke();

        if (_animationUI)
            _animationUI.Open();
        else
            SetCanvasGroup(true);
    }

    public void Close()
    {
        if (!_panel && !_animationUI)
            return;

        IsOpenScreen = false;
        OnScreenClosed?.Invoke();

        if (_animationUI)
            _animationUI.Close();
        else
            SetCanvasGroup(false);
    }

    public virtual void Toggle()
    {
        IsOpenScreen = !IsOpenScreen;

        if (IsOpenScreen)
        {
            Open();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.BuildTool.DeleteBuilding();
                PlayerInputHandler.ToggleAllInput(false);
                PlayerInputHandler.TogglePersonController(false);
                PlayerInputHandler.SetActiveCollider(false);

                if(_isUnplugScreenInput)
                    PlayerInputHandler.ToggleScreenPlayerInput(false);

                PlayerInputHandler.SetCursorVisible(true);
            }
        }
        else
        {
            Close();

            if (PlayerInputHandler)
            {
                PlayerInputHandler.ToggleAllInput(true);
                PlayerInputHandler.TogglePersonController(true);
                PlayerInputHandler.SetActiveCollider(true);
                PlayerInputHandler.SetCursorVisible(false);

                if (_isUnplugScreenInput)
                    PlayerInputHandler.ToggleScreenPlayerInput(true);
            }
        }
    }

    public void CloseDied()
    {
        IsOpenScreen = false;
        Close();
        PlayerInputHandler.SetCursorVisible(false);
        PlayerInputHandler.ToggleAllInput(true);
        OnScreenClosed?.Invoke();
    }

    protected virtual void ExitButtonClick()
    {
        OnButtonExited?.Invoke();
    }

    private void SetCanvasGroup(bool isActive)
    {
        if (isActive)
        {
            _panel.blocksRaycasts = true;
            _panel.alpha = 1;
        }
        else
        {
            _panel.blocksRaycasts = false;
            _panel.alpha = 0;
        }
    }
}
