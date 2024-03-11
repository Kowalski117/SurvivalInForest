using System;

public class ScreenPlayerInput : PlayerInputAction
{
    public event Action OnPauseScreenToggled;
    public event Action OnShopScreenToggled;
    public event Action OnDailyRewardsScreenToggled;
    public event Action OnQuestJournalToggled;

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerInput.UIScreen.TogglePauseScreen.performed += ctx => TogglePause();
        PlayerInput.UIScreen.ToggleShopScreen.performed += ctx => ToggleShop();
        PlayerInput.UIScreen.ToggleDailyRewardsScreen.performed += ctx => ToggleDailyRewards();
        PlayerInput.UIScreen.ToggleQuestJournal.performed += ctx => ToggleQuestJournal();
    }

    protected override void OnDisable()
    {
        PlayerInput.UIScreen.TogglePauseScreen.performed -= ctx => TogglePause();
        PlayerInput.UIScreen.ToggleShopScreen.performed -= ctx => ToggleShop();
        PlayerInput.UIScreen.ToggleDailyRewardsScreen.performed -= ctx => ToggleDailyRewards();
        PlayerInput.UIScreen.ToggleDailyRewardsScreen.performed -= ctx => ToggleQuestJournal();

         base.OnDisable();
    }

    public void TogglePause()
    {
        OnPauseScreenToggled?.Invoke();
    }

    public void ToggleShop()
    {
        OnShopScreenToggled?.Invoke();
    }

    public void ToggleDailyRewards()
    {
        OnDailyRewardsScreenToggled?.Invoke();
    }

    public void ToggleQuestJournal()
    {
        OnQuestJournalToggled?.Invoke();
    }
}
