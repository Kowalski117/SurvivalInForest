using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreen : MenuScreen
{
    [SerializeField] private SavingGame _saveGame;
    [SerializeField] private LoadPanel _loadPanel;
    [SerializeField] private ShopScreen _shopScreen;
    [SerializeField] private DailyRewardsScreen _dailyRewardsScreen;
    [SerializeField] private RouletteScreen _rouletteScreen;
    [SerializeField] private UIInventoryHandler _inventoryHandlerUI;
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

    public event Action OnContinuedButton;
    public event Action OnSavedButton;

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
        _shopButton.onClick.AddListener(ToggleShop);
        _dailyRewardsButton.onClick.AddListener(ToggleDailyRewards);
        _rouletteButton.onClick.AddListener(ToggleRoulette);
        _chestBonusButton.onClick.AddListener(OpenChestBonusClick);
        _continueButton.onClick.AddListener(ContinueButtonClick);
        _saveButton.onClick.AddListener(SaveButtonClick);
        _exitMainMenuButton.onClick.AddListener(ExitMainMenuButtonClick);

        _pauseButton.onClick.AddListener(ToggleAll);
        PlayerInputHandler.ScreenPlayerInput.OnPauseScreenToggled += ToggleAll;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _shopButton.onClick.RemoveListener(ToggleShop);
        _dailyRewardsButton.onClick.RemoveListener(ToggleDailyRewards);
        _rouletteButton.onClick.RemoveListener(ToggleRoulette);
        _chestBonusButton.onClick.RemoveListener(OpenChestBonusClick);
        _continueButton.onClick.RemoveListener(ContinueButtonClick);
        _saveButton.onClick.RemoveListener(SaveButtonClick);
        _exitMainMenuButton.onClick.RemoveListener(ExitMainMenuButtonClick);

        _pauseButton.onClick.RemoveListener(ToggleAll);
        PlayerInputHandler.ScreenPlayerInput.OnPauseScreenToggled += ToggleAll;
    }

    public void ToggleAll()
    {
        Toggle();

        if (IsOpenPanel)
        {
            if (_inventoryHandlerUI.IsInventoryOpen)
            {
                PlayerInputHandler.InventoryPlayerInput.Toggle();
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
            CloseAll();
        }
    }

    private void ContinueButtonClick()
    {
        OnContinuedButton?.Invoke();
        ToggleAll();
    }

    private void SaveButtonClick()
    {
        OnSavedButton?.Invoke();
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

    protected override void CloseAll()
    {
        base.CloseAll();
        _shopScreen.Close();
        _dailyRewardsScreen.Close();
        _rouletteScreen.Close();
    }

    private void ToggleShop()
    {
        if (!_shopScreen.IsOpenPanel)
            CloseAll();

        _shopScreen.Toggle();
    }

    private void ToggleDailyRewards()
    {
        if (!_dailyRewardsScreen.IsOpenPanel)
            CloseAll();
        
        _dailyRewardsScreen.Toggle();
    }

    private void ToggleRoulette()
    {
        if (!_rouletteScreen.IsOpenPanel)
            CloseAll();

        _rouletteScreen.Toggle();
    }

    private void OpenChestBonusClick()
    {
        _bonusHandler.Open();
    }
}
