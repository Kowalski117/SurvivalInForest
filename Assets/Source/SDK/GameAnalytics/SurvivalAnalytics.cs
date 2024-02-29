using UnityEngine;

public class SurvivalAnalytics : Analytics
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private TargetInteractionHandler _playerInteraction;
    [SerializeField] private InteractorConstruction _interactor;
    [SerializeField] private FishingRod _fishingRod;
    [SerializeField] private BuildTool _buildTool;
    [SerializeField] private CraftingHandler _craftingHandler;
    [SerializeField] private ExchangeHandler _exchangeHandler;
    [SerializeField] private SleepPanel _sleepPanel;
    [SerializeField] private SavingGame _saveGame;

    private void OnEnable()
    {
        _playerHealth.OnDied += DiePlayer;
        _playerHealth.OnRevived += RevivePlayer;

        _playerInteraction.OnBearKilled += KillBear;
        _playerInteraction.OnWolfKilled += KillWolf;
        _playerInteraction.OnDeerKilled += KillDeer;
        _playerInteraction.OnHareKilled += KillHare;

        _playerInteraction.OnTreeBroken += BreakTree;
        _playerInteraction.OnStoneBroken += BreakStone;
        _playerInteraction.OnTreasureBroken += BreakTreasure;

        _buildTool.OnBuildingCompleted += PutBuilding;
        _craftingHandler.OnItemCrafted += CraftItem;
        _exchangeHandler.OnItemExchanged += ExchangeItem;
        _sleepPanel.OnPlayerSleeped += SleepPlayer;

        _timeHandler.OnDayUpdated += UpdateDay;
        _fishingRod.OnFishCaughted += CatchFish;
        _interactor.OnSeedPlanted += PlantSeed;

        _saveGame.OnAutoSaved += SaveAuto;
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= DiePlayer;
        _playerHealth.OnRevived -= RevivePlayer;

        _playerInteraction.OnBearKilled -= KillBear;
        _playerInteraction.OnWolfKilled -= KillWolf;
        _playerInteraction.OnDeerKilled -= KillDeer;
        _playerInteraction.OnHareKilled -= KillHare;

        _playerInteraction.OnTreeBroken -= BreakTree;
        _playerInteraction.OnStoneBroken -= BreakStone;
        _playerInteraction.OnTreasureBroken -= BreakTreasure;

        _buildTool.OnBuildingCompleted -= PutBuilding;
        _craftingHandler.OnItemCrafted -= CraftItem;
        _exchangeHandler.OnItemExchanged -= ExchangeItem;
        _sleepPanel.OnPlayerSleeped -= SleepPlayer;

        _timeHandler.OnDayUpdated -= UpdateDay;
        _fishingRod.OnFishCaughted -= CatchFish;
        _interactor.OnSeedPlanted -= PlantSeed;

        _saveGame.OnAutoSaved -= SaveAuto;
    }

    private void DiePlayer()
    {
        GameAnalytics.OnFail(GameAnalyticsConstants.DiePlayer);
    }

    private void RevivePlayer()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.RevivePlayer);
    }

    private void UpdateDay(int count)
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.DayCount, count);
    }

    private void CatchFish()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.CatchFish);
    }

    private void PlantSeed()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.PlantedSeed);
    }

    private void KillBear()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.KillBear);
    }

    private void KillWolf()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.KillWolf);
    }

    private void KillDeer()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.KillDeer);
    }

    private void KillHare()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.KillHare);
    }

    private void BreakTree()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.BreakTree);
    }

    private void BreakStone()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.BreakStone);
    }

    private void BreakTreasure()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.BreakTreasure);
    }

    private void PutBuilding()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.PutBuilding);
    }

    private void CraftItem()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.CraftItem);
    }

    private void ExchangeItem()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.ExchangeItem);
    }

    private void SleepPlayer()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.SleepPlayer);
    }

    private void SaveAuto()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.SaveAuto);
    }
}
