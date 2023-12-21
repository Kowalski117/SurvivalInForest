using System.Collections;
using Agava.VKGames;
using Agava.YandexGames;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationSDK : MonoBehaviour
{
    [SerializeField] private LocalizationHandler _localization;
    [SerializeField] private YandexAds _yandexAds;
    [SerializeField] private SaveGame _saveGame;

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
        yield return new WaitForSeconds(0.5f);
    }
#endif

    //#if  VK_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    //    private IEnumerator Start()
    //    {
    //        yield return VKGamesSdk.Initialize(onSuccessCallback: () => Debug.Log($"Initialized: {VKGamesSdk.Initialized}"));
    //    }
    //#endif

#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    private void OnInitialized()
    {
        if (PlayerPrefs.HasKey(ConstantsSDK.Language))
            _localization.SetLanguageString(PlayerPrefs.GetString(ConstantsSDK.Language));
        else
            _localization.SetLanguageString(YandexGamesSdk.Environment.i18n.lang);

        if (_yandexAds != null)
            _yandexAds.ShowInterstitial();

        if (PlayerAccount.IsAuthorized)
            _saveGame.GetCloudSaveData();

        if (ES3.KeyExists(SaveLoadConstants.IsCutscenePlayed) && ES3.Load<bool>(SaveLoadConstants.IsCutscenePlayed) == true)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

        //if (ES3.KeyExists(SaveLoadConstants.IsNewGame) && ES3.Load<bool>(SaveLoadConstants.IsNewGame) == true)
        //else
    }
#endif

    //#if VK_GAMES && UNITY_WEBGL && !UNITY_EDITOR
    //    private void OnInitialized()
    //    {
    //        if (PlayerPrefs.HasKey(ConstantsSDK.Language))
    //            _localization.SetLanguageString(PlayerPrefs.GetString(ConstantsSDK.Language));
    //        else
    //            _localization.SetLanguageString(YandexGamesSdk.Environment.i18n.lang);

    //        if (_yandexAds != null)
    //            _yandexAds.ShowInterstitial();

    //        if (PlayerAccount.IsAuthorized)
    //            SaveGame.GetCloudSaveData();

    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    }
    //#endif
}
