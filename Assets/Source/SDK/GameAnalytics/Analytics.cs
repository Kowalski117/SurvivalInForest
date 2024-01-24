using GameAnalyticsSDK;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    public void OnStart(string progression)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, progression);
    }

    public void OnComplete(string progression)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, progression);
    }

    public void OnFail(string progression)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, progression);
    }

    public void OnResourceEvent(int amount, string resourceType, string loot, string chest)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, resourceType, amount, loot, chest);
    }

    public void OnAdClickedEvent()
    {
        GameAnalytics.NewDesignEvent("rewardtype-ad-click");
    }

    public void OnAdOfferEvent()
    {
        GameAnalytics.NewDesignEvent("rewardtype-ad-offer");
    }
}
