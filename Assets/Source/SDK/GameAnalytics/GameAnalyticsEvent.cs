using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsEvent : MonoBehaviour
{
    private int _defoultCount = -1;

    public void OnStart(string progression)
    {
#if UNITY_WEBGL && !UNITY_EDITOR

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, progression);
#endif
    }

    public void OnComplete(string progression, int count = -1)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if(count != _defoultCount)
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, progression, count);
        else
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, progression);
#endif
    }

    public void OnFail(string progression, int count = -1)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (count != _defoultCount)
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, progression, count);
        else
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, progression);
#endif
    }

    public void OnResourceEvent(int amount, string resourceType, string loot, string chest)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, resourceType, amount, loot, chest);
#endif
    }

    public void OnAdClickedEvent()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("rewardtype-ad-click");
#endif
    }

    public void OnAdOfferEvent()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameAnalytics.NewDesignEvent("rewardtype-ad-offer");
#endif
    }
}
