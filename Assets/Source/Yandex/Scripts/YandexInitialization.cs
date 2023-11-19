using System.Collections;
using Agava.YandexGames;
using UnityEngine;

public class YandexInitialization : MonoBehaviour
{
    //[SerializeField] private Localization _localization;
    [SerializeField] private YandexAds _yandexAds;

    private const string Language = "Language";

#if UNITY_WEBGL && !UNITY_EDITOR
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
        yield return new WaitForSeconds(0.5f);

        if (_yandexAds != null)
            _yandexAds.ShowInterstitial();
    }

    private void OnInitialized()
    {
        //if(PlayerPrefs.HasKey(Language))
        //    _localization.SetLanguage(Language);
        //else
        //    _localization.SetLanguage(YandexGamesSdk.Environment.i18n.lang);
    }
#endif
}
