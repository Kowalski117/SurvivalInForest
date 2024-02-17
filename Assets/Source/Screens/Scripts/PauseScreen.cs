using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreen : MenuScreen
{
    [SerializeField] private SaveGame _saveGame;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private DailyRewardsScreen _dailyRewardsScreen;
    [SerializeField] private RouletteScreen _rouletteScreen;
    [SerializeField] private UIInventoryHandler _inventoryHandler;
    [SerializeField] private BonusesHandler _bonusHandler;

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _dailyRewardsButton;
    [SerializeField] private Button _rouletteButton;
    [SerializeField] private Button _chestBonusButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _exitMainMenuButton;

    private int _indexMenuScene = 1;

    public event UnityAction OnContinueButton;
    public event UnityAction OnSaveButton;

    protected override void Awake()
    {
        base.Awake();

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        _shopButton.gameObject.SetActive(true);
#endif
#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        _shopButton.gameObject.SetActive(false);
#endif
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _shopButton.onClick.AddListener(ToggleShopScreen);
        _dailyRewardsButton.onClick.AddListener(ToggleDailyRewardsScreen);
        _rouletteButton.onClick.AddListener(ToggleRouletteScreen);
        _chestBonusButton.onClick.AddListener(OpenChestBonusClick);
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _saveButton.onClick.AddListener(SaveButtonClick);
        _exitMainMenuButton.onClick.AddListener(ExitMainMenuButtonClick);

        _pauseButton.onClick.AddListener(ToggleAllScreen);
        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleAllScreen;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _shopButton.onClick.RemoveListener(ToggleShopScreen);
        _dailyRewardsButton.onClick.RemoveListener(ToggleDailyRewardsScreen);
        _rouletteButton.onClick.RemoveListener(ToggleRouletteScreen);
        _chestBonusButton.onClick.RemoveListener(OpenChestBonusClick);
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _saveButton.onClick.RemoveListener(SaveButtonClick);
        _exitMainMenuButton.onClick.RemoveListener(ExitMainMenuButtonClick);

        _pauseButton.onClick.RemoveListener(ToggleAllScreen);
        PlayerInputHandler.ScreenPlayerInput.OnTogglePauseScreen += ToggleAllScreen;
    }

    public void ToggleAllScreen()
    {
        ToggleScreen();

        if (IsOpenPanel)
        {
            if (_inventoryHandler.IsInventoryOpen)
            {
                PlayerInputHandler.InventoryPlayerInput.ToggleInventory();
                PlayerInputHandler.ToggleAllInput(false);
            }

            PlayerInputHandler.ToggleHotbarDisplay(false);
            PlayerInputHandler.ToggleAllParametrs(false);
            PlayerInputHandler.SetActiveCollider(false);
        }
        else
        {
            PlayerInputHandler.ToggleHotbarDisplay(true);
            PlayerInputHandler.ToggleAllParametrs(true);
            PlayerInputHandler.SetActiveCollider(true);
            CloseAllScreens();
        }
    }

    private void ContinueButtonClick()
    {
        OnContinueButton?.Invoke();
        ToggleAllScreen();
    }

    private void SaveButtonClick()
    {
        OnSaveButton?.Invoke();
        _saveGame.Save();
    }

    private void ExitMainMenuButtonClick()
    {
        _loadPanel.StartLoad(_indexMenuScene);
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
        _rouletteScreen.CloseScreen();
    }

    private void ToggleShopScreen()
    {
        if (!_shopScreen.IsOpenPanel)
            CloseAllScreens();

        _shopScreen.ToggleScreen();
    }

    private void ToggleDailyRewardsScreen()
    {
        if (!_dailyRewardsScreen.IsOpenPanel)
            CloseAllScreens();
        
        _dailyRewardsScreen.ToggleScreen();
    }

    private void ToggleRouletteScreen()
    {
        if (!_rouletteScreen.IsOpenPanel)
            CloseAllScreens();

        _rouletteScreen.ToggleScreen();
    }

    private void OpenChestBonusClick()
    {
        _bonusHandler.OpenChest();
    }
}
