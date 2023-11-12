using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreen : MenuScreen
{
    [SerializeField] private SaveGame _saveGame;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private DailyRewardsScreen _dailyRewardsScreen;

    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _dailyRewardsButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitMainMenuButton;

    public event UnityAction OnContinueButton;
    public event UnityAction OnSaveButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _shopButton.onClick.AddListener(ToggleShopScreen);
        _dailyRewardsButton.onClick.AddListener(ToggleDailyRewardsScreen);
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _saveButton.onClick.AddListener(SaveButtonClick);
        _exitMainMenuButton.onClick.AddListener(ExitMainMenuButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleAllScreen;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _shopButton.onClick.RemoveListener(ToggleShopScreen);
        _dailyRewardsButton.onClick.RemoveListener(ToggleDailyRewardsScreen);
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _saveButton.onClick.RemoveListener(SaveButtonClick);
        _exitMainMenuButton.onClick.RemoveListener(ExitMainMenuButtonClick);

        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleAllScreen;
    }

    private void ToggleAllScreen()
    {
        ToggleScreen();

        if (!IsOpenScreen)
            CloseAllScreens();
    }

    private void ContinueButtonClick()
    {
        OnContinueButton?.Invoke();
        ToggleScreen();
    }

    private void SaveButtonClick()
    {
        OnSaveButton?.Invoke();
        _saveGame.Save();
    }

    private void ExitMainMenuButtonClick()
    {
        _loadPanel.StartLoad(0);
    }

    protected override void ExitButtonClick()
    {
        base.ExitButtonClick();
    }

    protected override void CloseAllScreens()
    {
        base.CloseAllScreens();
        _shopScreen.CloseScreen();
        _dailyRewardsScreen.CloseScreen();
    }

    private void ToggleShopScreen()
    {
        if (!_shopScreen.IsOpenScreen)
            CloseAllScreens();

        _shopScreen.ToggleScreen();
    }

    private void ToggleDailyRewardsScreen()
    {
        if (!_dailyRewardsScreen.IsOpenScreen)
            CloseAllScreens();
        
        _dailyRewardsScreen.ToggleScreen();
    }
}
