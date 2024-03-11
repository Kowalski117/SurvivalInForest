using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
//using CrazyGames;

public class InitializationSDK : MonoBehaviour
{
    [SerializeField] private LocalizationHandler _localization;
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private SavingGame _saveGame;

    private string _defaultLanguage = "en";

    private void Awake()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
        InitAnalytics();
#endif
    }

    private IEnumerator Start()
    {
        yield return null;
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize(OnInitializedYG);
#endif
        //#if CRAZY_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        //        yield return CrazySDK.Instance.IsInitialized;
        //        OnInitializedCG();
        //#endif
    }

    private void OnInitializedYG()
    {
        if (ES3.KeyExists(ConstantsSDK.LanguageIndex))
            _localization.SetLanguageIndex(ES3.Load<int>(ConstantsSDK.LanguageIndex));
        else
        {
            _localization.SetLanguageString(YandexGamesSdk.Environment.i18n.lang);
            ES3.Save(ConstantsSDK.Language, YandexGamesSdk.Environment.i18n.lang);
        }

        if (PlayerAccount.IsAuthorized)
            _saveGame.GetCloudSaveData();

        SwitchScene();
    }

    private void OnInitializedCG()
    {
        if (ES3.KeyExists(ConstantsSDK.LanguageIndex))
            _localization.SetLanguageIndex(ES3.Load<int>(ConstantsSDK.LanguageIndex));
        else
        {
            _localization.SetLanguageString(_defaultLanguage);
            ES3.Save(ConstantsSDK.Language, _defaultLanguage);
        }

        SwitchScene();
    }

    private void InitAnalytics()
    {
        GameAnalytics.Initialize();
    }

    private void SwitchScene()
    {
        if (ES3.KeyExists(SaveLoadConstants.IsCutscenePlayed) && ES3.Load<bool>(SaveLoadConstants.IsCutscenePlayed) == true)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
