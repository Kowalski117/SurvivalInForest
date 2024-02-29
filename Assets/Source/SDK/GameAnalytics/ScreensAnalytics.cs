using UnityEngine;

public class ScreensAnalytics : Analytics
{
    [SerializeField] private RouletteScrollHandler _rouletteScrollHandler;
    [SerializeField] private StatsBuff _statsBuff;
    [SerializeField] private StoreHandler _storeHandler;

    private void OnEnable()
    {
        _rouletteScrollHandler.OnScrolling += ScrollRoulette;
        _statsBuff.OnUsed += UseBuff;
        _storeHandler.OnProductBuyed += BuyProduct;
    }

    private void OnDisable()
    {
        _rouletteScrollHandler.OnScrolling -= ScrollRoulette;
        _statsBuff.OnUsed -= UseBuff;
        _storeHandler.OnProductBuyed -= BuyProduct;
    }

    private void ScrollRoulette()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.ScrollRoulette);
    }

    private void UseBuff()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.UseBuff);
    }

    private void BuyProduct()
    {
        GameAnalytics.OnComplete(GameAnalyticsConstants.BuyProduct);
    }
}
