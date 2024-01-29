using UnityEngine;

public class ScreensAnalytics : Analytics
{
    [SerializeField] private RouletteScrollHandler _rouletteScrollHandler;
    [SerializeField] private StatsBuff _statsBuff;

    private void OnEnable()
    {
        _rouletteScrollHandler.OnScroll += ScrollRoulette;
        _statsBuff.OnUseBuff += UseBuff;
    }

    private void OnDisable()
    {
        _rouletteScrollHandler.OnScroll -= ScrollRoulette;
        _statsBuff.OnUseBuff -= UseBuff;
    }

    private void ScrollRoulette()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.ScrollRoulette);
    }

    private void UseBuff()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.UseBuff);
    }
}
