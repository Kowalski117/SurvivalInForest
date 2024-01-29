using UnityEngine;

public class Analytics : MonoBehaviour
{
    protected GameAnalyticsEvent GameAnalytics;

    protected virtual void Awake()
    {
        GameAnalytics = GetComponent<GameAnalyticsEvent>();
    }
}
