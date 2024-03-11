using UnityEngine;

[RequireComponent(typeof(GameAnalyticsEvent))]
public class Analytics : MonoBehaviour
{
    protected GameAnalyticsEvent GameAnalytics;

    protected virtual void Awake()
    {
        GameAnalytics = GetComponent<GameAnalyticsEvent>();
    }
}
